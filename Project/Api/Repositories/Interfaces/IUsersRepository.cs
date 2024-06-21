using api.Models;

namespace Api.Repositories.Interfaces;

public interface IUsersRepository
{
    public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public void Add(User user);
}
