using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.BudgetPeriods;
using System;

namespace BudgetTracker.BudgetSquirrel.Application.Auth
{
    public class CreateUserViewModel
    {
        public const int DURATION_TYPE_BOOKENDED = 1;
        public const int DURATION_TYPE_DAYSPAN = 2;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

        public int DurationType { get; set; }

        #region Monthly Budget Fields
        public int StartDayOfMonth { get; set; }
        public int EndDayOfMonth { get; set; }
        public bool RolloverStartDateOnSmallMonths { get; set; }
        public bool RolloverEndDateOnSmallMonths { get; set; }
        #endregion

        #region Days Spanning Fields
        public int NumberDays { get; set; }
        #endregion

        public string RootBudgetName { get; set; }
        public double? PercentAmount { get; set; }
        public decimal? SetAmount { get; set; }
        public decimal InitialBalance { get; set; }
        public DateTime RootBudgetStart { get; set; }

        public CreateUserViewModel()
        {
            RootBudgetStart = DateTime.Now;
        }

        public User GetUser()
        {
            User user = new User()
            {
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Password = Password,
                Email = Email
            };
            return user;
        }

        public BudgetDurationBaseMessage GetBudgetDuration()
        {
            BudgetDurationBaseMessage duration;

            if (DurationType == DURATION_TYPE_BOOKENDED)
            {
                duration = new MonthlyBookEndedDurationMessage()
                {
                    StartDayOfMonth = StartDayOfMonth,
                    EndDayOfMonth = EndDayOfMonth,
                    RolloverStartDateOnSmallMonths = RolloverStartDateOnSmallMonths,
                    RolloverEndDateOnSmallMonths = RolloverEndDateOnSmallMonths,
                };
            }
            else
            {
                duration = new MonthlyDaySpanDurationMessage()
                {
                    NumberDays = NumberDays
                };
            }

            return duration;
        }

        public CreateBudgetRequestMessage GetRootBudget()
        {
            CreateBudgetRequestMessage budget = new CreateBudgetRequestMessage()
            {
                Name = RootBudgetName,
                PercentAmount = PercentAmount,
                SetAmount = SetAmount,
                StartingBalance = InitialBalance,
                BudgetStart = RootBudgetStart,
                Duration = GetBudgetDuration()
            };
            return budget;
        }
    }
}
