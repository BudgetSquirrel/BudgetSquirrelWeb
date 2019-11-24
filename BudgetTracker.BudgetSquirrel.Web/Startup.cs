using BudgetTracker.BudgetSquirrel.Application.Auth;
using BudgetTracker.BudgetSquirrel.Application.Budgeting;
using BudgetTracker.BudgetSquirrel.Application.Transactions;
using BudgetTracker.BudgetSquirrel.Data;
using BudgetTracker.BudgetSquirrel.Web.Auth;
using BudgetTracker.Business.BudgetPeriods;
using BudgetTracker.Business.Ports.Repositories;
using BudgetTracker.Data.EntityFramework;
using BudgetTracker.Data.EntityFramework.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.Converters.BudgetConverters;

namespace BudgetTracker.BudgetSquirrel.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            ConfigureBudgetTrackerBusiness(services);
            ConfigureAdapters(services);
            ConfigureAuth(services);

            services.AddTransient<BudgetService>();
            services.AddTransient<TransactionService>();
            services.AddTransient<UserService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpContextAccessor();
        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {

            });
            services.AddTransient<ILoginService, LoginService>();
        }

        public virtual void ConfigureAdapters(IServiceCollection services)
        {
            services.AddDbContext<BudgetTrackerContext, AppDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("Default"));
            });

            services.AddTransient<IBudgetRepository, BudgetRepository>();
            services.AddTransient<IBudgetPeriodRepository, BudgetPeriodRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public virtual void ConfigureBudgetTrackerBusiness(IServiceCollection services)
        {
            services.AddTransient<BudgetPeriodCalculator>();
            services.AddTransient<BudgetCreation>();
            services.AddTransient<BudgetValidator>();
            services.AddTransient<BudgetMessageConverter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
