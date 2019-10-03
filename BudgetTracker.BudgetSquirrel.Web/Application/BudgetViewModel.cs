using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Transactions;
using System;
using System.Collections.Generic;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class BudgetViewModel
    {
        public Budget Budget { get; set; }
        public Dictionary<Guid, List<Transaction>> TransactionsByBudget { get; set; }

        public BudgetViewModel(Budget budget, Dictionary<Guid, List<Transaction>> transactionsByBudget)
        {
            Budget = budget;
            TransactionsByBudget = transactionsByBudget;
        }
    }
}
