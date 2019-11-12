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
using Newtonsoft.Json;
using BudgetTracker.BudgetSquirrel.Application.Budgeting;
using BudgetTracker.BudgetSquirrel.Application.Transactions;

namespace BudgetTracker.BudgetSquirrel.Web.Pages
{
    public class LogTransactionModel : BasePageModel
    {
        public const string PageName = "LogTransaction";

        private BudgetService _budgetService;
        private TransactionService _transactionsService;

        public LogTransactionModel(ILoginService loginService, BudgetService budgetService,
            TransactionService transactionService)
            : base(loginService)
        {
            _budgetService = budgetService;
            _transactionsService = transactionService;
        }

        public async Task<IActionResult> OnPost(Transaction transaction)
        {
            IActionResult loginRedirect;
            if ( (loginRedirect = await AuthenticateOrGoLogin()) != null ) return loginRedirect;

            transaction.Owner = CurrentUser;
            transaction = await _transactionsService.LogTransaction(transaction);

            return RedirectToPage(IndexModel.PageName);
        }
    }
}
