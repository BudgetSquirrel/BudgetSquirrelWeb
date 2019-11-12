using BudgetTracker.Business.Auth;
using BudgetTracker.Business.Ports.Repositories;
using GateKeeper.Configuration;
using GateKeeper.Cryptogrophy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private ICryptor _cryptor;
        GateKeeperConfig _gateKeeperConfig;
        
        protected HttpContext HttpContext { get { return _httpContextAccessor.HttpContext; } }

        public LoginService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepo,
            IConfiguration appConfig)
        {
            _gateKeeperConfig = ConfigurationReader.FromAppConfiguration(appConfig);

            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepo;
            _cryptor = new Rfc2898Encryptor();
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
            string encryptedPasswordGuess = _cryptor.Encrypt(password, _gateKeeperConfig.EncryptionKey, _gateKeeperConfig.Salt);
            if (encryptedPasswordGuess != user.Password)
                return null;
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
