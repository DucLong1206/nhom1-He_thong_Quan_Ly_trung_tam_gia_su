using He_thong_Quan_Ly_trung_tam_gia_su.Application.Services;
using He_thong_Quan_Ly_trung_tam_gia_su.Data;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Email;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Repositories;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

builder.Services.AddDbContext<Appdbcontext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/Privacy";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

builder.Services.AddScoped<IMonhocLogic, MonhocLogic>();
builder.Services.AddScoped<ITaiKhoanLogic, TaiKhoanLogic>();
builder.Services.AddScoped<IDM_TinhLogic, DM_TinhLogic>();
builder.Services.AddScoped<IDM_XaLogic, DM_XaLogic>();
builder.Services.AddScoped<IUSERLogic, USERLogic>();
builder.Services.AddScoped<ILopHocLogic, LopHocLogic>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Appdbcontext>();
    db.Database.Migrate();
    DemoDataSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
