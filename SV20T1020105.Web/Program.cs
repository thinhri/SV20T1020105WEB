using Microsoft.AspNetCore.Authentication.Cookies;
using SV20T1020105.Web;

var builder = WebApplication.CreateBuilder(args);

///Add nhom cac Services can dung trong Application
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddMvcOptions(option =>
    {
        option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(option =>
				{
					option.Cookie.Name = "AuthenticationCookie";
					option.LoginPath = "/Account/Login";
					option.AccessDeniedPath = "/Account/AccessDenined";
					option.ExpireTimeSpan = TimeSpan.FromMinutes(60);//Thoi gian cua 1 phien dang nhap
				});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
// Add services to the container.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

///Cau hinh ung dung Web
ApplicationContext.Configure
(
    httpContextAccessor: app.Services.GetRequiredService<IHttpContextAccessor>(),
    hostEnvironment: app.Services.GetService<IWebHostEnvironment>()
);
string connectionString = "server=LAPTOP-02LRH71J;user id=sa;password=123;database=LiteCommerceDB;TrustServerCertificate=true";
SV20T1020105.BusinessLayers.Configuration.Initialize(connectionString);

app.Run();
