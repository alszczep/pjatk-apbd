using Api.DTOs;

namespace Api.Services.Interfaces;

public interface IAuthService
{
    public Task<TokensDTO> Login(UserDTO dto, CancellationToken cancellationToken);
    public Task Register(UserDTO dto, CancellationToken cancellationToken);
    public Task<TokensDTO> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}
