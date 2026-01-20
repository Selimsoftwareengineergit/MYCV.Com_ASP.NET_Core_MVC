using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Application.Services;
using MYCV.Infrastructure.Data;
using MYCV.Infrastructure.Repositories;
using MYCV.Infrastructure.Security;
using MYCV.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================
// 1️⃣ Configure DbContext
// ============================
builder.Services.AddDbContext<MyCvDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================
// 2️⃣ Configure Email Services
// ============================
builder.Services.AddSingleton<EmailTemplateService>(sp =>
    new EmailTemplateService(builder.Environment.ContentRootPath));
builder.Services.AddScoped<IEmailService, EmailService>();

// ============================
// 3️⃣ Configure Repositories
// ============================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCvRepository, UserCvRepository>();

// ============================
// 4️⃣ Configure Application Services
// ============================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCvService, UserCvService>();

// ============================
// 5️⃣ Configure Security Services
// ============================
builder.Services.AddScoped<ITokenService, TokenService>();

// ============================
// 6️⃣ Add Controllers, Swagger, CORS
// ============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy
            .WithOrigins("https://localhost:7167") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ============================
// 7️⃣ Configure Middleware
// ============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowWebApp");

app.UseAuthentication();   // ✅ if JWT/cookies added later
app.UseAuthorization();

app.MapControllers();

app.Run();
