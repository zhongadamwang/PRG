"""
Shared GitHub utilities for EDPS GitHub integration skills.
Provides authentication, configuration, and GitHub API access.
"""

import json
import os
import subprocess
import requests
import time
from pathlib import Path
from typing import Optional, Dict, List, Any, Union
from dataclasses import dataclass
import re


@dataclass
class GitHubCredentials:
    """Secure GitHub credentials container."""
    username: str
    token: str
    default_owner: Optional[str] = None
    default_repo: Optional[str] = None


@dataclass
class GitHubIssue:
    """GitHub Issue representation."""
    number: int
    title: str
    body: str
    state: str
    labels: List[str]
    assignees: List[str]
    url: str
    created_at: str
    updated_at: str
    closed_at: Optional[str] = None


class GitHubAuthenticationError(Exception):
    """Raised when GitHub authentication fails."""
    pass


class GitHubAPIError(Exception):
    """Raised when GitHub API calls fail."""
    pass


class ConfigurationManager:
    """Manages hierarchical GitHub configuration."""
    
    def __init__(self, project_path: Optional[str] = None):
        self.project_path = project_path
        self.config = self._load_configuration()
    
    def _load_configuration(self) -> Dict[str, Any]:
        """Load configuration with hierarchy: project > global > defaults."""
        config = self._get_default_config()
        
        # Load global configuration
        global_config_path = Path("projects/github-config.json")
        if global_config_path.exists():
            with open(global_config_path) as f:
                global_config = json.load(f)
                config = self._merge_configs(config, global_config)
        
        # Load project-specific configuration
        if self.project_path:
            project_config_path = Path(self.project_path) / "github-config.json"
            if project_config_path.exists():
                with open(project_config_path) as f:
                    project_config = json.load(f)
                    config = self._merge_configs(config, project_config)
        
        return config
    
    def _get_default_config(self) -> Dict[str, Any]:
        """Get default configuration."""
        return {
            "github": {
                "api": {
                    "base_url": "https://api.github.com",
                    "timeout": 30,
                    "rate_limit_delay": 1,
                    "max_retries": 3
                },
                "authentication": {
                    "token_env_var": "GITHUB_TOKEN",
                    "credentials_file": "github-credentials.json"
                },
                "default_repository": {},
                "issue_defaults": {
                    "auto_create_labels": True,
                    "default_assignee": None,
                    "milestone": None
                },
                "field_mapping": {
                    "state_mapping": {
                        "ready": "open",
                        "in-progress": "open", 
                        "completed": "closed",
                        "cancelled": "closed"
                    },
                    "priority_labels": {
                        "High": {"name": "priority:high", "color": "d73027"},
                        "Medium": {"name": "priority:medium", "color": "fee08b"},
                        "Low": {"name": "priority:low", "color": "1a9850"}
                    },
                    "effort_label_prefix": "effort:",
                    "additional_labels": [],
                    "exclude_labels": []
                },
                "batch_processing": {
                    "max_concurrent": 5,
                    "progress_reporting": True,
                    "stop_on_error": False
                },
                "sync_behavior": {
                    "auto_sync": False,
                    "preserve_manual_changes": True,
                    "conflict_resolution": "manual",
                    "sync_frequency": "on_demand"
                },
                "file_handling": {
                    "add_github_metadata": True,
                    "metadata_format": "markdown",
                    "backup_before_sync": False,
                    "create_sync_log": True,
                    "preserve_format": True
                }
            }
        }
    
    def _merge_configs(self, base: Dict, override: Dict) -> Dict:
        """Recursively merge configuration dictionaries."""
        result = base.copy()
        for key, value in override.items():
            if key in result and isinstance(result[key], dict) and isinstance(value, dict):
                result[key] = self._merge_configs(result[key], value)
            else:
                result[key] = value
        return result
    
    def get(self, key_path: str, default=None) -> Any:
        """Get configuration value using dot notation."""
        keys = key_path.split('.')
        value = self.config
        for key in keys:
            if isinstance(value, dict) and key in value:
                value = value[key]
            else:
                return default
        return value


class GitHubAuthenticator:
    """Handles GitHub authentication with multiple methods."""
    
    def __init__(self, config_manager: ConfigurationManager):
        self.config = config_manager
        self._credentials: Optional[GitHubCredentials] = None
    
    def get_credentials(self) -> GitHubCredentials:
        """Get GitHub credentials using priority order."""
        if self._credentials:
            return self._credentials
            
        # Try local credentials file
        credentials = self._load_from_credentials_file()
        if credentials:
            self._credentials = credentials
            return credentials
        
        # Try GitHub CLI
        credentials = self._load_from_github_cli()
        if credentials:
            self._credentials = credentials
            return credentials
        
        # Try environment variable
        credentials = self._load_from_environment()
        if credentials:
            self._credentials = credentials
            return credentials
        
        # Interactive setup
        credentials = self._interactive_setup()
        if credentials:
            self._credentials = credentials
            return credentials
        
        raise GitHubAuthenticationError("No valid GitHub authentication method found")
    
    def _load_from_credentials_file(self) -> Optional[GitHubCredentials]:
        """Load credentials from secure local file."""
        credentials_file = self.config.get("github.authentication.credentials_file", "github-credentials.json")
        credentials_path = Path(credentials_file)
        
        if not credentials_path.exists():
            return None
        
        try:
            with open(credentials_path) as f:
                data = json.load(f)
                github_config = data.get("github", {})
                
                return GitHubCredentials(
                    username=github_config.get("username"),
                    token=github_config.get("personal_access_token"),
                    default_owner=github_config.get("default_repository", {}).get("owner"),
                    default_repo=github_config.get("default_repository", {}).get("name")
                )
        except (json.JSONDecodeError, KeyError) as e:
            print(f"Warning: Invalid credentials file format: {e}")
            return None
    
    def _load_from_github_cli(self) -> Optional[GitHubCredentials]:
        """Load credentials from GitHub CLI."""
        try:
            # Check if GitHub CLI is authenticated
            result = subprocess.run(['gh', 'auth', 'status'], 
                                 capture_output=True, text=True, timeout=10)
            if result.returncode != 0:
                return None
            
            # Get username
            user_result = subprocess.run(['gh', 'api', 'user'], 
                                       capture_output=True, text=True, timeout=10)
            if user_result.returncode == 0:
                user_data = json.loads(user_result.stdout)
                username = user_data.get('login')
                
                return GitHubCredentials(
                    username=username,
                    token="gh_cli",  # Placeholder - CLI handles auth
                    default_owner=self.config.get("github.default_repository.owner"),
                    default_repo=self.config.get("github.default_repository.name")
                )
        except (subprocess.TimeoutExpired, FileNotFoundError, json.JSONDecodeError):
            pass
        
        return None
    
    def _load_from_environment(self) -> Optional[GitHubCredentials]:
        """Load credentials from environment variable."""
        token_env_var = self.config.get("github.authentication.token_env_var", "GITHUB_TOKEN")
        token = os.environ.get(token_env_var)
        
        if not token:
            return None
        
        # Try to get username from token
        try:
            headers = {"Authorization": f"Bearer {token}"}
            response = requests.get("https://api.github.com/user", headers=headers, timeout=10)
            if response.status_code == 200:
                user_data = response.json()
                username = user_data.get('login')
                
                return GitHubCredentials(
                    username=username,
                    token=token,
                    default_owner=self.config.get("github.default_repository.owner"),
                    default_repo=self.config.get("github.default_repository.name")
                )
        except requests.RequestException:
            pass
        
        return None
    
    def _interactive_setup(self) -> Optional[GitHubCredentials]:
        """Interactive credentials setup."""
        print("❌ GitHub credentials not found!")
        print("\n🔧 Setup Required:")
        print("1. Create Personal Access Token: https://github.com/settings/tokens")
        print("   - Token name: AI_Slowcooker_Integration")
        print("   - Scopes: ☑️ repo (or public_repo)")
        
        username = input("2. Enter your GitHub username: ").strip()
        if not username:
            return None
        
        token = input("3. Enter your Personal Access Token: ").strip()
        if not token:
            return None
        
        # Validate credentials
        try:
            headers = {"Authorization": f"Bearer {token}"}
            response = requests.get("https://api.github.com/user", headers=headers, timeout=10)
            if response.status_code != 200:
                print("❌ Invalid token or username")
                return None
        except requests.RequestException:
            print("❌ Could not validate credentials")
            return None
        
        # Save credentials
        credentials_data = {
            "github": {
                "username": username,
                "personal_access_token": token,
                "default_repository": {
                    "owner": username,
                    "name": input(f"4. Default repository name (optional): ").strip() or None
                }
            }
        }
        
        credentials_file = self.config.get("github.authentication.credentials_file", "github-credentials.json")
        with open(credentials_file, 'w') as f:
            json.dump(credentials_data, f, indent=2)
        
        print(f"✅ Credentials saved to: {credentials_file}")
        
        return GitHubCredentials(
            username=username,
            token=token,
            default_owner=username,
            default_repo=credentials_data["github"]["default_repository"]["name"]
        )


class GitHubClient:
    """GitHub API client with CLI and REST API support."""
    
    def __init__(self, credentials: GitHubCredentials, config_manager: ConfigurationManager):
        self.credentials = credentials
        self.config = config_manager
        self.session = requests.Session()
        self.session.headers.update({
            "Authorization": f"Bearer {credentials.token}",
            "Accept": "application/vnd.github.v3+json",
            "User-Agent": "AI-Slowcooker-EDPS/1.0"
        })
        self.base_url = config_manager.get("github.api.base_url", "https://api.github.com")
        self.use_cli = credentials.token == "gh_cli"
    
    def create_issue(self, owner: str, repo: str, title: str, body: str, 
                    labels: Optional[List[str]] = None, assignees: Optional[List[str]] = None) -> GitHubIssue:
        """Create a new GitHub issue."""
        if self.use_cli:
            return self._create_issue_cli(owner, repo, title, body, labels, assignees)
        else:
            return self._create_issue_api(owner, repo, title, body, labels, assignees)
    
    def update_issue(self, owner: str, repo: str, issue_number: int, 
                    title: Optional[str] = None, body: Optional[str] = None,
                    state: Optional[str] = None, labels: Optional[List[str]] = None,
                    assignees: Optional[List[str]] = None) -> GitHubIssue:
        """Update an existing GitHub issue."""
        if self.use_cli:
            return self._update_issue_cli(owner, repo, issue_number, title, body, state, labels, assignees)
        else:
            return self._update_issue_api(owner, repo, issue_number, title, body, state, labels, assignees)
    
    def get_issue(self, owner: str, repo: str, issue_number: int) -> GitHubIssue:
        """Get GitHub issue details."""
        if self.use_cli:
            return self._get_issue_cli(owner, repo, issue_number)
        else:
            return self._get_issue_api(owner, repo, issue_number)
    
    def list_issues(self, owner: str, repo: str, issue_numbers: List[int]) -> List[GitHubIssue]:
        """Get multiple issues efficiently."""
        if self.use_cli:
            return self._list_issues_cli(owner, repo, issue_numbers)
        else:
            return self._list_issues_api(owner, repo, issue_numbers)
    
    def _create_issue_cli(self, owner: str, repo: str, title: str, body: str,
                         labels: Optional[List[str]], assignees: Optional[List[str]]) -> GitHubIssue:
        """Create issue using GitHub CLI."""
        cmd = ['gh', 'issue', 'create', '--repo', f"{owner}/{repo}", '--title', title, '--body', body]
        
        if labels:
            cmd.extend(['--label', ','.join(labels)])
        if assignees:
            for assignee in assignees:
                cmd.extend(['--assignee', assignee])
        
        try:
            result = subprocess.run(cmd, capture_output=True, text=True, timeout=30)
            if result.returncode != 0:
                raise GitHubAPIError(f"GitHub CLI error: {result.stderr}")
            
            # Extract issue URL and number
            issue_url = result.stdout.strip()
            issue_number = int(issue_url.split('/')[-1])
            
            return self.get_issue(owner, repo, issue_number)
        except subprocess.TimeoutExpired:
            raise GitHubAPIError("GitHub CLI timeout")
    
    def _create_issue_api(self, owner: str, repo: str, title: str, body: str,
                         labels: Optional[List[str]], assignees: Optional[List[str]]) -> GitHubIssue:
        """Create issue using REST API."""
        url = f"{self.base_url}/repos/{owner}/{repo}/issues"
        payload = {"title": title, "body": body}
        
        if labels:
            payload["labels"] = labels
        if assignees:
            payload["assignees"] = assignees
        
        response = self._make_request("POST", url, json=payload)
        return self._parse_issue_response(response)
    
    def _update_issue_cli(self, owner: str, repo: str, issue_number: int,
                         title: Optional[str], body: Optional[str], state: Optional[str], 
                         labels: Optional[List[str]], assignees: Optional[List[str]]) -> GitHubIssue:
        """Update issue using GitHub CLI."""
        if state == "closed":
            cmd = ['gh', 'issue', 'close', str(issue_number), '--repo', f"{owner}/{repo}"]
        elif state == "open":
            cmd = ['gh', 'issue', 'reopen', str(issue_number), '--repo', f"{owner}/{repo}"]
        else:
            cmd = ['gh', 'issue', 'edit', str(issue_number), '--repo', f"{owner}/{repo}"]
        
        if title:
            cmd.extend(['--title', title])
        if body:
            cmd.extend(['--body', body])
        if labels:
            cmd.extend(['--add-label', ','.join(labels)])
        if assignees:
            for assignee in assignees:
                cmd.extend(['--add-assignee', assignee])
        
        try:
            result = subprocess.run(cmd, capture_output=True, text=True, timeout=30)
            if result.returncode != 0:
                raise GitHubAPIError(f"GitHub CLI error: {result.stderr}")
            
            return self.get_issue(owner, repo, issue_number)
        except subprocess.TimeoutExpired:
            raise GitHubAPIError("GitHub CLI timeout")
    
    def _update_issue_api(self, owner: str, repo: str, issue_number: int,
                         title: Optional[str], body: Optional[str], state: Optional[str],
                         labels: Optional[List[str]], assignees: Optional[List[str]]) -> GitHubIssue:
        """Update issue using REST API."""
        url = f"{self.base_url}/repos/{owner}/{repo}/issues/{issue_number}"
        payload = {}
        
        if title:
            payload["title"] = title
        if body:
            payload["body"] = body
        if state:
            payload["state"] = state
        if labels:
            payload["labels"] = labels
        if assignees:
            payload["assignees"] = assignees
        
        response = self._make_request("PATCH", url, json=payload)
        return self._parse_issue_response(response)
    
    def _get_issue_cli(self, owner: str, repo: str, issue_number: int) -> GitHubIssue:
        """Get issue using GitHub CLI."""
        cmd = ['gh', 'issue', 'view', str(issue_number), '--repo', f"{owner}/{repo}", '--json',
               'number,title,body,state,labels,assignees,url,createdAt,updatedAt,closedAt']
        
        try:
            result = subprocess.run(cmd, capture_output=True, text=True, timeout=30)
            if result.returncode != 0:
                raise GitHubAPIError(f"GitHub CLI error: {result.stderr}")
            
            data = json.loads(result.stdout)
            return self._parse_issue_data(data)
        except subprocess.TimeoutExpired:
            raise GitHubAPIError("GitHub CLI timeout")
        except json.JSONDecodeError as e:
            raise GitHubAPIError(f"Invalid JSON response from GitHub CLI: {e}")
    
    def _get_issue_api(self, owner: str, repo: str, issue_number: int) -> GitHubIssue:
        """Get issue using REST API."""
        url = f"{self.base_url}/repos/{owner}/{repo}/issues/{issue_number}"
        response = self._make_request("GET", url)
        return self._parse_issue_response(response)
    
    def _list_issues_cli(self, owner: str, repo: str, issue_numbers: List[int]) -> List[GitHubIssue]:
        """Get multiple issues using GitHub CLI."""
        issues = []
        for issue_number in issue_numbers:
            try:
                issue = self.get_issue(owner, repo, issue_number)
                issues.append(issue)
            except GitHubAPIError:
                continue  # Skip issues that can't be accessed
        return issues
    
    def _list_issues_api(self, owner: str, repo: str, issue_numbers: List[int]) -> List[GitHubIssue]:
        """Get multiple issues using REST API."""
        issues = []
        for issue_number in issue_numbers:
            try:
                issue = self.get_issue(owner, repo, issue_number)
                issues.append(issue)
            except GitHubAPIError:
                continue  # Skip issues that can't be accessed
        return issues
    
    def _make_request(self, method: str, url: str, **kwargs) -> Dict:
        """Make HTTP request with error handling and retries."""
        max_retries = self.config.get("github.api.max_retries", 3)
        timeout = self.config.get("github.api.timeout", 30)
        
        for attempt in range(max_retries):
            try:
                response = self.session.request(method, url, timeout=timeout, **kwargs)
                
                if response.status_code == 401:
                    raise GitHubAuthenticationError("Invalid GitHub token")
                elif response.status_code == 404:
                    raise GitHubAPIError("Repository or issue not found")
                elif response.status_code == 403:
                    # Rate limit handling
                    if 'X-RateLimit-Reset' in response.headers:
                        reset_time = int(response.headers['X-RateLimit-Reset'])
                        wait_time = reset_time - time.time() + 1
                        if wait_time > 0:
                            print(f"Rate limit reached. Waiting {wait_time:.0f} seconds...")
                            time.sleep(wait_time)
                            continue
                    raise GitHubAPIError("GitHub API access forbidden")
                elif response.status_code >= 400:
                    raise GitHubAPIError(f"GitHub API error {response.status_code}: {response.text}")
                
                return response.json()
                
            except requests.RequestException as e:
                if attempt == max_retries - 1:
                    raise GitHubAPIError(f"Request failed after {max_retries} attempts: {e}")
                time.sleep(2 ** attempt)  # Exponential backoff
    
    def _parse_issue_response(self, data: Dict) -> GitHubIssue:
        """Parse GitHub API response to GitHubIssue."""
        return self._parse_issue_data(data)
    
    def _parse_issue_data(self, data: Dict) -> GitHubIssue:
        """Parse issue data to GitHubIssue object."""
        return GitHubIssue(
            number=data['number'],
            title=data['title'],
            body=data.get('body', ''),
            state=data['state'],
            labels=[label.get('name', label) if isinstance(label, dict) else str(label) 
                   for label in data.get('labels', [])],
            assignees=[assignee.get('login', assignee) if isinstance(assignee, dict) else str(assignee)
                      for assignee in data.get('assignees', [])],
            url=data['url'] if 'url' in data else data.get('html_url', ''),
            created_at=data.get('createdAt', data.get('created_at', '')),
            updated_at=data.get('updatedAt', data.get('updated_at', '')),
            closed_at=data.get('closedAt', data.get('closed_at'))
        )


class TaskFileParser:
    """Parses EDPS task files and extracts GitHub-relevant information."""
    
    @staticmethod
    def parse_task_file(file_path: Path) -> Dict[str, Any]:
        """Parse task markdown file and extract structured data."""
        if not file_path.exists():
            raise FileNotFoundError(f"Task file not found: {file_path}")
        
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        task_data = {
            'file_path': file_path,
            'title': '',
            'state': '',
            'labels': [],
            'assignees': [],
            'priority': '',
            'estimated_effort': '',
            'description': '',
            'github_issue_number': None,
            'github_issue_url': '',
            'last_synced': '',
            'content': content
        }
        
        # Extract title from first heading or "Issue:" line
        title_match = re.search(r'^# (.+)$', content, re.MULTILINE)
        if title_match:
            task_data['title'] = title_match.group(1)
        else:
            issue_match = re.search(r'^\*\*Issue:\*\* (.+)$', content, re.MULTILINE)
            if issue_match:
                task_data['title'] = issue_match.group(1)
        
        # Extract metadata fields
        metadata_patterns = {
            'state': r'\*\*State:\*\*\s*(.+)$',
            'labels': r'\*\*Labels:\*\*\s*(.+)$',
            'assignees': r'\*\*Assignees:\*\*\s*(.+)$',
            'priority': r'\*\*Priority:\*\*\s*(.+)$',
            'estimated_effort': r'\*\*Estimated Effort:\*\*\s*(.+)$',
            'github_issue_number': r'\*\*GitHub Issue:\*\*\s*#?(\d+)',
            'github_issue_url': r'\*\*Issue URL:\*\*\s*(.+)$',
            'last_synced': r'\*\*Last Synced:\*\*\s*(.+)$'
        }
        
        for field, pattern in metadata_patterns.items():
            match = re.search(pattern, content, re.MULTILINE)
            if match:
                value = match.group(1).strip()
                if field in ['labels', 'assignees']:
                    task_data[field] = [item.strip() for item in value.split(',') if item.strip()]
                elif field == 'github_issue_number':
                    task_data[field] = int(value)
                else:
                    task_data[field] = value
        
        # Extract description content
        description_parts = []
        
        # Look for Description section
        desc_match = re.search(r'## Description\s*\n(.*?)(?=\n##|\Z)', content, re.DOTALL)
        if desc_match:
            description_parts.append(desc_match.group(1).strip())
        
        # Look for Tasks section  
        tasks_match = re.search(r'## Tasks\s*\n(.*?)(?=\n##|\Z)', content, re.DOTALL)
        if tasks_match:
            description_parts.append("## Tasks\n" + tasks_match.group(1).strip())
        
        # Look for Acceptance Criteria section
        criteria_match = re.search(r'## Acceptance Criteria\s*\n(.*?)(?=\n##|\Z)', content, re.DOTALL)
        if criteria_match:
            description_parts.append("## Acceptance Criteria\n" + criteria_match.group(1).strip())
        
        task_data['description'] = '\n\n'.join(description_parts)
        
        return task_data
    
    @staticmethod
    def update_task_file(file_path: Path, updates: Dict[str, Any], 
                        backup: bool = False) -> bool:
        """Update task file with new metadata while preserving format."""
        if backup:
            backup_path = file_path.with_suffix(file_path.suffix + '.backup')
            import shutil
            shutil.copy2(file_path, backup_path)
        
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Update metadata fields
            for field, new_value in updates.items():
                if field == 'github_issue_number' and new_value:
                    # Add or update GitHub Issue number
                    pattern = r'\*\*GitHub Issue:\*\*\s*#?\d+'
                    replacement = f"**GitHub Issue:** #{new_value}"
                    if re.search(pattern, content):
                        content = re.sub(pattern, replacement, content)
                    else:
                        # Insert after State field
                        state_pattern = r'(\*\*State:\*\*\s*.+$)'
                        content = re.sub(state_pattern, r'\1\n' + replacement, content, flags=re.MULTILINE)
                
                elif field == 'github_issue_url' and new_value:
                    # Add or update Issue URL
                    pattern = r'\*\*Issue URL:\*\*\s*.+'
                    replacement = f"**Issue URL:** {new_value}"
                    if re.search(pattern, content):
                        content = re.sub(pattern, replacement, content)
                    elif 'github_issue_number' in updates:
                        # Insert after GitHub Issue field
                        issue_pattern = r'(\*\*GitHub Issue:\*\*\s*#?\d+)'
                        content = re.sub(issue_pattern, r'\1\n' + replacement, content)
                
                elif field == 'state' and new_value:
                    # Update state
                    pattern = r'\*\*State:\*\*\s*.+'
                    replacement = f"**State:** {new_value}"
                    content = re.sub(pattern, replacement, content)
                
                elif field == 'last_synced' and new_value:
                    # Add or update last synced timestamp
                    pattern = r'\*\*Last Synced:\*\*\s*.+'
                    replacement = f"**Last Synced:** {new_value}"
                    if re.search(pattern, content):
                        content = re.sub(pattern, replacement, content)
                    else:
                        # Insert at end of metadata section
                        lines = content.split('\n')
                        insert_index = 0
                        for i, line in enumerate(lines):
                            if line.startswith('**') and ':' in line:
                                insert_index = i + 1
                            elif insert_index > 0 and not line.startswith('**'):
                                break
                        lines.insert(insert_index, replacement)
                        content = '\n'.join(lines)
            
            # Write updated content
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            
            return True
            
        except Exception as e:
            print(f"Error updating task file {file_path}: {e}")
            if backup:
                # Restore from backup
                backup_path = file_path.with_suffix(file_path.suffix + '.backup')
                if backup_path.exists():
                    import shutil
                    shutil.copy2(backup_path, file_path)
            return False


def find_task_files_with_github_issues(directory: Path) -> List[Path]:
    """Find all task files that have GitHub issue metadata."""
    task_files = []
    
    for file_path in directory.glob("*.md"):
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
                if re.search(r'\*\*GitHub Issue:\*\*\s*#?\d+', content):
                    task_files.append(file_path)
        except Exception:
            continue
    
    return task_files


def get_repository_from_config_or_credentials(config_manager: ConfigurationManager, 
                                             credentials: GitHubCredentials) -> tuple[str, str]:
    """Get repository owner and name from configuration or credentials."""
    # Try configuration first
    owner = config_manager.get("github.default_repository.owner")
    repo = config_manager.get("github.default_repository.name") 
    
    if owner and repo:
        return owner, repo
    
    # Fall back to credentials
    if credentials.default_owner and credentials.default_repo:
        return credentials.default_owner, credentials.default_repo
    
    raise ValueError("No default repository configured. Please set github.default_repository in configuration or credentials.")