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

        public IViewComponentResult Invoke(BudgetViewModel budget=null,
                                            Budget parentBudget=null,
                                            bool isCreationForm=false)
        {
            if (isCreationForm)
                return InvokeAsSubBudgetCreationForm(parentBudget);
            else
                return InvokeAsBudgetDisplayForm(budget);
        }

        private IViewComponentResult InvokeAsBudgetDisplayForm(BudgetViewModel budget)
        {
            if (budget.Budget.IsRootBudget)
            {
                return View("RootBudget", budget);
            }
            return View(budget);
        }

        private IViewComponentResult InvokeAsSubBudgetCreationForm(Budget parentBudget)
        {
            SubBudgetCreationViewModel form = new SubBudgetCreationViewModel(parentBudget.Id);
            return View("SubBudgetCreateForm", form);
        }
    }
}
