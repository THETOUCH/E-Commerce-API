using E_Commerce_API.Exceptions;
using E_Commerce_API.Models;
using E_Commerce_API.Models.DTO;
using E_Commerce_API.Repository;
using E_Commerce_API.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using ValidationException = E_Commerce_API.Exceptions.ValidationException;

namespace E_Commerce_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationContext _context;

        public AuthService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);
            if (user != null)
            {
                throw new NotUniqueEmail("Email must be unique");
            }
            string name = dto.Name;
            string email = dto.Email;
            string password = dto.Password;

            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ValidationException("Name, email and password must not be empty");
            }

            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
            {
                throw new ValidationException($"{email} is not a valid email address");
            }

            PasswordHasher<object> passwordHasher = new PasswordHasher<object>();
            string hashed = passwordHasher.HashPassword(null, password);

            User newUser = new User(name, email, hashed)
            {
                Role = dto.Role
            };
            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            string token = GetToken(newUser);

            return token;
        }

        private string GetToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }

        public async Task<string> Login(LoginDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            User? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);

            if (user == null)
            {
                throw new NotFoundException("not found login terms in db");
            }

            string password = dto.Password;

            PasswordHasher<object> passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, password);



            PasswordVerificationResult passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, user.Password, dto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new ValidationException("password not matched");
            }


            string token = GetToken(user);

            return token;
        }
    }
}
