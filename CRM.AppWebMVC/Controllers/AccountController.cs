using CRM.AppWebMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.AppWebMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiAuthService _apiAuthService;

        public AccountController(ApiAuthService apiAuthService)
        {
            _apiAuthService = apiAuthService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Llamar al servicio para autenticar al usuario
            var tokenResponse = await _apiAuthService.LoginAsync(model.Name, model.Password);

            if (tokenResponse == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View(model);
            }

            // Crear los claims para la autenticación
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Name),
                new Claim("Token", tokenResponse.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Mantener la sesión
            };

            // Iniciar sesión
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            // Redirigir a la página principal u otra ruta
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
