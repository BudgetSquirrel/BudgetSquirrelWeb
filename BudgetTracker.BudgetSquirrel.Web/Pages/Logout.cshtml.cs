using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports.Repositories;

using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class LogoutModel : PageModel
    {
        private IUserRepository _userRepository;

        public LogoutModel(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public async Task OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage(LoginModel.PageName);
        }
    }
}
