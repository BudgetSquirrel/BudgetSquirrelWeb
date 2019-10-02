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

        private IBudgetRepository _budgetRepository;
        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;

        public Budget RootBudget { get; set; }

        public Dictionary<Guid, List<Transaction>> TransactionsByBudget { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IndexModel(IBudgetRepository budgetRepo, IUserRepository userRepo,
            ITransactionRepository transactionRepo, ILoginService loginService)
            : base(loginService)
        {
            _budgetRepository = budgetRepo;
            _transactionRepository = transactionRepo;
            _userRepository = userRepo;
        }

        protected async Task Initialize()
        {
            (StartDate, EndDate) = GetDateWindow();
        }

        public async Task<IActionResult> OnGet()
        {
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
    }
}
