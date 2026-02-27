using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Prg.ProjectName.Core.Services;

namespace Prg.ProjectName.Core.Tests;

public class CurrentUserServiceTests
{
	private class FakeHttpContextAccessor : IHttpContextAccessor
	{
		public HttpContext? HttpContext { get; set; }
	}

	[Test]
	public void GetCurrentUsername_FromPreferredUsernameClaim_TrimsEmail()
	{
		var accessor = new FakeHttpContextAccessor();
		var identity = new ClaimsIdentity(new[] { new Claim("preferred_username", "bob@example.com") }, "test");
		accessor.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

		var svc = new CurrentUserService(accessor);

		var name = svc.GetCurrentUsername();
		Assert.That(name, Is.EqualTo("bob"));
	}

	[Test]
	public void GetCurrentUsername_FromNameClaim_ReturnsName()
	{
		var accessor = new FakeHttpContextAccessor();
		var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "Alice") }, "test");
		accessor.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

		var svc = new CurrentUserService(accessor);

		var name = svc.GetCurrentUsername();
		Assert.That(name, Is.EqualTo("Alice"));
	}

	[Test]
	public void GetCurrentUsername_FallbackToSetUsername_TrimsEmail()
	{
		var accessor = new FakeHttpContextAccessor();
		accessor.HttpContext = new DefaultHttpContext(); // unauthenticated

		var svc = new CurrentUserService(accessor);
		svc.SetCurrentUsername("mark@example.org");

		var name = svc.GetCurrentUsername();
		Assert.That(name, Is.EqualTo("mark"));
	}

	[Test]
	public void GetCurrentUsername_DefaultsToSystem_WhenNothingSet()
	{
		var accessor = new FakeHttpContextAccessor();
		accessor.HttpContext = null;

		var svc = new CurrentUserService(accessor);

		var name = svc.GetCurrentUsername();
		Assert.That(name, Is.EqualTo("SYSTEM"));
	}
}
