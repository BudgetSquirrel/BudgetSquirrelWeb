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

        public async Task<IActionResult> OnPost()
        {

            await HttpContext.SignOutAsync();
            return RedirectToPage("Login");
        }
    }
}
