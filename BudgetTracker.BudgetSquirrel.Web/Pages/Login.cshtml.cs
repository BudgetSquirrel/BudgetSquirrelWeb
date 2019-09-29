using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports;

using System;
using System.Collections.Generic;
using System.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private IUserRepository _userRepository;

        public RegisterModel(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public async Task<IActionResult> OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogin()
        {
            // string username = Request["Username"];
            // string password = Request["Password"];

            User authenticatedUser = new User()
            {
                Email = "user1@gmail.com",
                UserName = "user1"
            };

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email)
            };

            ClaimsIdentity identity = ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToPage("Index");
        }
    }
}
