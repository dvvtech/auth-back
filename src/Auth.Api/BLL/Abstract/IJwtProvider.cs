namespace Auth.Api.BLL.Abstract
{
    public interface IJwtProvider
    {
        string GenerateToken(string userName, int accountId);

        string GenerateRefreshToken();
    }
}
