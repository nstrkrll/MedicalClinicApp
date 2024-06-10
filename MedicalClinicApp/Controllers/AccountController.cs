using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MedicalClinicApp.Repositories;
using MedicalClinicApp.Models.ViewModels;

namespace MedicalClinicApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository _repository;

        public AccountController(AccountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Auth()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Auth(UserViewModel credentials)
        {
            if (ModelState.IsValid)
            {
                if (_repository.CheckAuth(credentials))
                {
                    await Authenticate(credentials.Login);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(credentials);
        }

        private async Task Authenticate(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<ActionResult> Register(UserViewModel credentials)
        {
            if (ModelState.IsValid)
            {
                if (_repository.Register(credentials))
                {
                    await Authenticate(credentials.Login);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(credentials);
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Auth", "Account");
        }
    }
}
