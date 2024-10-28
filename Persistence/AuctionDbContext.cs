using Microsoft.EntityFrameworkCore;

public class AuctionContext : DbContext
{
    public AuctionContext(DbContextOptions<AuctionContext> options)
        : base(options)
    {
    }
    
    //public DbSet<Auction> Auctions { get; set; }
    //public DbSet<Bid> Bids { get; set; }
}