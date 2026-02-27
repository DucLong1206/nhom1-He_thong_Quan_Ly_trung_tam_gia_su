using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Appdbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Appdbcontext"))
);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMonhocLogic, MonhocLogic>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();