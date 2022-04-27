using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyProjectServer.Models;
using MyProjectServer.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyProjectContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(@"MyProjectServer\MyProjectServer\Views\Shared\Error.cshtml");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MyProject}/{action=Read}/");

app.Run();
