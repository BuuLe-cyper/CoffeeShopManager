using BusinessObjects.Services;
using BussinessObjects.AutoMapper;
using BussinessObjects.Services;
using DataAccess.DataContext;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CoffeeShop.AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            // Add SignalR
            builder.Services.AddSignalR();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("CoffeeShop"),
                    sqlServerOptions => sqlServerOptions.MigrationsAssembly("DataAccess"));
            });
            //Register User repository and service
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            // Register MailSettings by binding to the configuration section "SmtpSettings"
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("SmtpSettings"));

            // Register MailService as a transient service
            builder.Services.AddTransient<MailService>();

            //Register and Authorization and Cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/Privacy";
                });
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });
            // Add services to the container.
            builder.Services.AddRazorPages();

            //Add Services
            builder.Services.AddScoped<ISizeService, SizeService>();
            //Add Repositories
            builder.Services.AddScoped<ISizeRepository, SizeRepository>();
            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(MappingProfileView).Assembly);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                DbInitializer.Initialize(context);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //Add Session
            app.UseSession();


            app.Use(async (context, next) =>
            {
                var userRole = context.Session.GetString("UserRole");
                var path = context.Request.Path.ToString().ToLower();
                if (path.StartsWith("/admin") && (userRole == null || userRole != "Admin"))
                {
                    context.Response.Redirect("/AccessDenied");
                    return;
                }

                if (path.StartsWith("/customer") && (userRole == null || userRole != "Customer"))
                {
                    context.Response.Redirect("/AccessDenied");
                    return;
                }

                await next.Invoke();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
