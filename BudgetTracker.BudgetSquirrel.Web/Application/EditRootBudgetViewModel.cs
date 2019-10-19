using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.BudgetPeriods;
using System;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class EditRootBudgetViewModel : EditBudgetViewModel
    {
        public const int DURATION_TYPE_BOOKENDED = 1;
        public const int DURATION_TYPE_DAYSPAN = 2;

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

        public EditRootBudgetViewModel() {}

        public EditRootBudgetViewModel(Budget budget)
            : base(budget)
        {
            bool isDaySpan = budget.Duration is MonthlyDaySpanDuration;
            if (isDaySpan)
                DurationType = DURATION_TYPE_DAYSPAN;
            else
                DurationType = DURATION_TYPE_BOOKENDED;

            if (DurationType == DURATION_TYPE_BOOKENDED)
            {
                StartDayOfMonth = ((MonthlyBookEndedDuration) budget.Duration).StartDayOfMonth;
                EndDayOfMonth = ((MonthlyBookEndedDuration) budget.Duration).EndDayOfMonth;
                RolloverStartDateOnSmallMonths = ((MonthlyBookEndedDuration) budget.Duration).RolloverStartDateOnSmallMonths;
                RolloverEndDateOnSmallMonths = ((MonthlyBookEndedDuration) budget.Duration).RolloverEndDateOnSmallMonths;
            }
            else
            {
                NumberDays = ((MonthlyDaySpanDuration) budget.Duration).NumberDays;
            }
        }

        public override void SetModifications(Budget toModify)
        {
            base.SetModifications(toModify);

            if (DurationType == DURATION_TYPE_BOOKENDED)
            {
                MonthlyBookEndedDuration updatedDuration = new MonthlyBookEndedDuration()
                {
                    Id = toModify.Duration.Id
                };
                updatedDuration.StartDayOfMonth = StartDayOfMonth;
                updatedDuration.EndDayOfMonth = EndDayOfMonth;
                updatedDuration.RolloverStartDateOnSmallMonths = RolloverStartDateOnSmallMonths;
                updatedDuration.RolloverEndDateOnSmallMonths = RolloverEndDateOnSmallMonths;

                toModify.Duration = updatedDuration;
            }
            else
            {
                MonthlyDaySpanDuration updatedDuration = new MonthlyDaySpanDuration()
                {
                    Id = toModify.Duration.Id
                };
                updatedDuration.NumberDays = NumberDays;

                toModify.Duration = updatedDuration;
            }
        }
    }
}
