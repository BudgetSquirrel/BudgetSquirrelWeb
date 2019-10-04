using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class BudgetViewModel
    {
        public Budget Budget { get; set; }
        public Dictionary<Guid, List<Transaction>> TransactionsByBudget { get; set; }

        /// <summary>
        /// <p>
        /// Total amount of money spent (including amounts gained) in the list
        /// of transactions in TransactionsByBudget for this budget.
        /// </p>
        /// <p>
        /// If this amount is positive, then that implies that the transactions
        /// result in a net positive earning. Otherwise, if it is negative, that
        /// implies that the user who performed the transactions spent more than
        /// they put into in this budget.
        /// </p>
        /// </summary>
        public decimal TransactionsTotal
        {
            get
            {
                if (_transactionsTotal == null)
                {
                    _transactionsTotal = Budget.CalculateTransactionsTotalNetValue(
                            TransactionsByBudget[Budget.Id]);
                }
                return _transactionsTotal.Value;
            }
        }
        private decimal? _transactionsTotal;

        /// <summary>
        /// <p>
        /// Amount of money left out of this budgets SetAmount. This is the amount
        /// that the user has left to spend in this budget for the budget period
        /// represented by the Transactions in TransactionsByBudget for this
        /// budget.
        /// </p>
        /// </summary>
        public decimal BudgetAmountLeft => (Budget.SetAmount.Value + TransactionsTotal);

        public BudgetViewModel() {}

        public BudgetViewModel(Budget budget, Dictionary<Guid, List<Transaction>> transactionsByBudget)
        {
            Budget = budget;
            TransactionsByBudget = transactionsByBudget;
        }

        public BudgetViewModel GetSubBudgetViewModel(Guid subBudgetId)
        {
            if (!Budget.SubBudgets.Select(b => b.Id).Contains(subBudgetId))
                throw new ArgumentException($"The budget (id: {subBudgetId} is not a sub budget of this budget {Budget.Id}.");

            return new BudgetViewModel(Budget.SubBudgets.Single(b => b.Id == subBudgetId),
                                        TransactionsByBudget);
        }
    }
}
