using BudgetTracker.Business.Budgeting;
using System;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class EditBudgetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? SetAmount { get; set; }

        /// <summary>
        /// The value of this is expected to be a whole
        /// number percent. For example, if the value is
        /// representing 35%, then the value is going to
        /// be 35, not 0.35. It is an integer based percent.
        /// To get the real value for the budget, divide by
        /// 100.
        ///
        /// This is done because the user should put in integer
        /// based percents which are more user friendly. This
        /// is a direct dump from the users input.
        /// </summary>
        public double? PercentAmount { get; set; }

        public EditBudgetViewModel() {}

        public EditBudgetViewModel(Budget budget)
        {
            Id = budget.Id;
            Name = budget.Name;
            if (budget.IsPercentBasedBudget)
            {
                PercentAmount = budget.PercentAmount * 100;
            }
            else
            {
                SetAmount = budget.SetAmount;
            }
        }

        public virtual void SetModifications(Budget toModify)
        {
            toModify.Name = Name;
            toModify.SetAmount = SetAmount;
            toModify.PercentAmount = PercentAmount != null ? (PercentAmount / 100) : null;
        }
    }
}
