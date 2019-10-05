namespace BudgetTracker.BudgetSquirrel.Application
{
    public class AppConstants
    {
        public const double BUDGET_STATUS_WARNING_THRESHOLD = 0.33;
        public const double BUDGET_STATUS_BAD_THRESHOLD = 0.05;
    }

    public enum BudgetStatus
    {
        Good = 2,
        Warning = 1,
        Bad = 0
    }
}
