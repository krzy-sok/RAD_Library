using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using ReactLibrary.Server.Data;
using ReactLibrary.Server.Models;
using ReactLibrary.Server.Controllers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReactLibraryContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ReactLibraryContext"
    ) ?? throw new InvalidOperationException("Connection string 'ReactLibraryContext' not found."))
);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new FilePro(Path.Combine("..", "..", "..", "..", "ReactLibrary.client", "src")),
//    RequestPath = "/"
//});

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

//app.MapBookEndpoints();

app.Run();
