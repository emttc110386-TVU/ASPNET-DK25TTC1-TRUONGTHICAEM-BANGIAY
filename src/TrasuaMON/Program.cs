using Microsoft.EntityFrameworkCore;
using TrasuaMON.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

// ==========================================
// TỰ ĐỘNG NẠP DỮ LIỆU SẢN PHẨM KHI CHẠY WEB
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    if (!context.Categories.Any())
    {
        context.Categories.Add(new Category { Name = "Trà Sữa MONMON", Description = "Danh mục chính" });
        context.SaveChanges();
    }

    var categoryId = context.Categories.First().Id;

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { Name = "Trà Sữa MATCHA", Price = 45000, ImageUrl = "/images/trasua_matcha.png", Description = "cực ngon và béo ngậy", CategoryId = categoryId },
            new Product { Name = "Trà sữa chocolate", Price = 40000, ImageUrl = "/images/chocolate.png", Description = "Vị ngon khó cưỡng", CategoryId = categoryId },
            new Product { Name = "Trà sữa truyền thống", Price = 46000, ImageUrl = "/images/truyenthong.avif", Description = "thơm ngon", CategoryId = categoryId },
            new Product { Name = "Full Topping", Price = 55000, ImageUrl = "/images/2.jpg", Description = "Vị ngon khó cưỡng", CategoryId = categoryId },
            new Product { Name = "Cam Đá Xay", Price = 60000, ImageUrl = "/images/321.jpg", Description = "Uống vào là ghiền", CategoryId = categoryId },
            new Product { Name = "Hoa Hướng Dương", Price = 60000, ImageUrl = "/images/xam.jpg", Description = "Thơm ngon béo ngậy", CategoryId = categoryId },
            new Product { Name = "Sữa Tươi Cà Phê", Price = 45000, ImageUrl = "/images/3.jpg", Description = "Rất thơm mát", CategoryId = categoryId },
            new Product { Name = "Sữa Tươi Trân Châu Đường", Price = 50000, ImageUrl = "/images/1.jpg", Description = "Ngọt ngào mê say", CategoryId = categoryId }
        );
        context.SaveChanges();
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();