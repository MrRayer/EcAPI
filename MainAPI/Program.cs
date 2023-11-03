using MainAPI.Models;
using MainAPI.Utils.DBHandlers;
using MainAPI.Utils.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("UserAuthentication").AddCookie("UserAuthentication", options =>
{
    options.Cookie.Name = "UserAuthentication";
    options.AccessDeniedPath = "/Users/AccessDenied";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeLoged", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "client");
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ProductsHelper>();
builder.Services.AddTransient<UsersHelper>();
builder.Services.AddTransient<PurchasesHelper>();
builder.Services.AddScoped<ProductsHandler>();
builder.Services.AddScoped<UsersHandler>();
builder.Services.AddScoped<PurchasesHandler>();
builder.Services.AddScoped<ProductCache>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
