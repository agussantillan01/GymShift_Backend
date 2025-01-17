using Business;
using Domain.Interface;
using Domain.Settings;
using Infrastructure.CustomIdentity.Interface;
using Infrastructure.CustomIdentity;
using Infrastructure.DBConnection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Business.Interfaces;
using Business.Services;
using Microsoft.Extensions.Logging;
using Infrastructure;
using Microsoft.Extensions.Options;
using Infrastructure.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Domain.Wrappers;
using Newtonsoft.Json;
using System.Text;
using GymShift.Middlewares;
using GymShift.Services;
using Infrastructure.Contexts;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
//probar si funciona
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddBusinessLayer();

builder.Services.AddSingleton<JWTSettings>(); // O el ciclo de vida adecuado para tu aplicaci�n

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
//builder.Services.AddIdentity<UsuarioLogin, IdentityRole<int>>() // Aseg�rate de usar el tipo correcto para UsuarioLogin
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite cualquier origen (frontend)
              .AllowAnyMethod()  // Permite cualquier m�todo HTTP (GET, POST, etc.)
              .AllowAnyHeader(); // Permite cualquier encabezado
    });
});

builder.Services.AddIdentity<UsuarioLogin, Grupo>(x =>
{
    x.Password.RequireDigit = false;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
    x.SignIn.RequireConfirmedAccount = false;
    x.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<ApplicationUserStore>()
            //.AddUserManager<ApplicationUserManager>()
            //.AddRoleStore<ApplicationRoleStore>()
            .AddDefaultTokenProviders();

builder.Services.AddTransient<IActiveDirectoryManager, ActiveDirectoryManager>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

//  MULTIEMPRESA: VER QUE LA CONNECTION STRING DEBER� SER DIN�MICA CUANDO SE DEBA INCORPORAR LA FUNCIONALIDAD MULTIEMPRESA EN EL VISUALIZADOR
//  POR LO QUE EL C�DIGO CAMBIAR� A FUTURO.
builder.Services.AddTransient<IConexion>(c => new SQLDBConnection("Password=sadtjat;Persist Security Info=False;User ID=smsbrowse;Initial Catalog=XM_APDES;Data Source=SMS036DESA\\MSSQLSERVER2014;MultipleActiveResultSets=true;TrustServerCertificate=true"));

//builder.Services.AddTransient<IPasswordHasher<UsuarioLogin>, CustomPasswordHasher>();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});
//builder.Services.AddAuthentication().AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        ValidateAudience = false,
//        ValidateIssuer = false,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                builder.Configuration.GetSection("appsettings:Token").Value!))
//    };
//});
builder.Services.AddSwaggerGen(c =>
{
    //c.IncludeXmlComments(string.Format(@"{0}\ERP.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
    //c.SwaggerDoc("v1", new OpenApiInfo
    //{
    //    Version = "v1",
    //    Title = "Clean Architecture - ERP.WebApi",
    //    Description = "This Api will be responsible for overall data distribution and authorization.",
    //    Contact = new OpenApiContact
    //    {
    //        Name = "Softlatam",
    //        Email = "mfryda@softlatam.com",
    //        Url = new Uri("https://softlatam.com.ar"),
    //    }
    //});
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
                        ValidAudience = builder.Configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                                return Task.CompletedTask;
                            }

                            context.NoResult();
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "text/plain";
                            return context.Response.WriteAsync(context.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PermissionPolicy", policy =>
    {
        policy.Requirements.Add(new PermissionRequirement()); // Agrega tus requerimientos de permisos aqu�
        policy.RequireAuthenticatedUser(); // Requiere que el usuario est� autenticado
    });
});
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
        c.RoutePrefix = string.Empty; // Esto hace que Swagger est� disponible en la ra�z de la aplicaci�n
    });

}

app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
