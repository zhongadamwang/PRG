---
name: github-issue-create-update
description: Create and update GitHub Issues from local task markdown files with field mapping, metadata extraction, and two-way synchronization support for project management integration.
license: MIT
---

# GitHub Issue Create/Update Skill

## Intent
Synchronize local development task files with GitHub Issues for seamless project management. Creates new GitHub Issues from task markdown files and updates existing issues when task files are modified, enabling one-way local-to-GitHub project tracking workflow.

## Inputs
- **Source**: Local task file path(s) or project directory
- **Format**: Markdown task files following EDPS task structure (title, state, labels, description, acceptance criteria)
- **Configuration**: Hierarchical configuration system (project-specific overrides global)
- **Authentication**: GitHub Personal Access Token (via configuration or environment variable)
- **Repository**: GitHub repository identifier from configuration files

## Outputs
**Operation Results:**
- Issue URLs for created/updated issues
- Operation status (created, updated, skipped, failed)
- Field mapping summary
- Error details for failed operations

**File Updates:**
- Task files updated with GitHub issue numbers for tracking
- Issue ID metadata embedded in task file frontmatter

## Core Functionality

### Task File Parsing
Extracts structured data from markdown task files:

```markdown
# Issue: T1 - GitHub Integration Skill
**State:** ready
**Labels:** feature, github-integration, mvp
**Assignees:** adam.wang
**Priority:** High
**Estimated Effort:** 1.5 days
```

**Parsed Fields:**
- **Title**: Extracted from first heading or "Issue:" line
- **Description**: Content from Description section + Tasks + Acceptance Criteria
- **State**: Maps to GitHub issue state (open/closed)
- **Labels**: Direct mapping to GitHub labels
- **Assignees**: Maps to GitHub usernames
- **Priority**: Converted to priority label

## Dual Integration Approach

The skill supports both GitHub CLI and REST API for maximum reliability and flexibility:

**Method 1: GitHub CLI (Preferred)**
- Simplified authentication via `gh auth login`
- Better error handling and user experience
- Automatic token management
- Native GitHub integration

```bash
# Create issue
gh issue create --repo owner/repo --title "Task Title" --body "Description" --label "feature,priority:high" --assignee "username"

# Update issue
gh issue edit 123 --title "Updated Title" --add-label "completed"

# Close issue
gh issue close 123 --reason completed
```

**Method 2: REST API (Fallback)**
- Direct HTTP API integration
- Custom error handling and retry logic
- Token-based authentication
- Fine-grained control over requests

```http
POST /repos/{owner}/{repo}/issues
PATCH /repos/{owner}/{repo}/issues/{issue_number}
```

### Field Mapping

| Task File Field | GitHub Issue Field | Mapping Logic |
|-----------------|-------------------|---------------|
| Title/Header | title | First heading or "Issue:" line |
| Description + Tasks + Criteria | body | Concatenated markdown content |
| State | state | ready/in-progress → open, completed/closed → closed |
| Labels | labels | Direct mapping, auto-create missing |
| Assignees | assignees | GitHub usernames |
| Priority | labels | High → priority:high, Medium → priority:medium |
| Estimated Effort | labels | effort:1.5-days format |

### Issue Tracking & Updates

**Create vs Update Logic:**
1. Check task file for existing `GitHub Issue:` field
2. If no issue number → Create new issue
3. If issue number exists → Update existing issue
4. Store issue number in task file metadata for future updates

**Metadata Storage:**
```markdown
**GitHub Issue:** #123
**Issue URL:** https://github.com/owner/repo/issues/123
```

## Usage Examples

### Configuration Setup
```bash
# 1. Set up global configuration
cat > projects/github-config.json << EOF
{
  "github": {
    "default_repository": {
      "owner": "your-org",
      "name": "main-repo"
    },
    "authentication": {
      "token_env_var": "GITHUB_TOKEN"
    }
  }
}
EOF

# 2. Set up project-specific config (optional)
cat > projects/01-my-project/github-config.json << EOF
{
  "github": {
    "issue_defaults": {
      "milestone": "Sprint 1",
      "default_assignee": "project.lead"
    }
  }
}
EOF

# 3. Set environment variable
export GITHUB_TOKEN="ghp_xxxxxxxxxxxxxxxxxxxx"
```

### Single Task File
```markdown
// Execute skill on single task file - uses global config
Input: /path/to/standalone-task.md
Config: projects/github-config.json
Output: Created issue #123 at https://github.com/your-org/main-repo/issues/123
```

### Project Directory Batch
```markdown
// Execute skill on project tasks directory - uses project-specific config
Input: /projects/01-building-skills/tasks/
Config: projects/01-building-skills/github-config.json (+ global fallback)
Output: 
- T1-github-integration.md → Created #123 [milestone: "MVP - Building Skills"]
- T2-requirements-ingest.md → Updated #124 [assignee: "adam.wang"]
- T3-goals-extract.md → Created #125 [labels: "skills-development", "priority:critical"]
```

### Update Workflow
```markdown
// Modify existing task file and re-run skill
Modified: T1-github-integration.md (state: ready → in-progress)
Output: Updated issue #123 - changed state to open, added in-progress label
```

### Implementation Methods

**GitHub CLI Implementation (Primary)**
```python
class GitHubCLIClient:
    def create_issue(self, repo, title, body, labels=None, assignees=None):
        cmd = ['gh', 'issue', 'create', '--repo', repo, '--title', title, '--body', body]
        if labels:
            cmd.extend(['--label', ','.join(labels)])
        if assignees:
            for assignee in assignees:
                cmd.extend(['--assignee', assignee])
        result = subprocess.run(cmd, capture_output=True, text=True)
        return self._parse_issue_url(result.stdout)
    
    def update_issue(self, repo, issue_number, **updates):
        cmd = ['gh', 'issue', 'edit', str(issue_number), '--repo', repo]
        for field, value in updates.items():
            if field == 'state' and value == 'closed':
                cmd = ['gh', 'issue', 'close', str(issue_number), '--repo', repo]
            elif field == 'title':
                cmd.extend(['--title', value])
            elif field == 'labels':
                cmd.extend(['--add-label', ','.join(value)])
        return subprocess.run(cmd, capture_output=True, text=True)
```

**REST API Implementation (Fallback)**
```python
class GitHubAPIClient:
    def create_issue(self, owner, repo, title, body, labels=None, assignees=None):
        url = f"https://api.github.com/repos/{owner}/{repo}/issues"
        payload = {"title": title, "body": body}
        if labels: payload["labels"] = labels
        if assignees: payload["assignees"] = assignees
        response = requests.post(url, headers=self.headers, json=payload)
        return response.json()
```

### Authentication Priority Order

1. **Local Credentials File** (Preferred - Secure)
   ```bash
   # Skill automatically reads github-credentials.json
   # File is Git-ignored for security
   ```

2. **GitHub CLI** (If available and authenticated)
   ```bash
   gh auth login
   gh auth status
   ```

3. **Environment Variable**
   ```bash
   export GITHUB_TOKEN="ghp_xxxxxxxxxxxxxxxxxxxx"
   ```

4. **Interactive Setup** (First-time users)
   - Prompts for GitHub username and Personal Access Token
   - Creates secure local credentials file
   - Provides step-by-step setup guidance

### Core Components

1. **ConfigurationManager**: Load and merge hierarchical configurations
2. **AuthenticationHandler**: Manage GitHub CLI and token-based authentication
3. **GitHubClientFactory**: Create CLI or REST API clients based on availability
4. **TaskFileParser**: Extract structure and metadata from markdown
5. **FieldMapper**: Convert task fields to GitHub issue fields with config customization
6. **IssueManager**: Handle create vs update logic with both CLI and API
7. **MetadataUpdater**: Update task files with issue numbers and sync status
8. **ErrorHandler**: Robust error handling with automatic fallback between methods

### Error Handling
- **Configuration Errors**: Validation and helpful error messages for invalid configs
- **API Failures**: Retry with exponential backoff based on configuration settings
- **Authentication**: Clear error messages for token or permission issues
- **Rate Limits**: Automatic waiting and progress updates per configuration
- **Missing Fields**: Graceful defaults from configuration and validation warnings
- **File Permissions**: Skip files that cannot be updated with detailed logging

## Quality Criteria
- **Accuracy**: All task file fields correctly mapped to GitHub issue fields
- **Reliability**: Robust handling of API failures and network issues  
- **Idempotency**: Re-running skill produces same results without duplicates
- **Traceability**: Clear mapping between local tasks and GitHub issues
- **Performance**: Batch operations with progress reporting for large projects

## Configuration Integration

### Secure Credentials Management
The skill uses a secure local credentials system that keeps sensitive tokens out of Git:

**Credentials File**: `github-credentials.json` (Git-ignored)
```json
{
  "github": {
    "username": "your-github-username",
    "personal_access_token": "ghp_xxxxxxxxxxxxxxxxxxxx",
    "default_repository": {
      "owner": "your-username-or-org",
      "name": "your-repo-name"
    },
    "preferences": {
      "default_assignee": "your-github-username",
      "remember_credentials": true
    }
  }
}
```

**First-Time Setup**:
1. Copy `github-credentials.json.template` to `github-credentials.json`
2. Replace placeholder values with your real GitHub credentials
3. Get Personal Access Token from: https://github.com/settings/tokens
4. Required token scopes: `repo` (full control) for private repos, or `public_repo` for public only

**Interactive Setup**: If credentials file is missing, skill will prompt:
```
❌ GitHub credentials not found!

🔧 Setup Required:
1. Create Personal Access Token: https://github.com/settings/tokens
   - Token name: AI_Slowcooker_Integration
   - Scopes: ☑️ repo (or public_repo)
2. Enter your GitHub username: _______
3. Enter your Personal Access Token: _______
4. Credentials saved to: github-credentials.json
```

### Hierarchical Configuration
Project settings override global settings, credentials remain secure:

```json
// projects/github-config.json (committed to Git)
{
  "authentication": {
    "credentials_file": "../../../github-credentials.json",
    "token_env_var": "GITHUB_TOKEN"
  }
}
```

## Configuration Best Practices

### Global Configuration Strategy
- **Repository Settings**: Set default repository for organization-wide tasks
- **Authentication**: Use environment variables, never hardcode tokens
- **API Settings**: Configure rate limits and timeouts for your GitHub instance
- **Label Standards**: Define consistent labeling scheme across all projects

### Project Configuration Strategy
- **Keep Minimal**: Only override what's different from global settings
- **Milestone Mapping**: Set project-specific milestones and assignees
- **Custom Labels**: Add project-specific labels while preserving global ones
- **Priority Overrides**: Customize priority labels for project criticality

### Security Considerations
```json
// ❌ DON'T: Hardcode tokens in configuration
{
  "authentication": {
    "token": "ghp_hardcoded_token_bad"
  }
}

// ✅ DO: Reference environment variables
{
  "authentication": {
    "token_env_var": "GITHUB_TOKEN"
  }
}
```

### Configuration Validation
The skill automatically validates configurations and provides helpful error messages:
- Missing required fields with suggestions
- Invalid repository names with format examples
- Token permission validation with required scopes
- Label color format validation with color examples

## Configuration Reference

### Complete Configuration Schema
```json
{
  "github": {
    "api": {
      "base_url": "string (GitHub API URL)",
      "timeout": "number (seconds)",
      "rate_limit_delay": "number (seconds between requests)",
      "max_retries": "number (max retry attempts)"
    },
    "authentication": {
      "token_env_var": "string (environment variable name)",
      "token": "string (direct token - not recommended)"
    },
    "default_repository": {
      "owner": "string (GitHub username or organization)",
      "name": "string (repository name)"
    },
    "issue_defaults": {
      "auto_create_labels": "boolean (create missing labels)",
      "default_assignee": "string (default assignee username)",
      "milestone": "string (milestone name)"
    },
    "field_mapping": {
      "state_mapping": {
        "task_state": "github_state"
      },
      "priority_labels": {
        "Priority_Level": {
          "name": "string (GitHub label name)",
          "color": "string (hex color without #)"
        }
      },
      "effort_label_prefix": "string (prefix for effort labels)",
      "additional_labels": "array (labels to add to all issues)",
      "exclude_labels": "array (task labels to not sync)"
    },
    "batch_processing": {
      "max_concurrent": "number (concurrent API requests)",
      "progress_reporting": "boolean (show progress bars)",
      "stop_on_error": "boolean (stop batch on first error)"
    },
    "file_updates": {
      "add_github_metadata": "boolean (add GitHub info to task files)",
      "metadata_format": "string (yaml_frontmatter|markdown)",
      "backup_on_update": "boolean (create backup before updating)"
    }
  }
}
```

## Integration Notes

**VS Code Integration:**
- Execute via Command Palette: "EDPS: Sync Tasks to GitHub Issues"
- File explorer context menu on task files
- Automatic execution hooks for file saves (optional)

**Copilot Integration:**
- Natural language commands: "Create GitHub issues for all tasks in this project"
- Progress reporting in chat interface
- Interactive conflict resolution for field mapping issues

**Project Structure Compatibility:**
- Works with existing EDPS project structure
- Preserves task file format and metadata
- Non-destructive updates to task files