using BudgetTracker.BudgetSquirrel.Application;
using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Ports.Repositories;
using BudgetTracker.Business.Transactions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Web.ViewComponents
{
    public class BudgetViewComponent : ViewComponent
    {
        public const string ComponentName = "Budget";

        public BudgetViewModel Budget { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(Budget budget,
            Dictionary<Guid, List<Transaction>> transactionsByBudget)
        {
            Budget = new BudgetViewModel(budget, transactionsByBudget);
            return View(Budget);
        }
    }
}
