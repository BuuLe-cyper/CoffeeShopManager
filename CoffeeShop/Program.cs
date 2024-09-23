using BussinessObjects.AutoMapper;
using CoffeeShop.AutoMapper;
using DataAccess.DataContext;
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

            // Add services to the container.
            builder.Services.AddRazorPages();

            //Add Services

            //Add Repositories

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
