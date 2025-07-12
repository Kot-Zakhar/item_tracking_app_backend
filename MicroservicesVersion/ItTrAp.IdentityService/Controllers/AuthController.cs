using System.Text.Json.Serialization;
using ItTrAp.IdentityService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator, IOptions<GlobalConfig> globalConfig) : ControllerBase
{
    private static readonly string CookieName = "RefreshToken";

    private readonly GlobalConfig configuration = globalConfig.Value;

    public record SignInBody(string Email, string Password, string Fingerprint);
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInBody body)
    {
        var command = new SignInCommand(
            body.Email,
            body.Password,
            body.Fingerprint,
            Request.Headers["User-Agent"].ToString()
        );

        var result = await mediator.Send(command);

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

