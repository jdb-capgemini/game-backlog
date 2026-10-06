using System.Security.Claims;
using GameBacklog.Api.Dtos.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBacklog.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login(
        [FromQuery] string? returnUrl = "/")
    {
        var frontendBaseUrl =
            _configuration["Frontend:BaseUrl"]
            ?? throw new InvalidOperationException(
                "Frontend BaseUrl is missing.");

        var safeReturnPath =
            GetSafeReturnPath(returnUrl);

        var redirectUri =
            $"{frontendBaseUrl.TrimEnd('/')}{safeReturnPath}";

        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUri
        };

        return Challenge(
            properties,
            GoogleDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserResponse> GetCurrentUser()
    {
        var id =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        var displayName =
            User.FindFirstValue(ClaimTypes.Name);

        var email =
            User.FindFirstValue(ClaimTypes.Email);

        var pictureUrl =
            User.FindFirstValue("picture");

        if (string.IsNullOrWhiteSpace(id) ||
            string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized();
        }

        return Ok(
            new CurrentUserResponse(
                id,
                displayName ?? email,
                email,
                pictureUrl));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }

    private static string GetSafeReturnPath(
        string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return "/";
        }

        var decodedReturnUrl =
            Uri.UnescapeDataString(returnUrl.Trim());

        var isSafeRelativePath =
            decodedReturnUrl.StartsWith('/') &&
            !decodedReturnUrl.StartsWith("//") &&
            !decodedReturnUrl.StartsWith("/\\") &&
            !decodedReturnUrl.Contains(
                '\r',
                StringComparison.Ordinal) &&
            !decodedReturnUrl.Contains(
                '\n',
                StringComparison.Ordinal);

        return isSafeRelativePath
            ? decodedReturnUrl
            : "/";
    }
}