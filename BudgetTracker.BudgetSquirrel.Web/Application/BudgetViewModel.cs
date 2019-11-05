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

        public List<Transaction> Transactions => TransactionsByBudget[Budget.Id];

        public EditBudgetViewModel EditForm { get; set; }

        public BudgetStatus StatusSummary
        {
            get
            {
                BudgetStatus status = BudgetStatus.Good;
                double percentOfBudgetLeft = (double) ((double)BalanceWithPlannedBudget / (double)Budget.SetAmount.Value);
                if (percentOfBudgetLeft > AppConstants.BUDGET_STATUS_WARNING_THRESHOLD)
                {
                    status = BudgetStatus.Good;
                }
                else if (percentOfBudgetLeft > AppConstants.BUDGET_STATUS_BAD_THRESHOLD)
                {
                    status = BudgetStatus.Warning;
                }
                else
                {
                    status = BudgetStatus.Bad;
                }
                return status;
            }
        }

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
        public decimal TransactionsTotal => Budget.CalculateTransactionsTotalNetValue(Transactions);

        /// <summary>
        /// <p>
        /// Amount of money spent represented by the fetched Transactions. This
        /// does not factor in expenses (negative transactions), only earnings
        /// (positive transactions).
        /// </p>
        /// </summary>
        public decimal AmountIn => Budget.GetAmountIn(Transactions);

        /// <summary>
        /// <p>
        /// Amount of money spent represented by the fetched Transactions. This
        /// does not factor in earnings (positive transactions), only expenses
        /// (negative transactions). This value is returned as the absolute
        /// value (always positive).
        /// </p>
        /// </summary>
        public decimal AmountOut => Budget.GetAmountOut(Transactions);

        /// <summary>
        /// <p>
        /// Amount of that the budget started with before the specified transactions
        /// were logged. A simple way to say this is that this is the balance
        /// before the budget period represented in the TransactionsByBudget
        /// started.
        /// </p>
        /// </summary>
        public decimal StartBalance => (Budget.FundBalance - TransactionsTotal);

        public decimal FundBalance => Budget.FundBalance;

        /// <summary>
        /// <p>
        /// Amount of that the budget left to spend if you factor in the amount
        /// that is planned to be added as income. This is basically the current
        /// balance added to the planned budget for the budget period.
        /// </p>
        /// </summary>
        public decimal BalanceWithPlannedBudget => FundBalance + Budget.SetAmount.Value;

        public BudgetViewModel() {}

        public BudgetViewModel(Budget budget, Dictionary<Guid, List<Transaction>> transactionsByBudget)
        {
            Budget = budget;
            TransactionsByBudget = transactionsByBudget;

            EditForm = GetEditForm(budget);
        }

        public BudgetViewModel GetSubBudgetViewModel(Guid subBudgetId)
        {
            if (!Budget.SubBudgets.Select(b => b.Id).Contains(subBudgetId))
                throw new ArgumentException($"The budget (id: {subBudgetId} is not a sub budget of this budget {Budget.Id}.");

            return new BudgetViewModel(Budget.SubBudgets.Single(b => b.Id == subBudgetId),
                                        TransactionsByBudget);
        }

        private EditBudgetViewModel GetEditForm(Budget budget)
        {
            if (budget.IsRootBudget)
            {
                return new EditRootBudgetViewModel(budget);
            }
            else
            {
                return new EditBudgetViewModel(budget);
            }
        }
    }
}
