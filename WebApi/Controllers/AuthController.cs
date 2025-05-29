using System.Text.Json.Serialization;
using Abstractions;
using Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Manager;

// TODO: Wrap IConfiguration in a service
// TODO: Use inbuilt ControllerBase's SignIn and SignOut methods

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator, IInfrastructureGlobalConfig configuration) : ControllerBase
{
    private static readonly string CookieName = "RefreshToken";

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var result = await mediator.Send(command with { UserAgent = Request.Headers["User-Agent"].ToString() });

        if (result == null) return Unauthorized();

        AddCookie(result.RefreshToken, result.ExpiresAt);

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken,
        });
    }

    [HttpPost("sign-out")]
    public async new Task<IActionResult> SignOut()
    {
        Request.Cookies.TryGetValue(CookieName, out var refreshToken);
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("No refresh token found.");

        var command = new SignOutCommand(refreshToken);
        await mediator.Send(command);

        Response.Cookies.Delete(CookieName);
        return NoContent();
    }

    public record RefreshTokensBody([property: JsonPropertyName("fingerprint")] string Fingerprint);
    [HttpPost("refresh-tokens")]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensBody body)
    {
        Request.Cookies.TryGetValue(CookieName, out var refreshToken);
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("No refresh token found.");
        
        var result = await mediator.Send(new RefreshTokenCommand(refreshToken, body.Fingerprint));

        if (result == null) return Unauthorized();

        AddCookie(result.RefreshToken, result.ExpiresAt);

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken,
        });
    }

    private void AddCookie(string refreshToken, DateTime expiresAt)
    {
        Response.Cookies.Append(CookieName, refreshToken.ToString(), new CookieOptions
        {
            Domain = configuration.Domain,
            HttpOnly = true,
            #if !DEBUG
                Secure = true,
            #endif
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth",
            Expires = expiresAt,
            MaxAge = expiresAt - DateTime.UtcNow,
        });
    }
}

