# GitHub Integration Skills - Shared Utilities

This directory contains shared utilities, templates, and workflows for the GitHub integration skills.

## Files Overview

### Core Utilities

- **`github_utils.py`** - Shared GitHub API client, authentication, and task file parsing utilities
- **`github_workflow.py`** - Workflow orchestrator that combines both skills
- **`task-template.md`** - Template for creating new task files

### Utility Classes

#### `ConfigurationManager`
Manages hierarchical configuration loading (global → project-specific → defaults).

#### `GitHubAuthenticator`  
Handles multiple authentication methods with priority order:
1. Local credentials file (secure)
2. GitHub CLI
3. Environment variable
4. Interactive setup

#### `GitHubClient`
Unified GitHub API client supporting both CLI and REST API methods with automatic fallback.

#### `TaskFileParser`
Parses and updates EDPS task markdown files while preserving format.

## Workflow Orchestrator

The `github_workflow.py` script provides convenient workflows:

```bash
# Complete bidirectional sync
python github_workflow.py full-sync --project /path/to/project

# Create/update issues only
python github_workflow.py create-issues --project /path/to/project

# Sync status only  
python github_workflow.py sync-status --project /path/to/project

# Check for conflicts
python github_workflow.py check-status --project /path/to/project

# Generate reports
python github_workflow.py report --project /path/to/project
```

## Task File Template

Use `task-template.md` as a starting point for new task files. The template includes:

- Proper metadata structure expected by the GitHub skills
- Placeholder sections for description, tasks, and acceptance criteria
- Comments showing where GitHub metadata will be added automatically

## Authentication Setup

Both skills use the same authentication system with multiple options:

### GitHub CLI (Recommended)
```bash
gh auth login
```

### Personal Access Token
Create `github-credentials.json` in the repository root:
```json
{
  "github": {
    "username": "your-username",
    "personal_access_token": "ghp_xxxxxxxxxxxxxxxxxxxx",
    "default_repository": {
      "owner": "your-username-or-org",
      "name": "your-repo"
    }
  }
}
```

### Environment Variable
```bash
export GITHUB_TOKEN="ghp_xxxxxxxxxxxxxxxxxxxx"
```

## Configuration

Both skills use hierarchical configuration:

1. **Global**: `projects/github-config.json`
2. **Project-specific**: `projects/{project-name}/github-config.json`  
3. **Skill defaults**: Built into the utilities

Project settings override global settings, which override defaults.

## Usage in Skills

Skills can import and use these utilities:

```python
import sys
from pathlib import Path

# Add shared utilities to path
sys.path.insert(0, str(Path(__file__).parent.parent / "shared"))

from github_utils import (
    ConfigurationManager, GitHubAuthenticator, GitHubClient,
    TaskFileParser, get_repository_from_config_or_credentials
)

# Initialize with project-specific configuration
config = ConfigurationManager("/path/to/project")
auth = GitHubAuthenticator(config)
credentials = auth.get_credentials()
client = GitHubClient(credentials, config)
```

## Error Handling

The utilities provide robust error handling:

- **Authentication errors** with setup guidance
- **API rate limiting** with automatic backoff
- **Network failures** with retries
- **File access issues** with detailed messages
- **Configuration validation** with helpful suggestions

## Security

- GitHub tokens are kept in Git-ignored credentials files
- Environment variables supported for CI/CD
- No sensitive data in configuration files
- Automatic token validation before use