using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Ports.Repositories;
using BudgetTracker.Business.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class BudgetService
    {
        private IBudgetRepository _budgetRepository;
        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;

        public BudgetService(IBudgetRepository budgetRepo, IUserRepository userRepo,
            ITransactionRepository transactionRepo)
        {
            _budgetRepository = budgetRepo;
            _transactionRepository = transactionRepo;
            _userRepository = userRepo;
        }

        public Task<List<Budget>> GetRootBudgets(Guid userId)
        {
            return _budgetRepository.GetRootBudgets(userId);
        }

        public async Task<Budget> GetBudgetTree(Guid rootBudgetId)
        {
            Budget rootBudget = await _budgetRepository.GetBudget(rootBudgetId);
            return await GetBudgetTree(rootBudget);
        }

        public async Task<Budget> GetBudgetTree(Budget rootBudget)
        {
            await _budgetRepository.LoadSubBudgets(rootBudget, true);
            return rootBudget;
        }
    }
}
