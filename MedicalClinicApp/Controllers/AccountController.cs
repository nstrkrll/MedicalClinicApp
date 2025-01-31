﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MedicalClinicApp.Repositories;
using MedicalClinicApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MedicalClinicApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository _accountRepository;

        public AccountController(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
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

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Auth(UserViewModel credentials)
        {
            if (ModelState.IsValid)
            {
                if (await _accountRepository.CheckAuth(credentials))
                {
                    await Authenticate(credentials.Login);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Login", "Некорректные логин и(или) пароль");
            }

            return View(credentials);
        }

        public async Task<IActionResult> Register(UserViewModel credentials)
        {
            if (await _accountRepository.Get(credentials.Login) != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже зарегистрирован");
            }

            if (ModelState.IsValid)
            {
                await _accountRepository.Register(credentials);
                await Authenticate(credentials.Login);
                return RedirectToAction("Index", "Home");
            }

            return View(credentials);
        }

        private async Task Authenticate(string email)
        {
            var currentUser = await _accountRepository.Get(email);
            var claims = new List<Claim>
            {
                new Claim("Email", email),
                new Claim("Role", currentUser.RoleId.ToString())
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Auth", "Account");
        }

        [Authorize]
        public async Task<IActionResult> PatientProfile()
        {
            var profile = await _accountRepository.GetPatientProfile(User.Claims.First(x => x.Type == "Email").Value);
            return View(profile);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PatientProfile(PatientProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.Claims.First(x => x.Type == "Email").Value;
                await _accountRepository.EditPatientProfile(model, email);
                if (User.Claims.First(x => x.Type == "Role").Value == "4")
                {
                    await Authenticate(email);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> EmployeeProfile()
        {
            var profile = await _accountRepository.GetEmployeeProfile(User.Claims.First(x => x.Type == "Email").Value);
            return View(profile);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EmployeeProfile(EmployeePreviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _accountRepository.EditEmployeeProfile(model);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
