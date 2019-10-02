using BudgetTracker.BudgetSquirrel.Web.Auth;
using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Transactions;
using BudgetTracker.Business.Ports.Repositories;

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class BasePageModel : PageModel
    {
        protected ILoginService _loginService;

        public User CurrentUser { get; protected set; }

        public BasePageModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        protected virtual async Task<IActionResult> AuthenticateOrGoLogin()
        {
            if (await _loginService.IsAuthenticated())
            {
                CurrentUser = await _loginService.GetUser();
                ViewData["UserFullName"] = $"{CurrentUser.FirstName} {CurrentUser.LastName}";
                return null;
            }
            else
            {
                return RedirectToPage(LoginModel.PageName);
            }
        }
    }
}
