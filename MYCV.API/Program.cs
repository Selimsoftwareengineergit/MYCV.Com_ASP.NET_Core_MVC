using Microsoft.EntityFrameworkCore;
using MYCV.Application.Interfaces;
using MYCV.Application.Services;
using MYCV.Infrastructure.Data;
using MYCV.Infrastructure.Repositories;
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

// ============================
// 4️⃣ Configure Application Services
// ============================
builder.Services.AddScoped<IUserService, UserService>();

// ============================
// 5️⃣ Add Controllers, Swagger, CORS
// ============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", builder =>
    {
        builder
            .WithOrigins("https://localhost:7167") // Your Web app origin
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ============================
// 6️⃣ Configure Middleware
// ============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Enable CORS BEFORE Authorization & Controllers
app.UseCors("AllowWebApp");

app.UseAuthorization();

app.MapControllers();

app.Run();