using System;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class BudgetSquirrelException : Exception
    {
        public BudgetSquirrelException(string message)
            : base(message)
        {}
    }
}
