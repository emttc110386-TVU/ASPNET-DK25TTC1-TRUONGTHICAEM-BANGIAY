using Microsoft.EntityFrameworkCore;
using BangiayCAEM.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Đăng ký Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký Session cho Giỏ hàng
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Bật Session
app.UseSession();

app.UseAuthorization();

// 3. Tự động nạp ĐẦY ĐỦ 9 mẫu giày từ thư mục images vào CSDL
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Giày Thể Thao / Sneaker", Description = "Phong cách trẻ trung" },
                new Category { Name = "Giày Tây / Công Sở", Description = "Lịch lãm sang trọng" }
            );
            context.SaveChanges();
        }

        var catTheThao = context.Categories.First(c => c.Name.Contains("Thể Thao")).Id;
        var catCongSo = context.Categories.First(c => c.Name.Contains("Công Sở")).Id;

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Giày Sneaker Nike Air", Price = 1200000, ImageUrl = "/images/nike.jfif", CategoryId = catTheThao },
                new Product { Name = "Giày Nike Zoom Running", Price = 1850000, ImageUrl = "/images/nike (2).jfif", CategoryId = catTheThao },
                new Product { Name = "Giày Adidas Ultraboost", Price = 1500000, ImageUrl = "/images/adidas.jfif", CategoryId = catTheThao },
                new Product { Name = "Giày Sneaker Converse Classic", Price = 850000, ImageUrl = "/images/convers.jfif", CategoryId = catTheThao },
                new Product { Name = "Giày Thể Thao Vans Old Skool", Price = 950000, ImageUrl = "/images/vans.jfif", CategoryId = catTheThao },
                new Product { Name = "Giày Sneaker Đi Chơi Thời Trang", Price = 690000, ImageUrl = "/images/dichoi.jfif", CategoryId = catTheThao },
                new Product { Name = "Giày Tây Đi Làm Da Cao Cấp", Price = 1350000, ImageUrl = "/images/dilam.jfif", CategoryId = catCongSo },
                new Product { Name = "Giày Lười Loafer Công Sở", Price = 1100000, ImageUrl = "/images/dilam2.jfif", CategoryId = catCongSo },
                new Product { Name = "Giày Thể Thao Nam Nữ Cổ Thấp", Price = 750000, ImageUrl = "/images/giaythethao.jfif", CategoryId = catTheThao }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // Bỏ qua lỗi nếu trùng
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();