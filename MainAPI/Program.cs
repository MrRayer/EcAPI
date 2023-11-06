using MainAPI.AuthorizationPolicies;
using MainAPI.Models;
using MainAPI.Utils.DBHandlers;
using MainAPI.Utils.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("UserAuthentication")
    .AddCookie("UserAuthentication", options =>
{
    options.Cookie.Name = "UserAuthentication";
    options.AccessDeniedPath = "/UserNotAuthorized";
    options.LoginPath = "/UserNotLogged";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoginRequired", policy =>
    {
        policy.Requirements.Add(new RolePolicy(0));
    });
    options.AddPolicy("AdminRequired", policy =>
    {
        policy.Requirements.Add(new RolePolicy(10));
    });
});
builder.Services.AddTransient<IAuthorizationHandler, RolePolicyHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
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
