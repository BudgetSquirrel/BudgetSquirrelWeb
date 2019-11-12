using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.BudgetSquirrel.Application;
using BudgetTracker.BudgetSquirrel.Application.Auth;
using BudgetTracker.BudgetSquirrel.Web.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class RegisterModel : PageModel
    {
        public static string PageName = "Register";
        
        private UserService _userService;
        private ILoginService _loginService;

        [TempData]
        public string ErrorMessage { get; set; }

        public RegisterModel(UserService userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        public async Task OnGet()
        {
            await _loginService.Logout();
        }

        public async Task<IActionResult> OnPostRegister(CreateUserViewModel input)
        {
            try
            {
                (User user, Budget rootBudget) = await _userService.RegisterUserAndRootBudget(input);
                await _loginService.Logout();
                await _loginService.Login(user);
                return RedirectToPage(IndexModel.PageName);
            }
            catch (BudgetSquirrelException e)
            {
                ErrorMessage = e.Message;
                return RedirectToPage();
            }
        }
    }
}
