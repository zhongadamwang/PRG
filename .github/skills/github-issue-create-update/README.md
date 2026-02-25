# GitHub Issue Create/Update Skill

This skill creates and updates GitHub Issues from local task markdown files with field mapping, metadata extraction, and two-way synchronization support for project management integration.

## Quick Start

### 1. Setup Authentication

Choose one of these authentication methods:

#### Option A: GitHub CLI (Recommended)
```bash
gh auth login
```

#### Option B: Personal Access Token
1. Create a Personal Access Token at https://github.com/settings/tokens
2. Copy `github-credentials.json.template` to `github-credentials.json`
3. Update with your credentials:
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

#### Option C: Environment Variable
```bash
export GITHUB_TOKEN="ghp_xxxxxxxxxxxxxxxxxxxx"
```

### 2. Configuration

Copy the example configuration:
```bash
cp github-config.json.example github-config.json
```

Edit `github-config.json` to set your default repository and preferences.

### 3. Usage

#### Process Single Task File
```bash
python create_update_issues.py --file /path/to/task.md
```

#### Process All Tasks in a Directory
```bash
python create_update_issues.py --directory /path/to/tasks
```

#### Process Entire Project
```bash
python create_update_issues.py --project /path/to/project
```

## Script Features

- **Dual Integration**: Uses GitHub CLI when available, falls back to REST API
- **Field Mapping**: Maps task metadata to GitHub issue fields automatically
- **State Management**: Handles create vs update operations intelligently
- **Error Handling**: Robust error handling with detailed feedback
- **Batch Processing**: Efficiently processes multiple files
- **Configuration**: Hierarchical configuration system (global + project-specific)

## Task File Format

The script expects EDPS-style task files with metadata fields:

```markdown
# Issue: T1 - GitHub Integration Skill

**State:** ready
**Labels:** feature, github-integration, mvp
**Assignees:** adam.wang
**Priority:** High
**Estimated Effort:** 1.5 days

## Description
Description of the task...

## Tasks
- [ ] Task item 1
- [ ] Task item 2

## Acceptance Criteria
- Acceptance criterion 1
- Acceptance criterion 2
```

After processing, the script adds GitHub metadata:
```markdown
**GitHub Issue:** #123
**Issue URL:** https://github.com/owner/repo/issues/123
**Last Synced:** 2026-02-24T14:30:00
```

## Field Mapping

| Task Field | GitHub Field | Notes |
|------------|-------------|-------|
| Title/Header | title | First heading or "Issue:" line |
| Description + Tasks + Criteria | body | Combined markdown content |
| State | state | Maps to open/closed |
| Labels | labels | Direct mapping, auto-creates missing |
| Assignees | assignees | GitHub usernames |
| Priority | labels | High → priority:high |
| Estimated Effort | labels | effort:1.5-days format |

## Configuration Options

See `github-config.json.example` for full configuration options:

- **Authentication**: Multiple authentication methods
- **Repository**: Default repository settings
- **Field Mapping**: Customize how task fields map to GitHub
- **Label Management**: Priority labels, effort labels, additional labels
- **Batch Processing**: Concurrency and error handling
- **File Updates**: Metadata format and backup options

## Command Line Options

```
usage: create_update_issues.py [-h] [--file FILE] [--directory DIRECTORY] 
                              [--project PROJECT] [--config CONFIG] 
                              [--dry-run] [--verbose] [--report REPORT]

Create and update GitHub Issues from EDPS task files

options:
  -h, --help            show this help message and exit
  --file FILE, -f FILE  Process a single task file
  --directory DIRECTORY, -d DIRECTORY
                        Process all task files in a directory
  --project PROJECT, -p PROJECT
                        Process all task files in a project (looks for tasks/ subdirectory)
  --config CONFIG       Path to configuration file override
  --dry-run             Show what would be done without making changes
  --verbose, -v         Enable verbose output
  --report REPORT       Save detailed report to file
```

## Error Handling

The script provides detailed error messages for common issues:

- **Authentication Errors**: Clear guidance on token setup
- **Permission Errors**: Missing repository access
- **API Errors**: Rate limits, network issues
- **File Errors**: Missing files, permission issues
- **Configuration Errors**: Invalid settings with suggestions

## Integration with VS Code

Add to your project's `.vscode/tasks.json`:

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "Create GitHub Issues",
            "type": "shell",
            "command": "python",
            "args": [
                "${workspaceFolder}/.github/skills/github-issue-create-update/create_update_issues.py",
                "--project",
                "${workspaceFolder}/OrgDocument/projects/${input:projectName}"
            ],
            "group": "build",
            "presentation": {
                "echo": true,
                "reveal": "always",
                "focus": false,
                "panel": "shared"
            }
        }
    ],
    "inputs": [
        {
            "id": "projectName",
            "description": "Project name",
            "default": "01 - Building Skills",
            "type": "promptString"
        }
    ]
}
```

Then run via Command Palette: `Tasks: Run Task` > `Create GitHub Issues`