using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MYCV.Application.Interfaces;
using MYCV.Application.Services;
using MYCV.Infrastructure.Data;
using MYCV.Infrastructure.Repositories;
using MYCV.Infrastructure.Security;
using MYCV.Infrastructure.Services;
using System.Text;

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
// 5️⃣ Configure File Service
// ============================
builder.Services.AddScoped<IFileService, FileService>();

// ============================
// 6️⃣ Configure Security Services
// ============================
builder.Services.AddScoped<ITokenService, TokenService>();

// ============================
// 7️⃣ Add Controllers, Swagger, CORS
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

// ============================
// 8️⃣ Configure JWT Authentication
// ============================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

// ============================
// 9️⃣ Build App
// ============================
var app = builder.Build();

// ============================
// 🔟 Configure Middleware
// ============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowWebApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();