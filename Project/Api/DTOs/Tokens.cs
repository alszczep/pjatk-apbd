namespace Api.DTOs;

public class RefreshTokenDTO
{
    public string RefreshToken { get; set; }
}

public class TokensDTO : RefreshTokenDTO
{
    public string AccessToken { get; set; }
}
