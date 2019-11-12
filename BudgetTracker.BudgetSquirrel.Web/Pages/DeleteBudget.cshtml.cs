using BudgetTracker.BudgetSquirrel.Application.Budgeting;
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
using Newtonsoft.Json;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class DeleteBudgetModel : BasePageModel
    {
        public const string PageName = "DeleteBudget";

        private BudgetService _budgetService;

        public DeleteBudgetModel(ILoginService loginService, BudgetService budgetService)
            : base(loginService)
        {
            _budgetService = budgetService;
        }

        public async Task<IActionResult> OnPost(Guid budgetId)
        {
            IActionResult loginRedirect;
            if ( (loginRedirect = await AuthenticateOrGoLogin()) != null ) return loginRedirect;

            Console.WriteLine(budgetId);
            throw new NotImplementedException("Delete budget not implemented.");

            // return RedirectToPage(IndexModel.PageName);
        }
    }
}
