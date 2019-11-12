using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Ports.Repositories;
using BudgetTracker.Business.Transactions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Application.Transactions
{
    public class TransactionService
    {
        private ITransactionRepository _transactionRepository;
        private IBudgetRepository _budgetRepository;
        private IUserRepository _userRepository;

        public TransactionService(ITransactionRepository transactionRepo,
            IBudgetRepository budgetRepo, IUserRepository userRepo)
        {
            _transactionRepository = transactionRepo;
            _budgetRepository = budgetRepo;
            _userRepository = userRepo;
        }

        public async Task<Transaction> LogTransaction(Transaction transaction)
        {
            Transaction created = await _transactionRepository.CreateTransaction(transaction);
            created.Owner = transaction.Owner;
            created.Budget = await _budgetRepository.GetBudget(created.BudgetId);
            await _budgetRepository.LoadSubBudgets(created.Budget, true);
            await created.Budget.ApplyTransaction(created, _budgetRepository);
            return created;
        }
    }
}
