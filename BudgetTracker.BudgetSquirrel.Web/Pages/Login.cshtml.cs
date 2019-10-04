using BudgetTracker.BudgetSquirrel.Web.Auth;
using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports.Repositories;

using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class LoginModel : PageModel
    {
        public const string PageName = "Login";

        private IUserRepository _userRepository;
        private ILoginService _loginService;

        public LoginModel(IUserRepository userRepo, ILoginService loginService)
        {
            _userRepository = userRepo;
            _loginService = loginService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            string username = Request.Form["Username"];
            string password = Request.Form["Password"];
            username = username != "" ? username : "user1";
            password = password != "" ? password : "user1234";

            User loggedInUser = await _loginService.Login(username, password);
            if (loggedInUser == null)
            {
                TempData["FailedLogin"] = true;
                return RedirectToPage();
            }
            return RedirectToPage(IndexModel.PageName);
        }
    }
}
