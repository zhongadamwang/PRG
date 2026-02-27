using System.Security.Claims;

namespace Sanjel.RequestManagement.Core.Services
{
	public interface ICurrentUserService
	{
		string GetCurrentUsername();
		void SetCurrentUsername(string username);
	}

	public class CurrentUserService : ICurrentUserService
	{
		private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
		private string _currentUsername = "SYSTEM";

		public CurrentUserService(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public void SetCurrentUsername(string username)
		{
			_currentUsername = username ?? "SYSTEM";
		}

		public string GetCurrentUsername()
		{
			// First try to get from HTTP context (for web requests)
			if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
			{
				// Try to get the username from different claim types based on the auth provider
				var user = _httpContextAccessor.HttpContext.User;

				// For Azure AD, try preferred_username first, then name, then email, then identity name
				var username = user.FindFirst("preferred_username")?.Value ??
											user.FindFirst(ClaimTypes.Name)?.Value ??
											user.FindFirst(ClaimTypes.Email)?.Value ??
											user.FindFirst("name")?.Value ??
											user.Identity.Name;

				if (!string.IsNullOrEmpty(username))
				{
					// Check if username is in email format and extract just the username part
					var atIndex = username.IndexOf('@');
					if (atIndex > 0)
					{
						username = username.Substring(0, atIndex);
					}

					return username;
				}
			}

			// Fallback to manually set username (for background tasks, tests, etc.)
			var fallbackUsername = _currentUsername;
			if (!string.IsNullOrEmpty(fallbackUsername))
			{
				// Also check fallback username for email format
				var atIndex = fallbackUsername.IndexOf('@');
				if (atIndex > 0)
				{
					fallbackUsername = fallbackUsername.Substring(0, atIndex);
				}
				return fallbackUsername;
			}

			return "SYSTEM";
		}
	}
}
