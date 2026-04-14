using E_Commerce_API.Models.DTO;

namespace E_Commerce_API.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto dto);
        Task<string> Login(LoginDto dto);
    }
}
