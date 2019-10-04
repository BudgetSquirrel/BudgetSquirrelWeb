using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public void OnGet()
        {
        }

        public IActionResult OnPostRegister()
        {
            // User user = new User()
            // {
            //     Username =
            // };
            return RedirectToPage("Index");
        }
    }
}
