using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    public static class DbInitializer
    {
        // Initialize data if not exists
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Initialize Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { CategoryName = "Beverages", IsActive = true },
                    new Category { CategoryName = "Snacks", IsActive = true },
                    new Category { CategoryName = "Meals", IsActive = true },
                    new Category { CategoryName = "Desserts", IsActive = true },
                    new Category { CategoryName = "Hot Drinks", IsActive = true },
                    new Category { CategoryName = "Cold Drinks", IsActive = true }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
            //if (!context.Tables.Any())
            //{
            //    var tables = new List<Table>
            //    {
            //        new Table { Description = "Table 1", QRCodeForOrderAndPay = "qr_code_1.png", QRCodeForMessaging = "msg_code_1.png" },
            //        new Table { Description = "Table 2", QRCodeForOrderAndPay = "qr_code_2.png", QRCodeForMessaging = "msg_code_2.png" },
            //        new Table { Description = "Table 3", QRCodeForOrderAndPay = "qr_code_3.png", QRCodeForMessaging = "msg_code_3.png" },
            //    };
            //    context.Tables.AddRange(tables);
            //    context.SaveChanges();
            //}
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { Username = "staff1", FullName = "Staff One", PhoneNumber = "1234567890", Password = "password1", AccountType = 1, Email = "admin@gmail.com" }, // Staff
                    new User { Username = "customer1", FullName = "Customer One", PhoneNumber = "0987654321", Password = "password2", AccountType = 0, Email = "buule.210303@gmail.com" }, // Customer
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { ProductName = "Latte", CategoryID = context.Categories.First(c => c.CategoryName == "Hot Drinks").CategoryID, Discount = 0.1f, IsAvailable = true, ProductDescription = "Hot coffee with milk", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQSNw7oOeCCCIv_D1ok78I61XqgUl1ZU_S2DA&s" },
                    new Product { ProductName = "Americano", CategoryID = context.Categories.First(c => c.CategoryName == "Hot Drinks").CategoryID, Discount = 0.05f, IsAvailable = true, ProductDescription = "Black coffee", ImageUrl = "https://cdn2.fptshop.com.vn/unsafe/1920x0/filters:quality(100)/2023_10_7_638322743867084776_americano-00.jpeg" },
                    new Product { ProductName = "Iced Coffee", CategoryID = context.Categories.First(c => c.CategoryName == "Cold Drinks").CategoryID, Discount = 0.1f, IsAvailable = true, ProductDescription = "Cold brewed coffee with ice", ImageUrl = "https://www.allrecipes.com/thmb/Hqro0FNdnDEwDjrEoxhMfKdWfOY=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/21667-easy-iced-coffee-ddmfs-4x3-0093-7becf3932bd64ed7b594d46c02d0889f.jpg" },
                    new Product { ProductName = "Cappuccino", CategoryID = context.Categories.First(c => c.CategoryName == "Hot Drinks").CategoryID, Discount = 0.15f, IsAvailable = true, ProductDescription = "Coffee with steamed milk", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c8/Cappuccino_at_Sightglass_Coffee.jpg" },
                    new Product { ProductName = "Chocolate Cake", CategoryID = context.Categories.First(c => c.CategoryName == "Desserts").CategoryID, Discount = 0.2f, IsAvailable = true, ProductDescription = "Rich chocolate cake", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSHF_4I3r7kvHwf5kYCGsM0gE-0GFVBeFCW4A&s" },
                    new Product { ProductName = "Cheesecake", CategoryID = context.Categories.First(c => c.CategoryName == "Desserts").CategoryID, Discount = 0.2f, IsAvailable = true, ProductDescription = "Classic cheesecake", ImageUrl = "https://cakesbymk.com/wp-content/uploads/2023/11/Template-Size-for-Blog-Photos-24.jpg" },
                    new Product { ProductName = "Club Sandwich", CategoryID = context.Categories.First(c => c.CategoryName == "Meals").CategoryID, Discount = 0.25f, IsAvailable = true, ProductDescription = "Triple layer sandwich with chicken, bacon, and veggies", ImageUrl = "https://img.taste.com.au/OX2Wle32/taste/2016/11/marinated-chicken-club-sandwich-1970-1.jpeg" },
                    new Product { ProductName = "French Fries", CategoryID = context.Categories.First(c => c.CategoryName == "Snacks").CategoryID, Discount = 0.05f, IsAvailable = true, ProductDescription = "Crispy golden fries", ImageUrl = "https://www.recipetineats.com/tachyon/2022/09/Fries-with-rosemary-salt_1.jpg" },
                    new Product { ProductName = "Smoothie", CategoryID = context.Categories.First(c => c.CategoryName == "Cold Drinks").CategoryID, Discount = 0.1f, IsAvailable = true, ProductDescription = "Fresh fruit smoothie", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSFXqYRxCyN6l5-56vMYgVCy9WsafkOjqeAGQ&s" },
                    new Product { ProductName = "Burger", CategoryID = context.Categories.First(c => c.CategoryName == "Meals").CategoryID, Discount = 0.2f, IsAvailable = true, ProductDescription = "Beef burger with fries", ImageUrl = "https://www.allrecipes.com/thmb/5JVfA7MxfTUPfRerQMdF-nGKsLY=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/25473-the-perfect-basic-burger-DDMFS-4x3-56eaba3833fd4a26a82755bcd0be0c54.jpg" },
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
            if (!context.Sizes.Any())
            {
                var sizes = new List<Size>
                {
                    new Size { SizeName = "S" },
                    new Size { SizeName = "M" },
                    new Size { SizeName = "L" }
                };
                context.Sizes.AddRange(sizes);
                context.SaveChanges();
            }
            if (!context.ProductSizes.Any())
            {
                var productSizes = new List<ProductSize>
                {
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Latte").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "S").SizeID, Price = 4.99m, OriginalPrice = 5.50m },
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Latte").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "M").SizeID, Price = 5.99m, OriginalPrice = 6.50m },
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Latte").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "L").SizeID, Price = 6.99m, OriginalPrice = 7.50m },

                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Americano").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "S").SizeID, Price = 3.99m, OriginalPrice = 4.50m },
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Americano").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "M").SizeID, Price = 4.49m, OriginalPrice = 5.00m },
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Americano").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "L").SizeID, Price = 4.99m, OriginalPrice = 5.50m },

                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Cappuccino").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "S").SizeID, Price = 4.99m, OriginalPrice = 5.50m },
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Cappuccino").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "M").SizeID, Price = 5.49m, OriginalPrice = 6.00m },
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Cappuccino").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "L").SizeID, Price = 5.99m, OriginalPrice = 6.50m },

                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Cheesecake").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "M").SizeID, Price = 6.99m, OriginalPrice = 8.00m },
                    new ProductSize { ProductID = context.Products.First(p => p.ProductName == "Burger").ProductID, SizeID = context.Sizes.First(s => s.SizeName == "M").SizeID, Price = 9.99m, OriginalPrice = 11.00m }
                };
                context.ProductSizes.AddRange(productSizes);
                context.SaveChanges();
            }
        }
    }
}
