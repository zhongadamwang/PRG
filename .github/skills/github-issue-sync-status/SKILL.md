---
name: github-issue-sync-status
description: Update local task status from GitHub Issue state changes while preserving local file format and metadata. Enables one-way synchronization from GitHub project management back to local development tasks.
license: MIT
---

# GitHub Issue Sync Status Skill

## Intent
Synchronize local development task status from GitHub Issues for seamless project management. Updates local task files when GitHub issue states change, preserving file format while maintaining consistency between GitHub project tracking and local development workflow.

## Inputs
- **Source**: Local task file path(s) or project directory containing files with GitHub issue metadata
- **Configuration**: Hierarchical configuration system (project-specific overrides global)
- **Authentication**: Secure local credentials file, GitHub CLI, or environment variables
- **Repository**: GitHub repository identifier from configuration or credentials file
- **Mode**: Real-time sync, manual trigger, or batch operation

## Outputs
**Sync Results:**
- Updated task files with synchronized status
- Sync operation summary (updated, skipped, failed)
- Issue state change details
- Conflict resolution reports for manual review

**File Updates:**
- Task state synchronized with GitHub issue state
- Last sync timestamp metadata
- Conflict markers for manual resolution when needed

## Core Functionality

### GitHub Issue State Mapping
Maps GitHub issue states to local task states:

| GitHub State | Local Task State | Sync Action |
|-------------|------------------|-------------|
| open | ready, in-progress | Update to ready if unassigned, in-progress if assigned |
| closed (completed) | completed | Update to completed, add completion date |
| closed (not planned) | cancelled | Update to cancelled with reason |
| reopened | ready | Revert to ready state |

### Issue Detection & Mapping
Identifies local task files with GitHub issue tracking:
- Scans for `**GitHub Issue:** #123` metadata in task files
- Extracts issue URLs from `**Issue URL:**` fields
- Maps issue numbers to local file paths
- Validates issue accessibility via GitHub API

### Local File Updates
Preserves local task file format while updating status:
- Updates `**State:**` field based on GitHub issue state
- Adds/updates `**Last Synced:**` timestamp
- Preserves all existing metadata and content structure
- Adds sync status indicators for tracking

### Dual Integration Approach

**Method 1: GitHub CLI (Preferred)**
```bash
gh issue list --repo owner/repo --state all --json number,state,title
gh issue view 123 --repo owner/repo --json state,closedAt,stateReason
```

**Method 2: REST API (Fallback)**
```http
GET /repos/{owner}/{repo}/issues/{issue_number}
Accept: application/vnd.github.v3+json
Authorization: Bearer TOKEN
```

## Secure Authentication System

### Credentials Management
Uses the same secure local credentials system as `github-issue-create-update`:

**Local Credentials File**: `github-credentials.json` (Git-ignored)
```json
{
  "github": {
    "username": "your-github-username",
    "personal_access_token": "ghp_xxxxxxxxxxxxxxxxxxxx",
    "default_repository": {
      "owner": "your-username-or-org",
      "name": "your-repo-name"
    }
  }
}
```

**Authentication Priority**:
1. **Local credentials file** (secure, Git-ignored)
2. **GitHub CLI** (if authenticated: `gh auth status`)
3. **Environment variable** (`GITHUB_TOKEN`)
4. **Interactive setup** (prompts for credentials on first use)

**First-Time Setup**:
```
❌ GitHub credentials not found for sync operation!

🔧 Setup Required:
1. Do you have a Personal Access Token? [Y/n]
2. If no: Visit https://github.com/settings/tokens
3. Enter GitHub username: _______
4. Enter Personal Access Token: _______
5. ✅ Credentials saved securely to github-credentials.json
```

## Configuration Integration

Uses the same hierarchical configuration as `github-issue-create-update` with secure credentials:

```json
{
  "github": {
    "authentication": {
      "credentials_file": "../../../github-credentials.json",
      "token_env_var": "GITHUB_TOKEN"
    },
    "sync_behavior": {
      "auto_sync": false,
      "preserve_manual_changes": true,
      "conflict_resolution": "manual",
      "sync_frequency": "on_demand"
    },
    "state_mapping": {
      "issue_to_task": {
        "open": "ready",
        "closed": "completed"
      },
      "preserve_in_progress": true,
      "add_completion_date": true
    },
    "file_handling": {
      "backup_before_sync": false,
      "create_sync_log": true,
      "preserve_format": true
    }
  }
}
```

## Usage Examples

### Single File Sync
```markdown
// Sync specific task file with its GitHub issue
Input: ./tasks/T1-github-integration.md (contains GitHub Issue: #123)
Process: Check GitHub issue #123 state → Update local task state
Output: Task file updated, sync status reported
```

### Project Directory Sync
```markdown
// Sync all task files in project with tracked GitHub issues
Input: ./projects/01-building-skills/tasks/
Process: 
- Scan for files with GitHub issue metadata
- Batch check issue states via GitHub API
- Update local files with state changes
Output: 
- 5 files scanned, 3 with GitHub issues
- T1: No change (already completed)
- T2: Updated from ready → in-progress (issue assigned)
- T3: Updated from in-progress → completed (issue closed)
```

### Conflict Detection
```markdown
// Handle conflicts between local and GitHub states
Scenario: Local task marked "completed" but GitHub issue reopened
Action: 
- Detect conflict between local:completed vs github:open
- Add conflict marker to file
- Report conflict for manual resolution
- Maintain current state until resolved
```

## Implementation Architecture

### Entry Point
```python
def execute_github_sync_status_skill(
    task_paths: List[str],
    project_path: Optional[str] = None,
    config_override: Optional[Dict] = None,
    sync_mode: str = "check_and_update",  # check_only, update_only, check_and_update
    dry_run: bool = False
) -> SkillResult
```

### Core Components

1. **IssueStateReader**: Fetch current states from GitHub (CLI + REST API)
2. **TaskFileScanner**: Identify local files with GitHub issue metadata
3. **StateSynchronizer**: Compare and update local states based on GitHub
4. **ConflictDetector**: Identify and handle discrepancies between local/GitHub
5. **FileUpdater**: Preserve format while updating task file metadata
6. **SyncReporter**: Generate detailed sync operation reports

### Dual GitHub Integration

**GitHub CLI Integration (Primary)**
```python
class GitHubCLIClient:
    def get_issue_state(self, repo, issue_number):
        result = subprocess.run(['gh', 'issue', 'view', str(issue_number), 
                               '--repo', repo, '--json', 'state,closedAt'], 
                               capture_output=True, text=True)
        return json.loads(result.stdout)
        
    def list_issues(self, repo, numbers):
        issues_str = ','.join(map(str, numbers))
        result = subprocess.run(['gh', 'issue', 'list', '--repo', repo,
                               '--json', 'number,state,title,closedAt'], 
                               capture_output=True, text=True)
        return json.loads(result.stdout)
```

**REST API Integration (Fallback)**
```python
class GitHubAPIClient:
    def get_issue_state(self, repo_owner, repo_name, issue_number):
        url = f"https://api.github.com/repos/{repo_owner}/{repo_name}/issues/{issue_number}"
        response = requests.get(url, headers=self.headers)
        return response.json()
```

### Error Handling & Resilience
- **GitHub API Rate Limits**: Automatic backoff and CLI fallback
- **Authentication Failures**: Clear error messages with setup guidance
- **Missing Issues**: Skip with warning, don't fail entire sync
- **File Lock Issues**: Retry mechanism with exponential backoff
- **Network Failures**: Graceful degradation with offline mode
- **Format Preservation**: Backup and validate file changes

## Advanced Features

### Conflict Resolution
```python
class ConflictResolver:
    STRATEGIES = {
        "github_wins": "Always use GitHub state",
        "local_wins": "Preserve local state", 
        "manual": "Mark conflicts for review",
        "smart": "Use timestamps and context"
    }
```

### Sync Logging
```markdown
## Sync Log - 2026-02-24 14:30:00

### Issues Processed
- #123: T1-github-integration.md → No change (both completed)
- #124: T2-requirements.md → Updated ready → in-progress
- #125: T3-goals.md → CONFLICT detected (manual review needed)

### Summary
- Files scanned: 8
- Issues found: 3  
- Updated: 1
- Conflicts: 1
- Errors: 0
```

## Quality Criteria
- **Accuracy**: Local task states accurately reflect GitHub issue states
- **Preservation**: Local file format and metadata completely preserved
- **Resilience**: Robust handling of API failures, rate limits, and network issues
- **Conflict Handling**: Clear detection and resolution of state conflicts
- **Performance**: Efficient batch processing with minimal API calls
- **Traceability**: Complete audit trail of sync operations and changes

## Integration Examples

### VS Code Integration
```json
// tasks.json
{
    "label": "Sync GitHub Issue Status",
    "type": "shell",
    "command": "python",
    "args": [".github/skills/github-issue-sync-status/sync.py", "--project", "current"]
}
```

### Automated Workflow
```yaml
# .github/workflows/sync-status.yml
name: Sync Issue Status
on:
  issues:
    types: [closed, reopened, assigned]
  
jobs:
  sync:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - run: |
          python .github/skills/github-issue-sync-status/sync.py \
            --issue ${{ github.event.issue.number }} \
            --auto-commit
```

### CLI Usage
```bash
# Manual sync specific file
python sync.py --file ./tasks/T1-github-integration.md

# Sync entire project
python sync.py --project ./projects/01-building-skills

# Check only (no updates)
python sync.py --project current --check-only

# Dry run to see what would change
python sync.py --project current --dry-run
```

## Security & Privacy
- **Token Security**: Uses same secure token handling as create-update skill
- **Read-Only Operations**: Only reads GitHub data, never modifies GitHub state
- **Local File Safety**: Creates backups before modifications (configurable)
- **Audit Trail**: Maintains detailed logs of all sync operations
- **Permission Validation**: Verifies read access to issues before processing

**Authentication Priority**:
1. GitHub CLI (if authenticated)
2. Environment variable token
3. Configuration file token reference
4. Interactive authentication prompt