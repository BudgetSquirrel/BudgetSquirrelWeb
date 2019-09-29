using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> OnPostRegister()
        {
            // User user = new User()
            // {
            //     UserName =
            // };
        }
    }
}
