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

        public BudgetStatus StatusSummary
        {
            get
            {
                BudgetStatus status = BudgetStatus.Good;
                double percentOfBudgetLeft = (double) (BudgetAmountLeft / Budget.SetAmount.Value);
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
        public decimal TransactionsTotal
        {
            get
            {
                if (_transactionsTotal == null)
                {
                    _transactionsTotal = Budget.CalculateTransactionsTotalNetValue(Transactions);
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
        public decimal BudgetAmountLeft => (FundBalance + TransactionsTotal);

        /// <summary>
        /// <p>
        /// Amount of money spent represented by the fetched Transactions. This
        /// does not factor in expenses (negative transactions), only earnings
        /// (positive transactions).
        /// </p>
        /// </summary>
        public decimal AmountIn
        {
            get
            {
                return Transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            }
        }

        /// <summary>
        /// <p>
        /// Amount of money spent represented by the fetched Transactions. This
        /// does not factor in earnings (positive transactions), only expenses
        /// (negative transactions). This value is returned as the absolute
        /// value (always positive).
        /// </p>
        /// </summary>
        public decimal AmountOut
        {
            get
            {
                return -1 * Transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
            }
        }

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
