using BusinessObjects.Services;
using BussinessObjects.AutoMapper;
using BussinessObjects.ImageService;
using BussinessObjects.Services;
using BussinessObjects.Utility;
using CoffeeShop.AutoMapper;
using DataAccess.DataContext;
using DataAccess.Qr;
using DataAccess.Repositories;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure the builder to use the custom appsettings filename
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = Directory.GetCurrentDirectory(),
                ApplicationName = typeof(Program).Assembly.FullName,
            });

            // Add custom configuration sources
            builder.Configuration.AddJsonFile("api.appsettings.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddJsonFile($"api.appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

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
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("SmtpSettings"));

            builder.Services.Configure<FireBaseOptions>(builder.Configuration.GetSection("FireBase"));

            builder.Services.AddTransient<MailService>();

            builder.Services.AddTransient(typeof(IImageService), typeof(ImageService));
            builder.Services.AddHttpClient();

            builder.Services.AddHttpContextAccessor();

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