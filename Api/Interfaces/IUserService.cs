using Api.Data.DTO;
using Api.Services;

namespace Api.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<string>> RegisterUserAsync(RegisterDto registerDto);
        Task<ServiceResponse<string?>> LoginUserAsync(LoginDto loginDto);
        Guid GetUserId();
    }

}
