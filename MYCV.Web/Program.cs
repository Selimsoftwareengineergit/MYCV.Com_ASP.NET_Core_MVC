using Microsoft.AspNetCore.Authentication.Cookies;
using MYCV.Web.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// 1️⃣ Add services to the container
// -------------------------------
builder.Services.AddControllersWithViews();

// -------------------------------
// 2️⃣ Configure API base URL
// -------------------------------
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl");

if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    throw new InvalidOperationException("API Base URL is not configured. Please set ApiSettings:BaseUrl in appsettings.json or environment variables.");
}

// -------------------------------
// 3️⃣ Register AuthApiService
// -------------------------------
builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(30);
});

// -------------------------------
// 4️⃣ Register CvApiService (Step 3)
// -------------------------------
builder.Services.AddHttpClient<ICvApiService, CvApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(30);
});

// -------------------------------
// 5️⃣ Add HttpContextAccessor
// -------------------------------
builder.Services.AddHttpContextAccessor();

// -------------------------------
// 6️⃣ Configure Cookie Authentication
// -------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// -------------------------------
// 7️⃣ Configure HTTP request pipeline
// -------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// -------------------------------
// 8️⃣ Map default route
// -------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
