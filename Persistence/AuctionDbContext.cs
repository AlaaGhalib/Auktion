using Microsoft.EntityFrameworkCore;

namespace Auktion.Persistence;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<AuctionDb> Auctions { get; set; }
    public DbSet<BidDb> Bids { get; set; }
}