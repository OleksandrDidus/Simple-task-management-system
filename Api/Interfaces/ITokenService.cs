using Api.Data.Model;

namespace Api.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
