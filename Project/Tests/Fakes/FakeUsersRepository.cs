using api.Models;
using Api.Repositories.Interfaces;

namespace Tests.Fakes;

public class FakeUsersRepository : IUsersRepository
{
    public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Add(User user)
    {
    }
}
