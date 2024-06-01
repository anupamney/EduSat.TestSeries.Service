using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EduSat.TestSeries.Service.Config;
using EduSat.TestSeries.Service.Data;
using EduSat.TestSeries.Service.Services.Interfaces;
using EduSat.TestSeries.Service.Services.Concrete;
using Edusat.TestSeries.Service.Domain.Models;
using EduSat.TestSeries.Service.Services;
using EduSat.TestSeries.Service.Provider;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      //policy =>
                      //{
                      //    policy.WithOrigins("http://localhost:3000");
                      //    policy.WithHeaders("Content-Type");
                      //});
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                      });
});


builder.Services.AddDbContext<ApiDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);

//JWT Config
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// Validation params
Byte[]? key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
TokenValidationParameters? tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ClockSkew = TimeSpan.Zero,
    ValidateAudience = false,
    ValidateLifetime = true,
    RequireExpirationTime = true
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParams;
});

builder.Services.AddSingleton(tokenValidationParams);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>();

builder.Services.AddScoped<IJwtService, JwtService>()
    .AddScoped<IUserContext,UserContext>()
    .AddScoped<ISchoolsService,SchoolsService>()
    .AddScoped<ISchoolsProvider,SchoolsProvider>()
    .AddScoped<IReportsProvider,ReportsProvider>()
    .AddScoped<IReportsService,ReportsService>()
    .AddScoped<INotificationService,NotificationService>()
    .AddScoped<IMessageService, EmailService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
