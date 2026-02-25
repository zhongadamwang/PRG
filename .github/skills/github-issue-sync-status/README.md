# GitHub Issue Sync Status Skill

This skill updates local task status from GitHub Issue state changes while preserving local file format and metadata. Enables one-way synchronization from GitHub project management back to local development tasks.

## Quick Start

### 1. Setup Authentication

Use the same authentication setup as the create/update skill:

#### Option A: GitHub CLI (Recommended)
```bash
gh auth login
```

#### Option B: Personal Access Token
Use the same `github-credentials.json` file as the create/update skill.

### 2. Configuration

Copy the example configuration:
```bash
cp github-config.json.example github-config.json
```

Edit to configure sync behavior and conflict resolution.

### 3. Usage

#### Sync Single Task File
```bash
python sync_status.py --file /path/to/task.md
```

#### Sync All Tasks in Directory
```bash
python sync_status.py --directory /path/to/tasks
```

#### Sync Entire Project
```bash
python sync_status.py --project /path/to/project
```

#### Sync Specific Issue
```bash
python sync_status.py --issue 123 --repo owner/repo
```

## How It Works

The script:

1. **Finds task files** with GitHub issue metadata (`**GitHub Issue:** #123`)
2. **Fetches current state** from GitHub for each issue
3. **Compares states** between local tasks and GitHub issues
4. **Updates local files** while preserving format and content
5. **Handles conflicts** based on configuration settings

## State Mapping

| GitHub State | Local State | Logic |
|-------------|-------------|-------|
| open (unassigned) | ready | Issue is available for work |
| open (assigned) | in-progress | Issue is being worked on |
| closed | completed | Issue is finished |
| reopened | ready | Issue needs work again |

## Conflict Resolution

The script detects conflicts between local and GitHub states:

### Conflict Types

- **Local Newer**: Local changes are more recent than GitHub
- **GitHub Newer**: GitHub changes are more recent than local sync
- **Incompatible States**: Local completed/cancelled but GitHub reopened
- **Manual Review Needed**: Can't determine precedence

### Resolution Strategies

Configure via `github.sync_behavior.conflict_resolution`:

#### `"manual"` (Default)
- Mark conflicts in task files with comment markers
- Report conflicts for manual resolution
- No automatic state changes

#### `"github_wins"`
- Always use GitHub state
- Override local changes
- Good for GitHub-primary workflows

#### `"local_wins"`
- Preserve local state
- Update sync timestamp only
- Good for local-primary workflows

#### `"smart"`
- Use GitHub state if it's newer
- Use timestamps to determine precedence
- Fallback to manual review

## Task File Requirements

Files must have GitHub issue metadata to be synced:

```markdown
# Task Title

**State:** ready
**GitHub Issue:** #123
**Issue URL:** https://github.com/owner/repo/issues/123
**Last Synced:** 2026-02-24T14:30:00

## Description
Task description...
```

## Conflict Markers

When conflicts are detected in manual mode, the script adds markers:

```markdown
<!-- SYNC CONFLICT DETECTED -->
<!-- Local State: completed -->
<!-- GitHub State: open -->
<!-- Conflict: Local task completed but GitHub issue reopened -->
<!-- Please resolve manually and remove this comment -->

**State:** completed
```

## Command Line Options

```
usage: sync_status.py [-h] [--file FILE] [--directory DIRECTORY] 
                     [--project PROJECT] [--issue ISSUE] [--repo REPO]
                     [--config CONFIG] [--check-only] [--dry-run] 
                     [--verbose] [--report REPORT]

Sync local task status from GitHub Issue state changes

options:
  -h, --help            show this help message and exit
  --file FILE, -f FILE  Sync a single task file
  --directory DIRECTORY, -d DIRECTORY
                        Sync all task files in a directory
  --project PROJECT, -p PROJECT
                        Sync all task files in a project
  --issue ISSUE, -i ISSUE
                        Sync specific GitHub issue number
  --repo REPO, -r REPO  GitHub repository in owner/repo format
  --config CONFIG       Path to configuration file override
  --check-only          Check for conflicts without making changes
  --dry-run             Show what would be done without making changes
  --verbose, -v         Enable verbose output
  --report REPORT       Save detailed report to file
```

## Configuration Options

Key configuration settings in `github-config.json`:

```json
{
  "github": {
    "sync_behavior": {
      "conflict_resolution": "manual",
      "preserve_in_progress": true,
      "add_completion_date": true,
      "state_mapping": {
        "issue_to_task": {
          "open": "ready",
          "closed": "completed"
        }
      }
    },
    "file_handling": {
      "backup_before_sync": false,
      "preserve_format": true
    }
  }
}
```

## Integration Examples

### Automated Sync on Issue State Change

Add to `.github/workflows/sync-status.yml`:

```yaml
name: Sync Issue Status
on:
  issues:
    types: [closed, reopened, assigned, unassigned]

jobs:
  sync:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup Python
        uses: actions/setup-python@v4
        with:
          python-version: '3.x'
      - name: Install dependencies
        run: pip install requests
      - name: Sync issue status
        run: |
          python .github/skills/github-issue-sync-status/sync_status.py \
            --issue ${{ github.event.issue.number }} \
            --repo ${{ github.repository }}
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
      - name: Commit changes
        if: success()
        run: |
          git config --local user.email "action@github.com"
          git config --local user.name "GitHub Action"
          git add -A
          git diff --staged --quiet || git commit -m "Sync issue #${{ github.event.issue.number }} status"
          git push
```

### VS Code Task

Add to `.vscode/tasks.json`:

```json
{
    "label": "Sync GitHub Issue Status",
    "type": "shell",
    "command": "python",
    "args": [
        "${workspaceFolder}/.github/skills/github-issue-sync-status/sync_status.py",
        "--project",
        "${workspaceFolder}/OrgDocument/projects/${input:projectName}"
    ],
    "group": "build"
}
```

### Manual Sync Workflow

```bash
# Check for conflicts first
python sync_status.py --project ./projects/current --check-only

# Perform sync with conflict resolution
python sync_status.py --project ./projects/current --report sync-report.txt

# Review conflicts and resolve manually
# Then sync again
python sync_status.py --project ./projects/current
```

## Best Practices

1. **Regular Syncing**: Run sync regularly to catch state changes early
2. **Conflict Review**: Always review conflicts before automatic resolution
3. **Backup Strategy**: Enable backups for important projects
4. **Testing**: Use `--check-only` or `--dry-run` before making changes
5. **Monitoring**: Save reports to track sync history and issues