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
using Business.Services.Email;
using Business.Services.Actividades;
using Business.Services.Clases;

namespace Business
{
    public static class ServicesExtensions
    {
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<UsuarioServiceAsync>();
            services.AddScoped<TipoEventoServiceAsync>();
            services.AddScoped<ClasesServiceAsync>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IActiveDirectoryManager, ActiveDirectoryManager>();
            services.AddTransient<IApplicationUserStore, ApplicationUserStore>();
            services.AddTransient<IServiceEmail, EmailService>();

        }
    }
}
