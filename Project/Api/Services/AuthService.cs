using Api.DTOs;
using Api.Helpers;
using api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration configuration;
    private readonly IUsersRepository usersRepository;

    public AuthService(IConfiguration configuration, IUsersRepository usersRepository)
    {
        this.configuration = configuration;
        this.usersRepository = usersRepository;
    }

    public async Task<TokensDTO> Login(UserDTO dto, CancellationToken cancellationToken)
    {
        User? user = await this.usersRepository.GetUserByUsernameAsync(dto.Username, cancellationToken);

        if (user == null || !user.IsPasswordValid(dto.Password)) throw new UnauthorizedAccessException();

        string accessToken = AuthHelpers.GenerateJwt(user, this.configuration["Auth:JwtSecret"]!);
        string refreshToken = user.GenerateNewRefreshToken();

        await this.usersRepository.SaveChangesAsync(cancellationToken);

        return new TokensDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task Register(UserDTO dto, CancellationToken cancellationToken)
    {
        User user = User.CreateUser(dto.Username, dto.Password);

        this.usersRepository.Add(user);
        await this.usersRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<TokensDTO> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        User? user = await this.usersRepository.GetUserByRefreshTokenAsync(refreshToken, cancellationToken);

        if (user == null) throw new SecurityTokenException("Invalid refresh token");

        if (user.RefreshTokenExpiration < DateTime.Now) throw new SecurityTokenException("Refresh token expired");

        string accessToken = AuthHelpers.GenerateJwt(user, this.configuration["Auth:JwtSecret"]!);
        string newRefreshToken = user.GenerateNewRefreshToken();

        await this.usersRepository.SaveChangesAsync(cancellationToken);

        return new TokensDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }
}
