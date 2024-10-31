using Microsoft.EntityFrameworkCore;

namespace Auktion.Persistence;

public class AuctionDbContext : DbContext
{
   
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
    }
    
    public DbSet<AuctionDb> Auctions { get; set; }
    public DbSet<BidDb> Bids { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AuctionDb Adb = new AuctionDb
        {
            AuctionId = -1,
            Title = "Test Auction",
            Description = "Test Description",
            StartingPrice = 10,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            OwnerId = "1",
            Bids = new List<BidDb>()
        };
        BidDb Bdb = new BidDb
        {
            BidId = -1,
            Amount = 10,
            Time = DateTime.Now,
            UserId = "2",
            AuctionId = -1
        };
    }
}