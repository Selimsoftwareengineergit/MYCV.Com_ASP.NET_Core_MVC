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
// EmailTemplateService needs the project root path, so register as Singleton
builder.Services.AddSingleton<EmailTemplateService>(sp =>
    new EmailTemplateService(builder.Environment.ContentRootPath));

// EmailService depends on EmailTemplateService, registered as Scoped
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
// 5️⃣ Add Controllers & Swagger
// ============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================
// 6️⃣ Build App
// ============================
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

app.UseAuthorization();

app.MapControllers();

// ============================
// 8️⃣ Run App
// ============================
app.Run();