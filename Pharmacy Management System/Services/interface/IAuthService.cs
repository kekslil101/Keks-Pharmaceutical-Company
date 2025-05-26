using Pharmacy_Management_System.Models.DTOs;

namespace Pharmacy_Management_System.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}

