#!/usr/bin/env python3
"""
GitHub Issue Sync Status Script
===============================

Updates local task status from GitHub Issue state changes while preserving 
local file format and metadata. Enables one-way synchronization from GitHub 
project management back to local development tasks.

Usage:
    python sync_status.py --file <task_file>
    python sync_status.py --project <project_directory>
    python sync_status.py --directory <tasks_directory>
    python sync_status.py --issue <issue_number> --repo <owner/repo>
"""

import argparse
import json
import os
import sys
from datetime import datetime, timezone
from pathlib import Path
from typing import List, Dict, Any, Optional, Tuple

# Import shared utilities
sys.path.insert(0, str(Path(__file__).parent.parent / "shared"))
from github_utils import (
    ConfigurationManager, GitHubAuthenticator, GitHubClient,
    TaskFileParser, GitHubAuthenticationError, GitHubAPIError,
    get_repository_from_config_or_credentials, find_task_files_with_github_issues
)


class ConflictType:
    """Types of state conflicts between local and GitHub."""
    LOCAL_NEWER = "local_newer" 
    GITHUB_NEWER = "github_newer"
    INCOMPATIBLE_STATES = "incompatible_states"
    MANUAL_REVIEW_NEEDED = "manual_review_needed"


class GitHubStatusSyncer:
    """Main class for syncing GitHub issue status to local task files."""
    
    def __init__(self, project_path: Optional[str] = None, config_override: Optional[Dict] = None):
        self.config_manager = ConfigurationManager(project_path)
        if config_override:
            self.config_manager.config.update(config_override)
        
        self.authenticator = GitHubAuthenticator(self.config_manager)
        self.credentials = None
        self.github_client = None
        self.results = []
        self.conflicts = []
    
    def initialize(self):
        """Initialize GitHub authentication and client."""
        try:
            self.credentials = self.authenticator.get_credentials()
            self.github_client = GitHubClient(self.credentials, self.config_manager)
            print(f"✅ Authenticated as: {self.credentials.username}")
        except GitHubAuthenticationError as e:
            print(f"❌ Authentication failed: {e}")
            sys.exit(1)
    
    def sync_single_file(self, file_path: Path) -> Dict[str, Any]:
        """Sync status for a single task file."""
        print(f"📄 Syncing: {file_path.name}")
        
        try:
            # Parse task file
            task_data = TaskFileParser.parse_task_file(file_path)
            
            # Check if file has GitHub issue metadata
            if not task_data['github_issue_number']:
                print(f"ℹ️  No GitHub issue metadata found in {file_path.name}")
                return {
                    'file': file_path.name,
                    'operation': 'skipped',
                    'reason': 'no_github_issue',
                    'success': True
                }
            
            # Get repository info
            owner, repo = get_repository_from_config_or_credentials(
                self.config_manager, self.credentials
            )
            
            return self._sync_with_github_issue(owner, repo, task_data)
        
        except Exception as e:
            error_msg = f"Error syncing {file_path.name}: {e}"
            print(f"❌ {error_msg}")
            return {
                'file': file_path.name,
                'operation': 'error',
                'error': str(e),
                'success': False
            }
    
    def sync_specific_issue(self, owner: str, repo: str, issue_number: int, 
                           tasks_directory: Optional[Path] = None) -> Dict[str, Any]:
        """Sync a specific GitHub issue with its corresponding local task file."""
        print(f"🔍 Syncing specific issue #{issue_number} from {owner}/{repo}")
        
        try:
            # Get issue from GitHub
            issue = self.github_client.get_issue(owner, repo, issue_number)
            
            # Find corresponding local task file
            task_file = self._find_task_file_for_issue(issue_number, tasks_directory)
            
            if not task_file:
                print(f"ℹ️  No local task file found for issue #{issue_number}")
                return {
                    'issue_number': issue_number,
                    'operation': 'skipped',
                    'reason': 'no_local_file',
                    'success': True
                }
            
            # Parse the task file
            task_data = TaskFileParser.parse_task_file(task_file)
            
            # Sync the status
            return self._sync_task_with_issue(task_data, issue)
        
        except GitHubAPIError as e:
            error_msg = f"Failed to get issue #{issue_number}: {e}"
            print(f"❌ {error_msg}")
            return {
                'issue_number': issue_number,
                'operation': 'error',
                'error': str(e),
                'success': False
            }
    
    def _sync_with_github_issue(self, owner: str, repo: str, task_data: Dict[str, Any]) -> Dict[str, Any]:
        """Sync task file with its corresponding GitHub issue."""
        issue_number = task_data['github_issue_number']
        
        try:
            # Get current issue state from GitHub
            issue = self.github_client.get_issue(owner, repo, issue_number)
            
            return self._sync_task_with_issue(task_data, issue)
            
        except GitHubAPIError as e:
            error_msg = f"Failed to get issue #{issue_number}: {e}"
            print(f"❌ {error_msg}")
            return {
                'file': task_data['file_path'].name,
                'operation': 'error',
                'issue_number': issue_number,
                'error': str(e),
                'success': False
            }
    
    def _sync_task_with_issue(self, task_data: Dict[str, Any], issue) -> Dict[str, Any]:
        """Sync task data with GitHub issue, handling conflicts."""
        file_path = task_data['file_path']
        issue_number = issue.number
        
        # Map GitHub issue state to local task state
        github_state = self._map_github_state_to_task_state(issue.state, issue.assignees)
        local_state = task_data['state']
        
        # Check for conflicts
        conflict = self._detect_state_conflict(local_state, github_state, task_data, issue)
        
        if conflict:
            return self._handle_conflict(task_data, issue, conflict)
        
        # No conflict - proceed with sync
        if local_state == github_state:
            print(f"ℹ️  {file_path.name} already in sync (both {local_state})")
            return {
                'file': file_path.name,
                'operation': 'no_change',
                'issue_number': issue_number,
                'local_state': local_state,
                'github_state': github_state,
                'success': True
            }
        
        # Update local state
        return self._update_local_state(task_data, issue, github_state)
    
    def _detect_state_conflict(self, local_state: str, github_state: str, 
                              task_data: Dict[str, Any], issue) -> Optional[Dict[str, Any]]:
        """Detect conflicts between local and GitHub states."""
        conflict_resolution = self.config_manager.get("github.sync_behavior.conflict_resolution", "manual")
        
        # No conflict if states match
        if local_state == github_state:
            return None
        
        # Check for incompatible state transitions
        incompatible_transitions = [
            ("completed", "open"),  # Local completed but GitHub reopened
            ("cancelled", "open"),   # Local cancelled but GitHub reopened
        ]
        
        if (local_state, github_state) in incompatible_transitions:
            return {
                'type': ConflictType.INCOMPATIBLE_STATES,
                'local_state': local_state,
                'github_state': github_state,
                'description': f"Local task is {local_state} but GitHub issue is {issue.state}"
            }
        
        # Check timestamps to determine which is newer (if available)
        last_synced = task_data.get('last_synced')
        if last_synced and hasattr(issue, 'updated_at'):
            try:
                sync_time = datetime.fromisoformat(last_synced.replace('Z', '+00:00'))
                issue_time = datetime.fromisoformat(issue.updated_at.replace('Z', '+00:00'))
                
                if sync_time > issue_time:
                    return {
                        'type': ConflictType.LOCAL_NEWER,
                        'local_state': local_state,
                        'github_state': github_state,
                        'description': f"Local changes are newer than GitHub updates"
                    }
                elif issue_time > sync_time:
                    return {
                        'type': ConflictType.GITHUB_NEWER,
                        'local_state': local_state,
                        'github_state': github_state,
                        'description': f"GitHub changes are newer than local sync"
                    }
            except (ValueError, AttributeError):
                pass
        
        # Default to manual review if we can't determine precedence
        if conflict_resolution == "manual":
            return {
                'type': ConflictType.MANUAL_REVIEW_NEEDED,
                'local_state': local_state,
                'github_state': github_state,
                'description': f"State mismatch: local={local_state}, github={github_state}"
            }
        
        return None
    
    def _handle_conflict(self, task_data: Dict[str, Any], issue, conflict: Dict[str, Any]) -> Dict[str, Any]:
        """Handle state conflicts based on configuration."""
        file_path = task_data['file_path']
        conflict_resolution = self.config_manager.get("github.sync_behavior.conflict_resolution", "manual")
        
        print(f"⚠️  Conflict detected in {file_path.name}: {conflict['description']}")
        
        if conflict_resolution == "github_wins":
            # Always use GitHub state
            github_state = self._map_github_state_to_task_state(issue.state, issue.assignees)
            return self._update_local_state(task_data, issue, github_state, conflict_resolved=True)
        
        elif conflict_resolution == "local_wins":
            # Keep local state, just update sync timestamp
            updates = {'last_synced': datetime.now().isoformat()}
            success = TaskFileParser.update_task_file(file_path, updates)
            
            return {
                'file': file_path.name,
                'operation': 'local_preserved',
                'issue_number': issue.number,
                'conflict': conflict,
                'success': success
            }
        
        elif conflict_resolution == "smart" and conflict['type'] == ConflictType.GITHUB_NEWER:
            # Use GitHub state if it's newer
            github_state = self._map_github_state_to_task_state(issue.state, issue.assignees)
            return self._update_local_state(task_data, issue, github_state, conflict_resolved=True)
        
        else:
            # Manual resolution - mark for review
            return self._mark_for_manual_review(task_data, issue, conflict)
    
    def _mark_for_manual_review(self, task_data: Dict[str, Any], issue, conflict: Dict[str, Any]) -> Dict[str, Any]:
        """Mark conflict for manual review."""
        file_path = task_data['file_path']
        
        # Add conflict marker to task file
        conflict_marker = f"""
<!-- SYNC CONFLICT DETECTED -->
<!-- Local State: {conflict['local_state']} -->
<!-- GitHub State: {conflict['github_state']} --> 
<!-- Conflict: {conflict['description']} -->
<!-- Please resolve manually and remove this comment -->
"""
        
        # Read current content
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Add conflict marker if not already present
        if "SYNC CONFLICT DETECTED" not in content:
            # Insert after the metadata section
            lines = content.split('\n')
            insert_idx = 0
            for i, line in enumerate(lines):
                if line.startswith('**') and ':' in line:
                    insert_idx = i + 1
                elif insert_idx > 0 and not line.startswith('**') and line.strip():
                    break
            
            lines.insert(insert_idx, conflict_marker)
            
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write('\n'.join(lines))
        
        # Track conflict for reporting
        self.conflicts.append({
            'file': file_path.name,
            'issue_number': issue.number,
            'conflict': conflict
        })
        
        return {
            'file': file_path.name,
            'operation': 'conflict_marked',
            'issue_number': issue.number,
            'conflict': conflict,
            'success': True
        }
    
    def _update_local_state(self, task_data: Dict[str, Any], issue, new_state: str,
                           conflict_resolved: bool = False) -> Dict[str, Any]:
        """Update local task file with new state."""
        file_path = task_data['file_path']
        old_state = task_data['state']
        
        updates = {
            'state': new_state,
            'last_synced': datetime.now().isoformat()
        }
        
        # Add completion date if transitioning to completed
        if new_state == 'completed' and old_state != 'completed':
            if hasattr(issue, 'closed_at') and issue.closed_at:
                updates['completed_date'] = issue.closed_at
        
        success = TaskFileParser.update_task_file(
            file_path, 
            updates,
            backup=self.config_manager.get("github.file_handling.backup_before_sync", False)
        )
        
        operation = "conflict_resolved" if conflict_resolved else "updated"
        
        if success:
            print(f"✅ Updated {file_path.name}: {old_state} → {new_state}")
            return {
                'file': file_path.name,
                'operation': operation,
                'issue_number': issue.number,
                'state_change': f"{old_state} → {new_state}",
                'success': True
            }
        else:
            print(f"❌ Failed to update {file_path.name}")
            return {
                'file': file_path.name,
                'operation': 'update_failed',
                'issue_number': issue.number,
                'error': 'File update failed',
                'success': False
            }
    
    def _map_github_state_to_task_state(self, github_state: str, assignees: List[str]) -> str:
        """Map GitHub issue state to local task state."""
        # Get state mapping from configuration
        state_mapping = self.config_manager.get("github.sync_behavior.state_mapping", {})
        issue_to_task = state_mapping.get("issue_to_task", {
            "open": "ready",
            "closed": "completed"
        })
        
        if github_state == "open":
            # If issue is open and assigned, consider it in-progress
            if assignees and self.config_manager.get("github.sync_behavior.preserve_in_progress", True):
                return "in-progress"
            else:
                return issue_to_task.get("open", "ready")
        elif github_state == "closed":
            return issue_to_task.get("closed", "completed")
        else:
            return issue_to_task.get(github_state, "ready")
    
    def _find_task_file_for_issue(self, issue_number: int, search_directory: Optional[Path] = None) -> Optional[Path]:
        """Find local task file that references the given GitHub issue number."""
        if search_directory and search_directory.exists():
            search_paths = [search_directory]
        else:
            # Default search paths
            search_paths = [
                Path.cwd() / "tasks",
                Path.cwd() / "OrgDocument" / "projects" / "*" / "tasks",
            ]
        
        for search_path in search_paths:
            if '*' in str(search_path):
                # Handle glob patterns
                from pathlib import Path
                parent = Path(str(search_path).split('*')[0])
                if parent.exists():
                    for path in parent.glob(str(search_path).split('/')[-1]):
                        if path.is_dir():
                            task_files = find_task_files_with_github_issues(path)
                            for task_file in task_files:
                                if self._file_references_issue(task_file, issue_number):
                                    return task_file
            elif search_path.exists():
                task_files = find_task_files_with_github_issues(search_path)
                for task_file in task_files:
                    if self._file_references_issue(task_file, issue_number):
                        return task_file
        
        return None
    
    def _file_references_issue(self, file_path: Path, issue_number: int) -> bool:
        """Check if task file references the given GitHub issue number."""
        try:
            task_data = TaskFileParser.parse_task_file(file_path)
            return task_data.get('github_issue_number') == issue_number
        except Exception:
            return False
    
    def sync_directory(self, directory: Path) -> List[Dict[str, Any]]:
        """Sync all task files with GitHub issues in a directory."""
        if not directory.exists():
            print(f"❌ Directory not found: {directory}")
            return []
        
        print(f"📁 Syncing directory: {directory}")
        
        # Find all task files with GitHub issue metadata
        task_files = find_task_files_with_github_issues(directory)
        
        if not task_files:
            print("ℹ️  No task files with GitHub issues found")
            return []
        
        results = []
        successful = 0
        failed = 0
        
        for file_path in task_files:
            result = self.sync_single_file(file_path)
            results.append(result)
            
            if result.get('success', False):
                successful += 1
            else:
                failed += 1
        
        print(f"\n📊 Summary: {successful} successful, {failed} failed")
        return results
    
    def sync_project(self, project_path: Path) -> List[Dict[str, Any]]:
        """Sync all task files in a project's tasks directory."""
        tasks_dir = project_path / "tasks"
        if not tasks_dir.exists():
            print(f"❌ Tasks directory not found: {tasks_dir}")
            return []
        
        print(f"🎯 Syncing project: {project_path.name}")
        return self.sync_directory(tasks_dir)
    
    def generate_report(self, results: List[Dict[str, Any]]) -> str:
        """Generate a summary report of sync operations."""
        if not results:
            return "No sync operations performed."
        
        successful = [r for r in results if r.get('success', False)]
        failed = [r for r in results if not r.get('success', False)]
        updated = [r for r in successful if r.get('operation') in ['updated', 'conflict_resolved']]
        conflicts = [r for r in successful if r.get('operation') == 'conflict_marked']
        
        report = [
            f"GitHub Issue Status Sync Report",
            f"Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}",
            f"",
            f"Summary:",
            f"  Total files processed: {len(results)}",
            f"  Successful operations: {len(successful)}",
            f"  Failed operations: {len(failed)}",
            f"  Files updated: {len(updated)}",
            f"  Conflicts detected: {len(conflicts)}",
            f""
        ]
        
        if updated:
            report.append("Updated Files:")
            for result in updated:
                file_name = result['file']
                issue_num = result.get('issue_number', 'N/A')
                state_change = result.get('state_change', 'N/A')
                report.append(f"  ✅ {file_name} → Issue #{issue_num}: {state_change}")
            report.append("")
        
        if conflicts:
            report.append("Conflicts (Manual Review Needed):")
            for result in conflicts:
                file_name = result['file']
                issue_num = result.get('issue_number', 'N/A')
                conflict = result.get('conflict', {})
                report.append(f"  ⚠️  {file_name} → Issue #{issue_num}: {conflict.get('description', 'Unknown conflict')}")
            report.append("")
        
        if failed:
            report.append("Failed Operations:")
            for result in failed:
                file_name = result.get('file', result.get('issue_number', 'Unknown'))
                error = result.get('error', 'Unknown error')
                report.append(f"  ❌ {file_name} → {error}")
            report.append("")
        
        return "\n".join(report)


def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(
        description="Sync local task status from GitHub Issue state changes",
        formatter_class=argparse.RawDescriptionHelpFormatter
    )
    
    parser.add_argument(
        '--file', '-f',
        type=Path,
        help='Sync a single task file'
    )
    
    parser.add_argument(
        '--directory', '-d',
        type=Path,
        help='Sync all task files in a directory'
    )
    
    parser.add_argument(
        '--project', '-p',
        type=Path,
        help='Sync all task files in a project (looks for tasks/ subdirectory)'
    )
    
    parser.add_argument(
        '--issue', '-i',
        type=int,
        help='Sync specific GitHub issue number'
    )
    
    parser.add_argument(
        '--repo', '-r',
        help='GitHub repository in owner/repo format (required with --issue)'
    )
    
    parser.add_argument(
        '--config',
        type=Path,
        help='Path to configuration file override'
    )
    
    parser.add_argument(
        '--check-only',
        action='store_true',
        help='Check for conflicts without making changes'
    )
    
    parser.add_argument(
        '--dry-run',
        action='store_true',
        help='Show what would be done without making changes'
    )
    
    parser.add_argument(
        '--verbose', '-v',
        action='store_true',
        help='Enable verbose output'
    )
    
    parser.add_argument(
        '--report',
        type=Path,
        help='Save detailed report to file'
    )
    
    args = parser.parse_args()
    
    # Validate arguments
    if args.issue and not args.repo:
        parser.error("--repo is required when using --issue")
    
    if args.issue:
        input_sources = 1
    else:
        input_sources = sum(1 for source in [args.file, args.directory, args.project] if source)
        if input_sources != 1:
            parser.error("Please specify exactly one of: --file, --directory, --project, or --issue")
    
    # Load configuration override if specified
    config_override = {}
    if args.config and args.config.exists():
        with open(args.config) as f:
            config_override = json.load(f)
    
    # Determine project path for configuration hierarchy
    project_path = None
    if args.project:
        project_path = str(args.project)
    elif args.directory and 'projects' in str(args.directory):
        parts = args.directory.parts
        if 'projects' in parts:
            project_idx = parts.index('projects')
            if project_idx + 1 < len(parts):
                project_path = str(Path(*parts[:project_idx + 2]))
    
    if args.dry_run or args.check_only:
        mode = "CHECK ONLY" if args.check_only else "DRY RUN"
        print(f"🔍 {mode} MODE - No changes will be made")
        print()
    
    # Initialize syncer
    syncer = GitHubStatusSyncer(project_path, config_override)
    syncer.initialize()
    
    # Process files based on arguments
    results = []
    
    if args.issue:
        # Parse repository
        if '/' not in args.repo:
            parser.error("Repository must be in owner/repo format")
        owner, repo = args.repo.split('/', 1)
        
        # Determine search directory
        search_dir = None
        if args.directory:
            search_dir = args.directory
        elif args.project:
            search_dir = args.project / "tasks"
        
        results = [syncer.sync_specific_issue(owner, repo, args.issue, search_dir)]
    
    elif args.file:
        if not args.file.exists():
            print(f"❌ File not found: {args.file}")
            sys.exit(1)
        results = [syncer.sync_single_file(args.file)]
    
    elif args.directory:
        results = syncer.sync_directory(args.directory)
    
    elif args.project:
        results = syncer.sync_project(args.project)
    
    # Generate and display report
    report = syncer.generate_report(results)
    print("\n" + "="*50)
    print(report)
    
    # Save report if requested
    if args.report:
        with open(args.report, 'w') as f:
            f.write(report)
        print(f"\n📄 Report saved to: {args.report}")
    
    # Display conflicts summary
    if syncer.conflicts:
        print(f"\n⚠️  {len(syncer.conflicts)} conflicts detected - manual review needed")
        for conflict in syncer.conflicts:
            print(f"   📄 {conflict['file']} (Issue #{conflict['issue_number']})")
    
    # Exit with appropriate code
    failed_count = sum(1 for r in results if not r.get('success', False))
    if failed_count > 0:
        print(f"\n⚠️  {failed_count} operations failed")
        sys.exit(1)
    else:
        print(f"\n✅ All operations completed successfully")
        sys.exit(0)


if __name__ == "__main__":
    main()