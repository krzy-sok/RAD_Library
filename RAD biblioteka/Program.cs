﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RAD_biblioteka.Data;
using RAD_biblioteka.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RAD_bibliotekaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RAD_bibliotekaContext") ?? throw new InvalidOperationException("Connection string 'RAD_bibliotekaContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
    ).AddCookie(options =>
    {
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
        options.SlidingExpiration = true;
    }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Librarian", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    seedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
