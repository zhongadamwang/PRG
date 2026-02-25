#!/usr/bin/env python3
"""
GitHub Issue Create/Update Script
==================================

Creates and updates GitHub Issues from local task markdown files with 
field mapping, metadata extraction, and two-way synchronization support 
for project management integration.

Part of the EDPS (Evolutionary Development Process System) skills.

Usage:
    python create_update_issues.py --file <task_file>
    python create_update_issues.py --project <project_directory>
    python create_update_issues.py --directory <tasks_directory>
"""

import argparse
import json
import os
import sys
import re
import requests
from datetime import datetime
from pathlib import Path
from typing import List, Dict, Any, Optional


class GitHubIssueCreator:
    """Main class for creating and updating GitHub issues from task files."""
    
    def __init__(self, project_path: Optional[str] = None, config_override: Optional[Dict] = None):
        self.config = self._load_config(project_path, config_override)
        self.token = self._get_token()
        self.repo_owner = self.config['github']['default_repository']['owner']
        self.repo_name = self.config['github']['default_repository']['name']
        self.headers = {
            'Authorization': f'token {self.token}',
            'Accept': 'application/vnd.github.v3+json',
            'Content-Type': 'application/json'
        }
        self.base_url = f"https://api.github.com/repos/{self.repo_owner}/{self.repo_name}"
        self.results = []
    
    def _load_config(self, project_path: Optional[str], config_override: Optional[Dict]) -> Dict:
        """Load configuration from JSON file."""
        # Try project-specific config first
        config_paths = []
        if project_path:
            config_paths.append(Path(project_path) / "github-config.json")
        config_paths.extend([
            Path("projects/github-config.json"),
            Path("../../../projects/github-config.json"),
            Path("github-config.json")
        ])
        
        config = None
        for config_path in config_paths:
            if config_path.exists():
                with open(config_path, 'r') as f:
                    config = json.load(f)
                    break
        
        if not config:
            raise Exception("No github-config.json found. Please create configuration file.")
        
        if config_override:
            # Simple merge of overrides
            config.update(config_override)
        
        return config
    
    def _get_token(self) -> str:
        """Get GitHub token from environment or configuration."""
        token_env_var = self.config.get('github', {}).get('authentication', {}).get('token_env_var', 'GITHUB_TOKEN')
        token = os.getenv(token_env_var)
        if not token:
            raise Exception(f"GitHub token not found in environment variable {token_env_var}")
        return token
    
    def parse_task_file(self, file_path: str) -> Dict:
        """Parse a task markdown file and extract metadata."""
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Extract title from first heading
        title_match = re.search(r'^#\s+(.+)', content, re.MULTILINE)
        title = title_match.group(1).strip() if title_match else os.path.basename(file_path)
        
        # Check if there's already a GitHub issue reference
        github_issue_match = re.search(r'\*\*GitHub Issue:\*\*\s*#?(\d+)', content)
        existing_issue = int(github_issue_match.group(1)) if github_issue_match else None
        
        # Extract state from content (if present)
        state_match = re.search(r'\*\*State:\*\*\s*(\w+)', content)
        state = state_match.group(1).lower() if state_match else "open"
        
        # Map task state to GitHub state
        state_mapping = self.config['github']['field_mapping']['state_mapping']
        github_state = state_mapping.get(state, "open")
        
        # Generate labels
        labels = self.config['github']['field_mapping']['additional_labels'].copy()
        
        # Create issue body from content (remove title)
        body_content = re.sub(r'^#\s+.+\n?', '', content, flags=re.MULTILINE)
        body_content = body_content.strip()
        
        return {
            'title': title,
            'body': body_content,
            'state': github_state,
            'labels': labels,
            'existing_issue': existing_issue,
            'file_path': file_path,
            'original_content': content
        }

    def create_issue(self, task_data: Dict) -> Optional[int]:
        """Create a new GitHub issue."""
        url = f"{self.base_url}/issues"
        payload = {
            'title': task_data['title'],
            'body': task_data['body'],
            'labels': task_data['labels']
        }
        
        # Add default assignee
        if self.config['github']['issue_defaults'].get('default_assignee'):
            payload['assignees'] = [self.config['github']['issue_defaults']['default_assignee']]
        
        try:
            response = requests.post(url, headers=self.headers, json=payload)
            response.raise_for_status()
            issue_data = response.json()
            print(f"✅ Created issue #{issue_data['number']}: {task_data['title']}")
            return issue_data['number']
        except requests.exceptions.RequestException as e:
            print(f"❌ Failed to create issue for {task_data['title']}: {e}")
            if hasattr(e, 'response') and hasattr(e.response, 'text'):
                print(f"   Response: {e.response.text}")
            return None

    def update_issue(self, issue_number: int, task_data: Dict) -> bool:
        """Update an existing GitHub issue."""
        url = f"{self.base_url}/issues/{issue_number}"
        payload = {
            'title': task_data['title'],
            'body': task_data['body'],
            'state': task_data['state'],
            'labels': task_data['labels']
        }
        
        try:
            response = requests.patch(url, headers=self.headers, json=payload)
            response.raise_for_status()
            print(f"🔄 Updated issue #{issue_number}: {task_data['title']}")
            return True
        except requests.exceptions.RequestException as e:
            print(f"❌ Failed to update issue #{issue_number}: {e}")
            if hasattr(e, 'response') and hasattr(e.response, 'text'):
                print(f"   Response: {e.response.text}")
            return False

    def update_task_file(self, task_data: Dict, issue_number: int):
        """Update task file with GitHub issue metadata."""
        content = task_data['original_content']
        
        # Check if GitHub metadata already exists
        if '**GitHub Issue:**' in content:
            # Update existing metadata
            content = re.sub(
                r'\*\*GitHub Issue:\*\*\s*#?\d+',
                f'**GitHub Issue:** #{issue_number}',
                content
            )
            content = re.sub(
                r'\*\*Issue URL:\*\*.*',
                f'**Issue URL:** https://github.com/{self.repo_owner}/{self.repo_name}/issues/{issue_number}',
                content
            )
        else:
            # Add metadata after the title
            title_match = re.search(r'^(#\s+.+\n)', content, re.MULTILINE)
            if title_match:
                insert_pos = title_match.end()
                metadata = f"\n**GitHub Issue:** #{issue_number}\n**Issue URL:** https://github.com/{self.repo_owner}/{self.repo_name}/issues/{issue_number}\n"
                content = content[:insert_pos] + metadata + content[insert_pos:]
        
        # Write updated content back to file
        with open(task_data['file_path'], 'w', encoding='utf-8') as f:
            f.write(content)
        
        print(f"📝 Updated task file: {Path(task_data['file_path']).name}")
    
    def process_single_file(self, file_path: Path) -> Dict[str, Any]:
        """Process a single task file."""
        print(f"📋 Processing {file_path.name}...")
        
        try:
            task_data = self.parse_task_file(str(file_path))
            
            if task_data['existing_issue']:
                # Update existing issue
                success = self.update_issue(task_data['existing_issue'], task_data)
                if success:
                    self.update_task_file(task_data, task_data['existing_issue'])
                return {
                    'file': file_path.name,
                    'operation': 'updated',
                    'issue_number': task_data['existing_issue'],
                    'success': success
                }
            else:
                # Create new issue
                issue_number = self.create_issue(task_data)
                if issue_number:
                    self.update_task_file(task_data, issue_number)
                    return {
                        'file': file_path.name,
                        'operation': 'created',
                        'issue_number': issue_number,
                        'success': True
                    }
                else:
                    return {
                        'file': file_path.name,
                        'operation': 'failed',
                        'success': False
                    }
                
        except Exception as e:
            error_msg = f"❌ Error processing {file_path.name}: {e}"
            print(error_msg)
            return {
                'file': file_path.name,
                'operation': 'error',
                'error': str(e),
                'success': False
            }

    def process_tasks_directory(self, tasks_directory: str):
        """Sync all tasks in the directory to GitHub issues."""
        task_files = list(Path(tasks_directory).glob("T*.md"))
        if not task_files:
            print(f"❌ No task files found in {tasks_directory}")
            return
        
        print(f"🚀 Syncing {len(task_files)} tasks to GitHub...")
        print(f"📁 Repository: {self.repo_owner}/{self.repo_name}")
        print()
        
        for task_file in sorted(task_files):
            result = self.process_single_file(task_file)
            self.results.append(result)
            print()
        
        # Print summary
        successful = sum(1 for r in self.results if r['success'])
        total = len(self.results)
        print(f"✅ {successful}/{total} tasks synced successfully")
        
        return self.results


def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(
        description="Create and update GitHub Issues from EDPS task files",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
    # Process a single task file
    python create_update_issues.py --file T001-setup.md
    
    # Process all tasks in a directory
    python create_update_issues.py --directory ./tasks
    
    # Process all tasks in a project (looks for tasks/ subdirectory)
    python create_update_issues.py --project ./projects/my-project
        """
    )
    
    parser.add_argument(
        '--file', '-f',
        type=Path,
        help='Process a single task file'
    )
    
    parser.add_argument(
        '--directory', '-d',
        type=Path,
        help='Process all task files in a directory'
    )
    
    parser.add_argument(
        '--project', '-p',
        type=Path,
        help='Process all task files in a project\'s tasks directory'
    )
    
    parser.add_argument(
        '--config',
        type=Path,
        help='Path to configuration file'
    )
    
    args = parser.parse_args()
    
    # Validate arguments
    if not any([args.file, args.directory, args.project]):
        parser.error("Must specify --file, --directory, or --project")
    
    try:
        # Initialize the issue creator
        creator = GitHubIssueCreator(
            project_path=str(args.project) if args.project else None
        )
        
        # Process based on arguments
        if args.file:
            if not args.file.exists():
                print(f"❌ File not found: {args.file}")
                sys.exit(1)
            
            result = creator.process_single_file(args.file)
            if result['success']:
                print("✅ Task successfully synced to GitHub!")
            else:
                print("❌ Sync failed!")
                sys.exit(1)
                
        elif args.directory:
            if not args.directory.exists():
                print(f"❌ Directory not found: {args.directory}")
                sys.exit(1)
            
            creator.process_tasks_directory(str(args.directory))
            
        elif args.project:
            if not args.project.exists():
                print(f"❌ Project not found: {args.project}")
                sys.exit(1)
            
            tasks_dir = args.project / "tasks"
            if not tasks_dir.exists():
                print(f"❌ Tasks directory not found: {tasks_dir}")
                sys.exit(1)
            
            creator.process_tasks_directory(str(tasks_dir))
    
    except Exception as e:
        print(f"❌ Error: {e}")
        sys.exit(1)


if __name__ == "__main__":
    main()