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
using Business.Services.Usuarios;
using Infrastructure.Contexts;

namespace Business
{
    public static class ServicesExtensions
    {
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<UsuarioServiceAsync>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IActiveDirectoryManager, ActiveDirectoryManager>();
            services.AddTransient<IApplicationUserStore, ApplicationUserStore>();

        }
    }
}
