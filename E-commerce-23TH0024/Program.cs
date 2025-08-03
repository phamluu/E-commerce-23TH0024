using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Extensions;
using E_commerce_23TH0024.Hubs;
using E_commerce_23TH0024.Lib;
using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkManagement.Data;
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
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// từ reference "WorkManagement"

builder.Services.AddDbContext<WorkManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<WorkDbContext>(provider =>
    provider.GetRequiredService<WorkManagementDbContext>());

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

// Tạm thời biên dịch Views 
builder.Services
        .AddControllersWithViews()
        .AddRazorRuntimeCompilation();

builder.Services.AddTransient<IEmailSender, EmailSender>();

// Đăng ký service
builder.Services.AddAppServices();
// Lưu key
builder.Services.ConfigureDataProtection(builder.Environment);
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";        // khi chưa đăng nhập
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // khi không đủ quyền
});

builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<TaskHub>("/taskHub");
// Thêm middleware phục vụ file tĩnh
app.UseStaticFiles();
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
