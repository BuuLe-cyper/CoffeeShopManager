
using BussinessObjects.Services;
using DataAccess.DataContext;
using DataAccess.Repositories;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("CoffeeShop"),
                    sqlServerOptions => sqlServerOptions.MigrationsAssembly("DataAccess"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            //Add Services
            builder.Services.AddScoped<ITableService, TableService>();
            builder.Services.AddScoped<IMessService, MessService>();

            //Add Repositories
            builder.Services.AddScoped<ITableRepository, TableRepository>();
            builder.Services.AddScoped<IMessRepository, MessRepository>();

            builder.Services.AddControllers()
            .AddOData(options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100))
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
