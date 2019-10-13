using BudgetTracker.Business.Budgeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class BudgetCreationViewModel
    {
        public string Name { get; set; }
        public double? PercentAmount { get; set; }
        public decimal? SetAmount { get; set; }

        public virtual Budget ToDomain()
        {
            Budget budget = new Budget()
            {
                Name = Name,
                PercentAmount = PercentAmount,
                SetAmount = SetAmount
            };
            return budget;
        }
    }
}
