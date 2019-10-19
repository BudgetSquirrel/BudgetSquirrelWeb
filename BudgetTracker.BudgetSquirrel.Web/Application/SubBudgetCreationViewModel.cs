using BudgetTracker.Business.Budgeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class SubBudgetCreationViewModel : BudgetCreationViewModel
    {
        public Guid ParentBudgetId { get; set; }

        public SubBudgetCreationViewModel(Guid parentBudgetId)
        {
            ParentBudgetId = parentBudgetId;
        }

        public SubBudgetCreationViewModel() {}

        public override Budget ToDomain()
        {
            Budget budget = base.ToDomain();
            budget.ParentBudgetId = ParentBudgetId;
            return budget;
        }
    }
}
