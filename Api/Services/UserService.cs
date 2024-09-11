using Api.Data.DTO;
using Api.Data.Model;
using Api.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService,
            ILogger<UserService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string>> RegisterUserAsync(RegisterDto registerDto)
        {
            // Check if user already exists
            if (await _userRepository.UserExistsAsync(registerDto.Username, registerDto.Email))
            {
                return new ServiceResponse<string> { Success = false, Message = "User already exists" };
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create a new user
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserAsync(user);
            _logger.LogInformation("User {Username} registered successfully", user.Username);

            return new ServiceResponse<string> { Success = true, Message = "User registered successfully" };
        }

        public async Task<ServiceResponse<string?>> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(loginDto.UsernameOrEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for {UsernameOrEmail}", loginDto.UsernameOrEmail);
                return new ServiceResponse<string?> { Success = false, Message = "Invalid username or password" };
            }

            var token = _tokenService.GenerateToken(user);
            _logger.LogInformation("User {Username} logged in successfully", user.Username);

            return new ServiceResponse<string?> { Success = true, Data = token };
        }

        public Guid GetUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            throw new Exception("User is not authenticated.");
        }
    }
}
