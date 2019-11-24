using BudgetTracker.Business.Budgeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Application.Budgeting
{
    public class BudgetCreationViewModel
    {
        public string Name { get; set; }
        public double? PercentAmount { get; set; }
        public decimal? SetAmount { get; set; }

        public virtual CreateBudgetRequestMessage ToCreateMessage()
        {
            CreateBudgetRequestMessage budget = new CreateBudgetRequestMessage()
            {
                Name = Name,
                PercentAmount = PercentAmount,
                SetAmount = SetAmount
            };
            return budget;
        }
    }
}
