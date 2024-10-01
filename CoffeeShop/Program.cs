using BusinessObjects.Services;
using BussinessObjects.AutoMapper;
using BussinessObjects.ImageService;
using BussinessObjects.Services;
using BussinessObjects.Utility;
using CoffeeShop.AutoMapper;
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
                    options.LoginPath = "/Shared/Login";
                    options.AccessDeniedPath = "/Shared/AccessDenied";
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

            // Add Firebase Uility
            builder.Services.Configure<FireBaseOptions>(builder.Configuration.GetSection("FireBase"));
            builder.Services.AddTransient(typeof(IImageService), typeof(ImageService));
            // Add Services
            builder.Services.AddScoped(typeof(ISizeService), typeof(SizeService));
            builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
            
           
            // Add Repositories
            builder.Services.AddScoped(typeof(ISizeRepository), typeof(SizeRepository));
            builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
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
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            // C?u h�nh middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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

            //Middleware to check user role?
            //app.Use(async (context, next) =>
            //{
            //    var userRole = context.Session.GetString("UserRole");
            //    var path = context.Request.Path.ToString().ToLower();
            //    if (path.StartsWith("/admin") && (userRole == null || userRole != "Admin"))
            //    {
            //        context.Response.Redirect("/AccessDenied");
            //        return;
            //    }

            //    if (path.StartsWith("/customer") && (userRole == null || userRole != "Customer"))
            //    {
            //        context.Response.Redirect("/AccessDenied");
            //        return;
            //    }

            //    await next.Invoke();
            //});

            app.UseRouting();

            //Using authentication
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
