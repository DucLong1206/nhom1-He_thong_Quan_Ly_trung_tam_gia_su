using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic;
using He_thong_Quan_Ly_trung_tam_gia_su.Data;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.BackgroundJobs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing in appsettings.json");

builder.Services.AddDbContext<Appdbcontext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMonhocLogic, MonhocLogic>();
builder.Services.AddScoped<ITaiKhoanLogic, TaiKhoanLogic>();
builder.Services.AddScoped<IDM_TinhLogic, DM_TinhLogic>();
builder.Services.AddScoped<IDM_XaLogic, DM_XaLogic>();
builder.Services.AddScoped<IUSERLogic, USERLogic>();
builder.Services.AddScoped<ILopHocLogic, LopHocLogic>();
builder.Services.AddScoped<IDM_NganHangLogic, DM_NganHangLogic>();
builder.Services.AddHostedService<LessonAlertBackgroundService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Appdbcontext>();
    db.Database.EnsureCreated();
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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
