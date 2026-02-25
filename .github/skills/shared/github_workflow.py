#!/usr/bin/env python3
"""
GitHub Integration Workflow Script
==================================

Convenience script that combines both GitHub integration skills for common workflows.

Usage:
    python github_workflow.py sync-all --project <project_path>
    python github_workflow.py create-issues --project <project_path>
    python github_workflow.py sync-status --project <project_path>
    python github_workflow.py full-sync --project <project_path>
"""

import argparse
import subprocess
import sys
from pathlib import Path
from datetime import datetime


class GitHubWorkflow:
    """Orchestrates GitHub integration workflows."""
    
    def __init__(self, project_path: str, verbose: bool = False):
        self.project_path = Path(project_path)
        self.verbose = verbose
        self.skills_path = Path(__file__).parent.parent
        
        # Paths to skill scripts
        self.create_update_script = self.skills_path / "github-issue-create-update" / "create_update_issues.py"
        self.sync_status_script = self.skills_path / "github-issue-sync-status" / "sync_status.py"
        
        # Validate paths
        if not self.project_path.exists():
            print(f"❌ Project path does not exist: {self.project_path}")
            sys.exit(1)
        
        if not self.create_update_script.exists():
            print(f"❌ Create/update script not found: {self.create_update_script}")
            sys.exit(1)
            
        if not self.sync_status_script.exists():
            print(f"❌ Sync status script not found: {self.sync_status_script}")
            sys.exit(1)
    
    def run_command(self, cmd: list, description: str) -> dict:
        """Run a command and return results."""
        print(f"\n🔄 {description}")
        
        if self.verbose:
            print(f"   Command: {' '.join(cmd)}")
        
        try:
            result = subprocess.run(cmd, capture_output=True, text=True)
            
            if result.returncode == 0:
                print(f"✅ {description} completed successfully")
                if self.verbose and result.stdout:
                    print("Output:")
                    print(result.stdout)
                return {'success': True, 'output': result.stdout, 'error': None}
            else:
                print(f"❌ {description} failed")
                if result.stderr:
                    print("Error:")
                    print(result.stderr)
                return {'success': False, 'output': result.stdout, 'error': result.stderr}
        
        except Exception as e:
            print(f"❌ {description} failed with exception: {e}")
            return {'success': False, 'output': None, 'error': str(e)}
    
    def create_issues(self) -> bool:
        """Create/update GitHub issues for all tasks."""
        cmd = [
            "python", str(self.create_update_script),
            "--project", str(self.project_path)
        ]
        
        if self.verbose:
            cmd.append("--verbose")
        
        result = self.run_command(cmd, "Creating/updating GitHub issues")
        return result['success']
    
    def sync_status(self) -> bool:
        """Sync GitHub issue status to local tasks."""
        cmd = [
            "python", str(self.sync_status_script),
            "--project", str(self.project_path)
        ]
        
        if self.verbose:
            cmd.append("--verbose")
        
        result = self.run_command(cmd, "Syncing GitHub issue status")
        return result['success']
    
    def check_status(self) -> bool:
        """Check for status conflicts without making changes."""
        cmd = [
            "python", str(self.sync_status_script),
            "--project", str(self.project_path),
            "--check-only"
        ]
        
        if self.verbose:
            cmd.append("--verbose")
        
        result = self.run_command(cmd, "Checking for status conflicts")
        return result['success']
    
    def full_sync(self) -> bool:
        """Perform full bidirectional sync."""
        print(f"🎯 Starting full sync for project: {self.project_path.name}")
        
        # Step 1: Create/update issues for local tasks
        if not self.create_issues():
            print("❌ Failed to create/update issues, aborting full sync")
            return False
        
        # Step 2: Check for conflicts before syncing status
        print("\n🔍 Checking for status conflicts...")
        if not self.check_status():
            print("⚠️  Status conflicts detected, manual review may be needed")
        
        # Step 3: Sync status from GitHub
        if not self.sync_status():
            print("❌ Failed to sync status")
            return False
        
        print("\n✅ Full sync completed successfully")
        return True
    
    def sync_all(self) -> bool:
        """Legacy alias for full_sync."""
        return self.full_sync()
    
    def generate_report(self) -> None:
        """Generate a comprehensive sync report."""
        report_path = self.project_path / f"github-sync-report-{datetime.now().strftime('%Y%m%d-%H%M%S')}.txt"
        
        print(f"\n📄 Generating comprehensive report...")
        
        # Run create/update with report
        create_report_path = self.project_path / "create-update-report.txt"
        cmd = [
            "python", str(self.create_update_script),
            "--project", str(self.project_path),
            "--dry-run", "--report", str(create_report_path)
        ]
        self.run_command(cmd, "Generating create/update report")
        
        # Run sync status with report
        sync_report_path = self.project_path / "sync-status-report.txt"
        cmd = [
            "python", str(self.sync_status_script),
            "--project", str(self.project_path),
            "--check-only", "--report", str(sync_report_path)
        ]
        self.run_command(cmd, "Generating sync status report")
        
        # Combine reports
        try:
            with open(report_path, 'w') as combined:
                combined.write(f"GitHub Integration Report\n")
                combined.write(f"Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}\n")
                combined.write(f"Project: {self.project_path.name}\n")
                combined.write("="*50 + "\n\n")
                
                if create_report_path.exists():
                    combined.write("CREATE/UPDATE REPORT\n")
                    combined.write("-"*20 + "\n")
                    with open(create_report_path) as f:
                        combined.write(f.read())
                    combined.write("\n\n")
                
                if sync_report_path.exists():
                    combined.write("SYNC STATUS REPORT\n")
                    combined.write("-"*18 + "\n")
                    with open(sync_report_path) as f:
                        combined.write(f.read())
            
            print(f"📄 Combined report saved to: {report_path}")
            
            # Cleanup individual reports
            if create_report_path.exists():
                create_report_path.unlink()
            if sync_report_path.exists():
                sync_report_path.unlink()
                
        except Exception as e:
            print(f"❌ Failed to generate combined report: {e}")


def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(
        description="GitHub Integration Workflow Orchestrator",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Workflows:
  sync-all      Complete bidirectional sync (create issues + sync status)
  full-sync     Alias for sync-all
  create-issues Create/update GitHub issues from local tasks
  sync-status   Sync GitHub issue status to local tasks
  check-status  Check for status conflicts (read-only)
  report        Generate comprehensive reports
        """
    )
    
    parser.add_argument(
        'workflow',
        choices=['sync-all', 'full-sync', 'create-issues', 'sync-status', 'check-status', 'report'],
        help='Workflow to execute'
    )
    
    parser.add_argument(
        '--project', '-p',
        type=Path,
        required=True,
        help='Path to project directory'
    )
    
    parser.add_argument(
        '--verbose', '-v',
        action='store_true',
        help='Enable verbose output'
    )
    
    args = parser.parse_args()
    
    # Initialize workflow
    workflow = GitHubWorkflow(args.project, args.verbose)
    
    # Execute requested workflow
    success = False
    
    if args.workflow in ['sync-all', 'full-sync']:
        success = workflow.full_sync()
    elif args.workflow == 'create-issues':
        success = workflow.create_issues()
    elif args.workflow == 'sync-status':
        success = workflow.sync_status()
    elif args.workflow == 'check-status':
        success = workflow.check_status()
    elif args.workflow == 'report':
        workflow.generate_report()
        success = True
    
    if success:
        print(f"\n✅ Workflow '{args.workflow}' completed successfully")
        sys.exit(0)
    else:
        print(f"\n❌ Workflow '{args.workflow}' failed")
        sys.exit(1)


if __name__ == "__main__":
    main()