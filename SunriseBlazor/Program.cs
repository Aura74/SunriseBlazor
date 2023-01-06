using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using SunriseBlazor.Models;
using SunriseBlazor.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<SunriseRepo>(); 
builder.Services.AddDbContext<SunriseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SunriseLocal")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
