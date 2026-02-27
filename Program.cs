
using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Entity Framework với SQL Server
builder.Services.AddDbContext<Appdbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Appdbcontext"));
});

// Add Logic
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IMonhocLogic, MonhocLogic>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
