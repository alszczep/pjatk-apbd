using Api.Helpers;

namespace api.Models;

public enum UserRole
{
    User = 0,
    Admin = 1
}

public class User
{
    public string Username { get; init; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiration { get; set; }
    public string Salt { get; private set; } = null!;

    public static User CreateUser(string username, string password)
    {
        var hashedPasswordAndSalt = AuthHelpers.GetHashedPasswordAndSalt(password);

        User user = new()
        {
            Username = username,
            PasswordHash = hashedPasswordAndSalt.Item1,
            Role = UserRole.User,
            RefreshToken = AuthHelpers.GenerateRefreshToken(),
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(1),
            Salt = hashedPasswordAndSalt.Item2
        };

        return user;
    }

    public bool IsPasswordValid(string password)
    {
        return this.PasswordHash == AuthHelpers.GetHashedPasswordWithSalt(password, this.Salt);
    }

    public string GenerateNewRefreshToken()
    {
        this.RefreshToken = AuthHelpers.GenerateRefreshToken();
        this.RefreshTokenExpiration = DateTime.UtcNow.AddDays(1);
        return this.RefreshToken;
    }
}
