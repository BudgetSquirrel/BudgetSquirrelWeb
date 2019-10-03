using BudgetTracker.BudgetSquirrel.Application;
using BudgetTracker.BudgetSquirrel.Web.Auth;
using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Transactions;
using BudgetTracker.Business.Ports.Repositories;

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class IndexModel : BasePageModel
    {
        public const string PageName = "Index";

        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;
        private BudgetService _budgetService;

        public Guid? RootBudgetId { get; set; }
        public Budget RootBudget { get; set; }
        public List<Budget> AvailableRootBudgets { get; set; }

        public Dictionary<Guid, List<Transaction>> TransactionsByBudget { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IndexModel(IUserRepository userRepo, ITransactionRepository transactionRepo,
            ILoginService loginService, BudgetService budgetService)
            : base(loginService)
        {
            _transactionRepository = transactionRepo;
            _userRepository = userRepo;
            _budgetService = budgetService;
        }

        protected async Task Initialize()
        {
            if (RootBudgetId != null)
            {
                RootBudget = await _budgetService.GetBudgetTree(RootBudgetId.Value);
            }
            else
            {
                List<Budget> rootBudgets = await _budgetService.GetRootBudgets(CurrentUser.Id.Value);
                if (rootBudgets.Count() == 1)
                {
                    // User only has 1 root budget so just automatically choose that one.
                    await InitializeAsImpliedRootBudgetDetail(rootBudgets.First());
                }
                else
                {
                    // User has multiple or no root budgets so show the list for
                    // them to select the one to view.
                    AvailableRootBudgets = rootBudgets;
                }
            }
            (StartDate, EndDate) = GetDateWindow();
        }

        public async Task<IActionResult> OnGet(Guid? bid)
        {
            RootBudgetId = bid;
            IActionResult loginRedirect;
            if ( (loginRedirect = await AuthenticateOrGoLogin()) != null ) return loginRedirect;

            await Initialize();

            return Page();
        }

        protected virtual (DateTime, DateTime) GetDateWindow()
        {
            DateTime today = DateTime.Now;

            DateTime start = new DateTime(today.Year, today.Month, 1);
            DateTime end = new DateTime(today.Year,
                       today.Month,
                       DateTime.DaysInMonth(today.Year,
                                            today.Month));
            return (start, end);
        }

        protected virtual async Task LoadTransactions(IEnumerable<Budget> budgets)
        {
            foreach (Budget budget in budgets.ToList())
            {
                IEnumerable<Transaction> fetchedTransactions = await budget.GetTransactions(StartDate, EndDate, _transactionRepository);
                TransactionsByBudget[budget.Id] = fetchedTransactions.ToList();
                await LoadTransactions(budget.SubBudgets);
            }
        }

        protected virtual async Task InitializeAsImpliedRootBudgetDetail(Budget chosenRootBudget)
        {
            RootBudgetId = chosenRootBudget.Id;
            RootBudget = await _budgetService.GetBudgetTree(chosenRootBudget);
        }
    }
}
