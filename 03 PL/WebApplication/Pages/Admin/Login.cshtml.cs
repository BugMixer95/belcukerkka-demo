using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Belcukerkka.Models.Entities;
using Belcukerkka.Models.ViewModels;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Services.Security;
using Belcukerkka.TelegramNotifier;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Admin
{
    public class LoginModel : PageModel
    {
        public LoginModel(IEntityRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        private readonly IEntityRepository<User> _userRepository;

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }

        public void OnGet()
        {
            LoginViewModel = new LoginViewModel();
        }

        public async Task<IActionResult> OnPostLogin()
        {
            var checker = new AuthorizationChecker(_userRepository);

            bool isPassed = checker.CheckCredentials(LoginViewModel);

            if (isPassed)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, LoginViewModel.LoginName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

                var notifier = new AccessAttemptSender();
                await notifier.SendLoginAttemptMessageAsync(true, LoginViewModel, DateTime.Now, HttpContext.Connection.RemoteIpAddress.ToString());

                return RedirectToPage("/Admin/Orders/Index");
            }
            else
            {
                var notifier = new AccessAttemptSender();
                await notifier.SendLoginAttemptMessageAsync(false, LoginViewModel, DateTime.Now, HttpContext.Connection.RemoteIpAddress.ToString());

                return Page();
            }
        }
    }
}
