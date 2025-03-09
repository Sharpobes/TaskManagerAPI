using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Helpers;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly TaskApiContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(TaskApiContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(UserAuthModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Поле {Field}: {Error}", state.Key, error.ErrorMessage);
                        System.Diagnostics.Trace.TraceWarning("Поле {0}: {1}", state.Key, error.ErrorMessage);
                    }
                }
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");
                return View(model);
            }

            string intermediateHash = MD5Hash.ComputeMd5Hash(model.Password);
            string hashedPassword = Convert.ToBase64String(MD5Hash.ComputeMd5HashBytes(intermediateHash));

            if (user.PasswordHash != hashedPassword)
            {
                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(3)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("TasksList", "Tasks");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Поле {Field}: {Error}", state.Key, error.ErrorMessage);
                        System.Diagnostics.Trace.TraceWarning("Поле {0}: {1}", state.Key, error.ErrorMessage);
                    }
                }
                return View(model);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует.");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Пароли не совпадают.");
                return View(model);
            }

            string intermediateHash = MD5Hash.ComputeMd5Hash(model.Password);
            string hashedPassword = Convert.ToBase64String(MD5Hash.ComputeMd5HashBytes(intermediateHash));

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Auth");
        }


        // POST: /Auth/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
