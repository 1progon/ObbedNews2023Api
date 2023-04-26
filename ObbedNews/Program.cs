using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ObbedNews.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMemoryCache();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt")["Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("Jwt")["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt")["Key"] ?? ""))
        };
    });

builder.Services.AddNpgsql<AppDbContext>(
    builder.Configuration.GetConnectionString("AppDbContext"),
    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
);

builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

builder.Services.AddRouting(o =>
{
    o.LowercaseUrls = true;
    o.LowercaseQueryStrings = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(b
    => b
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins(builder.Configuration.GetValue<string>("Origin")));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();