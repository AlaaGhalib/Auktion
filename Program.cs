using Auktion.Areas.Identity.Data;
using Auktion.Core;
using Auktion.Core.Interfaces;
using Auktion.Data;
using Auktion.Mappers;
using Auktion.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllersWithViews();

// Register AuctionDbContext with MySQL configuration
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("AuctionConnection")));

// Register custom services and persistence layer for dependency injection
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IBidService, BidService>();

// Use GenericPersistence for both Auctions and Bids
builder.Services.AddScoped<IGenericPersistence<AuctionDb,Auction>, GenericPersistence<Auction, AuctionDb>>();
builder.Services.AddScoped<IGenericPersistence<BidDb,Bid>, GenericPersistence<Bid, BidDb>>();


// Register the Identity context and default identity configuration
builder.Services.AddDbContext<AuktionContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDefaultIdentity<AuktionUser>(options => 
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuktionContext>();

// Add AutoMapper profiles
builder.Services.AddAutoMapper(typeof(AuctionProfile));
builder.Services.AddAutoMapper(typeof(BidProfile));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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