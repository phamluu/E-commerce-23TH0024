using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Extensions;
using E_commerce_23TH0024.Lib;
using Microsoft.AspNetCore.Identity.UI.Services;
using E_commerce_23TH0024.Service;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// add dbcontext để tạo các migration
builder.Services.AddDbContext<LocationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<SystemSettingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// end

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();




builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Cấu hình mật khẩu
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Các thiết lập khác nếu cần
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor(); // bổ sung
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

//if (builder.Environment.IsDevelopment())
//{
//    builder.Services
//        .AddControllersWithViews()
//        .AddRazorRuntimeCompilation();
//}

builder.Services.AddTransient<IEmailSender, EmailSender>();

// Đăng ký service
builder.Services.AddAppServices();
// Lưu key
builder.Services.ConfigureDataProtection(builder.Environment);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Nếu có tham số "seeddata" trong lệnh thì chạy seed và thoát
if (args.Contains("seeddata"))
{
    await app.UseIdentitySeedDataAsync();
    return;  // Dừng chương trình sau khi seed
}


// Gọi routes
app.UseEndpoints(endpoints =>
{
    endpoints.MapCustomRoutes();
});

app.MapRazorPages(); // Dùng gọi Page Identity mặc định
app.Run();
