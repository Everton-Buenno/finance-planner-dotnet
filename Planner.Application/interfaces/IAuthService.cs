using Planner.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Application.interfaces
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
        string GenerateJwtToken(Guid userId, string name, string email);
        Task<ResultViewModel> LoginAsync(LoginUserInputModel input);
        Task<ResultViewModel> RegisterAsync(RegisterUserInputModel input);
    }
}
