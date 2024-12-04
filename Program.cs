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
builder.Services.AddRazorPages(); // Register Razor Pages

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
builder.Services.AddIdentity<AuktionUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddEntityFrameworkStores<AuktionContext>()
    .AddDefaultUI()  
    .AddDefaultTokenProviders();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Administrator").RequireClaim("AdminClaim", "True"));

builder.Services.AddAutoMapper(typeof(AuctionProfile));
builder.Services.AddAutoMapper(typeof(BidProfile));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await RoleInitializer.InitializeRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing roles and admin: {ex.Message}");
    }
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();