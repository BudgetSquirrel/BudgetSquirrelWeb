using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports.Repositories;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BudgetTracker.BudgetSquirrel.Web.Auth
{
    public class LoginService : ILoginService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IUserRepository _userRepository;

        protected HttpContext HttpContext { get { return _httpContextAccessor.HttpContext; } }

        public LoginService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepo;
        }

        public async Task<User> GetUser()
        {
            if (!(await IsAuthenticated())) return null;

            string username = HttpContext.User.Identity.Name;
            return await _userRepository.GetByUsername(username);
        }

        public Task<bool> IsAuthenticated() => Task.FromResult(HttpContext.User.Identity.IsAuthenticated);

        public async Task<User> Authenticate(string username, string password)
        {
            User user = await _userRepository.GetByUsername(username);
            // TODO: Check password
            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            if ((await IsAuthenticated())) return await GetUser();

            User user = await Authenticate(username, password);
            if (user == null) return null;

            await Login(user);
            return user;
        }

        /// <summary>
        /// Logs the user in. This does not check password correctness as it
        /// assumes that the user has already been authenticated.
        /// </summary>
        public async Task Login(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);
        }

        public Task Logout()
        {
            return HttpContext.SignOutAsync();
        }
    }
}
