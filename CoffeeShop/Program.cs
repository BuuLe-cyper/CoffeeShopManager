using BusinessObjects.Services;
using BussinessObjects.AutoMapper;
using BussinessObjects.ImageService;
using BussinessObjects.Services;
using CoffeeShop.AutoMapper;
using CoffeeShop.CoffeeShopHub;
using BussinessObjects.Utility;
using DataAccess.DataContext;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading.RateLimiting;
using DataAccess.Qr;

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
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

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
            // Configure FireBase
            builder.Services.Configure<FireBaseOptions>(builder.Configuration.GetSection("FireBase"));
            // Register MailService as a transient service
            builder.Services.AddTransient<MailService>();
            // Add Firebase Uility
            builder.Services.AddTransient(typeof(IImageService), typeof(ImageService));
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

            //Add Services
            builder.Services.AddScoped<ITableService, TableService>();
            builder.Services.AddScoped<IMessService, MessService>();
            builder.Services.AddScoped(typeof(ISizeService), typeof(SizeService));
            builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
            builder.Services.AddScoped(typeof(IProductSizesService), typeof(ProductSizesService));
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            builder.Services.AddScoped(typeof(IOrderDetailService), typeof(OrderDetailService));
            //Add Repositories
            builder.Services.AddScoped<ITableRepository, TableRepository>();
            builder.Services.AddScoped<IMessRepository, MessRepository>();
            builder.Services.AddScoped(typeof(ISizeRepository), typeof(SizeRepository));
            builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            builder.Services.AddScoped(typeof(IProductSizesRepository), typeof(ProductSizesRepository));
            builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            builder.Services.AddScoped(typeof(IOrderDetailRepository), typeof(OrderDetailRepository));
            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(MappingProfileView).Assembly);


            // Add QR Code
            builder.Services.AddScoped<GenerateQRCode>();

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




            app.UseRouting();

            //Using authentication
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                // Map Razor Pages
                endpoints.MapRazorPages();

                // Map SignalR hub
                endpoints.MapHub<ChatHub>("/chatHub");

                // Map controller routes for admin chat
                endpoints.MapControllerRoute(
                    name: "adminChat",
                    pattern: "Admin/Chats/Chat/{tableId:int}",
                    defaults: new { area = "Admin", page = "/Chat" });

                // Map controller routes for customer chat
                endpoints.MapControllerRoute(
                    name: "customerChat",
                    pattern: "Customer/Chats/Chat/{tableId:int}",
                    defaults: new { area = "Customer", page = "/Chat" });
            });


            app.Run();
        }
    }
}
