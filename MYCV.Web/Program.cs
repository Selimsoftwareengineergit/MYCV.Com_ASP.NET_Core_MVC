using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using MYCV.Web.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configure HTTPClient for API calls
builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5001");
    client.DefaultRequestHeaders.Add("Accept","application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

//Register other service
builder.Services.AddScoped<IAuthApiService, AuthApiService>();

//Add HttpContextAccessor (needed by AuthApiService)
builder.Services.AddHttpContextAccessor();

//Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
{
    Options.LoginPath = "/Account/Login";
    Options.AccessDeniedPath = "/Account/AccessDenied";
    Options.ExpireTimeSpan = TimeSpan.FromHours(8);
    Options.SlidingExpiration = true;

    //Security settings
    Options.Cookie.HttpOnly = true;
    Options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    Options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
