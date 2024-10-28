using Auktion.Areas.Identity.Data;
using Auktion.Core;
using Auktion.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Auktion.Data;
using Auktion.Mappers;
using Auktion.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("AuctionConnection")));

builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IAuctionPersistence, AuctionPersistence>();

builder.Services.AddDbContext<AuktionContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddDefaultIdentity<AuktionUser>(
        options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuktionContext>();

builder.Services.AddAutoMapper(typeof(AuctionProfile));
builder.Services.AddAutoMapper(typeof(BidProfile));

var app = builder.Build();

/*builder.Services.AddDbContext<>(
    options => options.UseMySQL (
    builder.configuration.getConnectionString(
    "AuktionDbConnection")))*/
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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();