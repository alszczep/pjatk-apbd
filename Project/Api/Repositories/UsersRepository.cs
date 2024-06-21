using Api.Context;
using api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ProjectContext projectContext;

    public UsersRepository(ProjectContext projectContext)
    {
        this.projectContext = projectContext;
    }

    public Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return this.projectContext.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return this.projectContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken,
            cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.projectContext.SaveChangesAsync(cancellationToken);
    }

    public void Add(User user)
    {
        this.projectContext.Users.Add(user);
    }
}
