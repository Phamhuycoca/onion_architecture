﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using onion_architecture.Infrastructure.Context;
using onion_architecture.Application.Module;
using onion_architecture.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using onion_architecture.Domain.Entity;
using System.Security.Cryptography;
using System.Text;
using Application.Helpers;
using onion_architecture.Api.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using onion_architecture.Infrastructure.Exceptions;
using Microsoft.OpenApi.Models;
using onion_architecture.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ValidationFilter));
}).AddFluentValidation();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationModules();


//Jwt
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Cấu hình bảo mật Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [Space] then your token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
            new string[]{}
        }
        });
});
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});













//ConnectStrings
builder.Services.AddDbContext<onion_architecture_Context>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("Base_Context")));
var app = builder.Build();

//seed data
using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<onion_architecture_Context>();

        await dbContext.Database.MigrateAsync();

        if (!dbContext.Users.Any())
        {
            DateTime now = DateTime.Now;
            string password = "12345678a";

            await dbContext.Users.AddAsync(
                new User
                {
                    createdAt = DateTime.Today.AddDays(1).AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second),
                    FullName = "Phạm Khắc Huy",
                    Email = "Phamkhachuy240702@gmail.com",
                    Role = "Admin",
                    Gender = "Nam",
                    PassWord = PasswordHelper.CreateHashedPassword(password),
                    Address = "Hải phòng",
                    Avatar = "https://res.cloudinary.com/drhdgw1xx/image/upload/v1709880283/m76gmmyuzzv3phiyuy2u.jpg",
                    PhoneNumber = "0325472224",
                    UserId = 0,
                    Is_Active = false,
                }
            );
            await dbContext.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw new ApiException(400, $"An error occurred: {ex.Message}");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCustomExceptionMiddleware();

app.UseAuthentication();
app.UseCors(builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
