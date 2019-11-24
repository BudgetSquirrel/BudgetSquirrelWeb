using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Ports.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Application.Auth
{
    public class UserService
    {
        private BudgetCreation _budgetCreator;
        private IUserRepository _userRepository;

        public UserService(BudgetCreation budgetCreator, IUserRepository userRepository)
        {
            _budgetCreator = budgetCreator;
            _userRepository = userRepository;
        }

        public async Task<(User, Budget)> RegisterUserAndRootBudget(CreateUserViewModel setupInput)
        {
            EnsurePasswordConfirmation(setupInput.Password, setupInput.ConfirmPassword);
            await EnsureUserUniqueness(setupInput.Username);
            User user = setupInput.GetUser();
            bool userCreated = await _userRepository.Register(user);
            User createdUser = await _userRepository.GetByUsername(user.Username);
            CreateBudgetRequestMessage budgetToCreate = setupInput.GetRootBudget();
            Budget rootBudget = await _budgetCreator.CreateBudgetForUser(budgetToCreate, createdUser);
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
