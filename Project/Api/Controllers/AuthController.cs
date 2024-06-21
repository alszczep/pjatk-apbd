using Api.DTOs;
using Api.Helpers;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Microsoft.AspNetCore.Components.Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly IConfiguration configuration;

    public AuthController(IConfiguration configuration, IAuthService authService)
    {
        this.configuration = configuration;
        this.authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterStudent([FromBody] UserDTO dto, CancellationToken cancellationToken)
    {
        await this.authService.Register(dto, cancellationToken);

        return this.Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokensDTO>> Login([FromBody] UserDTO dto, CancellationToken cancellationToken)
    {
        TokensDTO tokens = await this.authService.Login(dto, cancellationToken);

        return this.Ok(tokens);
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh(RefreshTokenDTO dto, CancellationToken cancellationToken)
    {
        TokensDTO tokens = await this.authService.RefreshToken(dto.RefreshToken, cancellationToken);

        return this.Ok(tokens);
    }

    [Authorize]
    [HttpGet("authtest")]
    public ActionResult AuthTest()
    {
        return this.Ok(AuthHelpers.GetUsernameFromClaimsPrincipal(this.User));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admintest")]
    public ActionResult AdminTest()
    {
        return this.Ok(AuthHelpers.GetRoleFromClaimsPrincipal(this.User));
    }
}
