using Api.Data.Model;

namespace Api.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string username, string email);
        Task AddUserAsync(User user);
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    }

}
