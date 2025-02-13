using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;
using TaskManagerAPI.Helpers;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly TaskApiContext _context;
        private readonly IConfiguration _configuration;

        public AuthApiController(TaskApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Ошибка в поле {state.Key}: {error.ErrorMessage}");
                    }
                }
                return BadRequest(ModelState);
            }


            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                return Unauthorized("Неверное имя пользователя или пароль");
            }

            // Хэширование пароля (если использовали MD5 на сайте)
            string intermediateHash = MD5Hash.ComputeMd5Hash(model.Password);
            string hashedPassword = Convert.ToBase64String(MD5Hash.ComputeMd5HashBytes(intermediateHash));

            if (user.PasswordHash != hashedPassword)
            {
                return Unauthorized("Неверное имя пользователя или пароль");
            }

            var token = GenerateJwtToken(user);  // Генерация JWT токена

            return Ok(new { Token = token });
        }

        // Генерация JWT токена
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);  // Возвращаем токен
        }
    }

    // Модель для логина через API
    //public class UserAuthModel
    //{
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //}
}
