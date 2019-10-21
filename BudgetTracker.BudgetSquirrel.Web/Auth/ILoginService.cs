using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports.Repositories;

using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Web.Auth
{
    public interface ILoginService
    {
        Task<User> GetUser();

        Task<bool> IsAuthenticated();

        Task<User> Login(string username, string password);

        Task Login(User user);

        Task Logout();
    }
}
