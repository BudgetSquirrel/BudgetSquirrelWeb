using BudgetTracker.BudgetSquirrel.Application;
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
    public class IndexModel : BasePageModel
    {
        public const string PageName = "Index";

        private BudgetService _budgetService;

        public Guid? RootBudgetId { get; set; }
        public BudgetViewModel RootBudget { get; set; }
        public List<Budget> AvailableRootBudgets { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IndexModel(ILoginService loginService, BudgetService budgetService)
            : base(loginService)
        {
            _budgetService = budgetService;
        }

        protected async Task Initialize()
        {
            if (RootBudgetId != null)
            {
                (StartDate, EndDate) = GetDateWindow();
                RootBudget = await _budgetService.GetBudgetTreeForCurrentPeriod(RootBudgetId.Value);
            }
            else
            {
                List<Budget> rootBudgets = await _budgetService.GetRootBudgets(CurrentUser.Id.Value);
                if (rootBudgets.Count() == 1)
                {
                    RootBudgetId = rootBudgets.First().Id;
                    await Initialize();
                    return;
                }
                else
                {
                    // User has multiple or no root budgets so show the list for
                    // them to select the one to view.
                    AvailableRootBudgets = rootBudgets;
                }
            }
        }

        public async Task<IActionResult> OnGetAsync(Guid? budgetId)
        {
            RootBudgetId = budgetId;
            IActionResult loginRedirect;
            if ( (loginRedirect = await AuthenticateOrGoLogin()) != null ) return loginRedirect;

            await Initialize();

            return Page();
        }

        public async Task<IActionResult> OnPostNewSubBudget(SubBudgetCreationViewModel input)
        {
            IActionResult loginRedirect;
            if ( (loginRedirect = await AuthenticateOrGoLogin()) != null ) return loginRedirect;

            Budget created = await _budgetService.CreateSubBudget(input, CurrentUser);

            return RedirectToPage(IndexModel.PageName);
        }

        public async Task<IActionResult> OnPostEditSubBudget(EditBudgetViewModel input)
        {
            IActionResult loginRedirect;
            if ( (loginRedirect = await AuthenticateOrGoLogin()) != null ) return loginRedirect;

            Budget modified = await _budgetService.EditBudget(input, CurrentUser);

            return RedirectToPage(IndexModel.PageName);
        }

        public async Task<IActionResult> OnPostEditRootBudget(EditRootBudgetViewModel input)
        {
            IActionResult loginRedirect;
            if ( (loginRedirect = await AuthenticateOrGoLogin()) != null ) return loginRedirect;

            Budget modified = await _budgetService.EditBudget(input, CurrentUser);

            return RedirectToPage(IndexModel.PageName);
        }

        protected virtual (DateTime, DateTime) GetDateWindow()
        {
            DateTime today = DateTime.Now;

            DateTime start = new DateTime(today.Year, today.Month, 1);
            DateTime end = new DateTime(today.Year,
                       today.Month,
                       DateTime.DaysInMonth(today.Year,
                                            today.Month));
            return (start, end);
        }
    }
}
