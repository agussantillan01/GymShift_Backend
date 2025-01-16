using Business.Interfaces;
using Business.Services;
using Infrastructure.CustomIdentity.Interface;
using Infrastructure.CustomIdentity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class ServicesExtensions
    {
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();

            //services.AddScoped<IUserRoleService, UserRoleService>

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IActiveDirectoryManager, ActiveDirectoryManager>();
            services.AddTransient<IApplicationUserStore, ApplicationUserStore>();

        }
    }
}
