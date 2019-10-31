using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.BudgetPeriods;
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
        private BudgetPeriodCalculator _budgetPeriodCalculator;

        public BudgetService(IBudgetRepository budgetRepo, IUserRepository userRepo,
            ITransactionRepository transactionRepo,
            BudgetPeriodCalculator budgetPeriodCalculator)
        {
            _budgetRepository = budgetRepo;
            _transactionRepository = transactionRepo;
            _userRepository = userRepo;
            _budgetPeriodCalculator = budgetPeriodCalculator;
        }

        public Task<List<Budget>> GetRootBudgets(Guid userId)
        {
            return _budgetRepository.GetRootBudgets(userId);
        }

        public async Task<BudgetViewModel> GetBudgetTreeForCurrentPeriod(Guid rootBudgetId)
        {
            Budget rootBudget = await _budgetRepository.GetBudget(rootBudgetId);
            return await GetBudgetTreeForCurrentPeriod(rootBudget);
        }

        public async Task<BudgetViewModel> GetBudgetTree(Guid rootBudgetId,
            DateTime budgetPeriodStart, DateTime budgetPeriodEnd)
        {
            Budget rootBudget = await _budgetRepository.GetBudget(rootBudgetId);
            return await GetBudgetTree(rootBudget, budgetPeriodStart, budgetPeriodEnd);
        }

        public async Task<BudgetViewModel> GetBudgetTreeForCurrentPeriod(Budget rootBudget)
        {
            BudgetPeriod currentPeriod = await _budgetPeriodCalculator.GetOrCreateForDate(rootBudget, DateTime.Now);
            return await GetBudgetTree(rootBudget, currentPeriod.StartDate, currentPeriod.EndDate);
        }

        public async Task<BudgetViewModel> GetBudgetTree(Budget rootBudget,
            DateTime budgetPeriodStart, DateTime budgetPeriodEnd)
        {
            BudgetViewModel budgetVM = new BudgetViewModel(rootBudget, null);

            await _budgetRepository.LoadSubBudgets(rootBudget, true);
            budgetVM.TransactionsByBudget = await LoadTransactions(new List<Budget>() { rootBudget },
                                    budgetPeriodStart, budgetPeriodEnd);
            return budgetVM;
        }

        public async Task<Dictionary<Guid, List<Transaction>>> LoadTransactions(IEnumerable<Budget> budgets,
            DateTime budgetPeriodStart, DateTime budgetPeriodEnd)
        {
            Dictionary<Guid, List<Transaction>> transactionsByBudget = new Dictionary<Guid, List<Transaction>>();
            foreach (Budget budget in budgets.ToList())
            {
                IEnumerable<Transaction> fetchedTransactions = await budget.GetTransactions(budgetPeriodStart,
                                                                        budgetPeriodEnd, _transactionRepository);
                transactionsByBudget[budget.Id] = fetchedTransactions.ToList();
                Dictionary<Guid, List<Transaction>> subBudgetsTransactionsByBudget = await LoadTransactions(budget.SubBudgets, budgetPeriodStart, budgetPeriodEnd);
                AddRangeDictionary(transactionsByBudget, subBudgetsTransactionsByBudget);
            }
            return transactionsByBudget;
        }

        public async Task<Budget> CreateSubBudget(SubBudgetCreationViewModel input, User owner)
        {
            Budget created = await BudgetCreation.CreateBudgetForUser(input.ToDomain(), owner, _budgetRepository);
            return created;
        }

        public async Task<Budget> EditBudget(EditBudgetViewModel input, User owner)
        {
            Budget toEdit = await _budgetRepository.GetBudget(input.Id);
            await toEdit.LoadParentBudget(_budgetRepository);

            decimal originalSetAmount = toEdit.SetAmount.Value;
            input.SetModifications(toEdit);
            toEdit.SetAmount = toEdit.CalculateBudgetSetAmount();
            decimal setAmountDifference = toEdit.SetAmount.Value - originalSetAmount;
            toEdit.FundBalance += setAmountDifference; // TODO: Use a transaction instead to do this?

            Budget modified = await _budgetRepository.UpdateBudget(toEdit);
            return modified;
        }

        private void AddRangeDictionary<K,V>(Dictionary<K,V> destination, Dictionary<K,V> source)
        {
            foreach (K key in source.Keys)
            {
                if (destination.ContainsKey(key)) throw new InvalidOperationException("Found duplicate keys.");
                destination[key] = source[key];
            }
        }
    }
}
