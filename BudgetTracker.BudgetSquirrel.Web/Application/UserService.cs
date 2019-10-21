using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Ports.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Application
{
    public class UserService
    {
        private IUserRepository _userRepository;
        public IBudgetRepository _budgetRepository;

        public UserService(IUserRepository userRepository,
            IBudgetRepository budgetRepository)
        {
            _userRepository = userRepository;
            _budgetRepository = budgetRepository;
        }

        public async Task<(User, Budget)> RegisterUserAndRootBudget(CreateUserViewModel setupInput)
        {
            EnsurePasswordConfirmation(setupInput.Password, setupInput.ConfirmPassword);
            await EnsureUserUniqueness(setupInput.Username);
            User user = setupInput.GetUser();
            bool userCreated = await _userRepository.Register(user);
            User createdUser = await _userRepository.GetByUsername(user.Username);
            Budget budgetToCreate = setupInput.GetRootBudget();
            Budget rootBudget = await BudgetCreation.CreateBudgetForUser(budgetToCreate, createdUser, _budgetRepository);
            return (createdUser, rootBudget);
        }

        private async Task EnsureUserUniqueness(string username)
        {
            if (await _userRepository.GetByUsername(username) != null)
            {
                throw new BudgetSquirrelException("A user with that username already exists");
            }
        }

        private void EnsurePasswordConfirmation(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                throw new BudgetSquirrelException("The password and confirmation password must match");
            }
        }
    }
}
