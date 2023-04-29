using ExpressKuryer.Application;
using ExpressKuryer.Application.Enums;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Infrastructure;
using ExpressKuryer.Persistence;
using ExpressKuryer.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(StorageEnum.CloudinaryStorage);

//builder.Services.AddIdentity<AppUser, IdentityRole>(x =>
//{
//    x.Password.RequiredLength = 5;
//    x.Password.RequireNonAlphanumeric = false;
//    x.Password.RequireUppercase = false;
//    x.Password.RequireLowercase = false;
//}).AddDefaultTokenProviders().AddEntityFrameworkStores<DataContext>();

var app = builder.Build();

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
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();