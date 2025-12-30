using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Planner.Application.interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Planner.Application.DTOs.Auth;
using Planner.Application.DTOs;
using System.Threading.Tasks;
using Planner.Domain.Interfaces;
using Planner.Domain.ValueObjects;

namespace Planner.Application.services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

  

        public string GenerateJwtToken(Guid userId, string name, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public async Task<ResultViewModel> LoginAsync(LoginUserInputModel input)
        {
            if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Password))
                return ResultViewModel.Error("E-mail e senha são obrigatórios.");

            var emailVO = new Email(input.Email);
            var user = await _userRepository.GetByEmailAsync(emailVO);
            if (user == null)
                return ResultViewModel.Error("Usuário null ou senha inválidos.");

            if (!VerifyPassword(input.Password, user.PasswordHash))
                return ResultViewModel.Error("Usuário ou senha inválidos.");

            var token = GenerateJwtToken(user.Id, user.Name, user.Email.Value);
            var result = new AuthResult
            {
                Token = token,
                UserId = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email.Value,
                Message = "Login realizado com sucesso."
            };
            return ResultViewModel<AuthResult>.Success(result);
        }

        public async Task<ResultViewModel> RegisterAsync(RegisterUserInputModel input)
        {
            if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Password) || string.IsNullOrEmpty(input.Name))
                return ResultViewModel.Error("Nome, e-mail e senha são obrigatórios.");

            var emailVO = new Email(input.Email);
            var exists = await _userRepository.ExistsByEmailAsync(emailVO);
            if (exists)
                return ResultViewModel.Error("Já existe um usuário com este e-mail.");

            var passwordHash = HashPassword(input.Password);
            var user = new Planner.Domain.Entities.User(input.Name, emailVO, passwordHash);
            try
            {
                await _userRepository.AddAsync(user);
            }
            catch (Exception ex)
            {
                return ResultViewModel.Error($"Erro ao registrar usuário: {ex.Message}");
            }

            var token = GenerateJwtToken(user.Id, user.Name, user.Email.Value);
            var result = new AuthResult
            {
                Token = token,
                UserId = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email.Value,
                Message = "Usuário registrado com sucesso."
            };
            return ResultViewModel<AuthResult>.Success(result, "Usuário registrado com sucesso.");
        }
    }
}
