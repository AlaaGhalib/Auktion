using Auktion.Core.Interfaces;

namespace Auktion.Core;

public class AuctionService : IAuctionService
{
    private readonly AuctionContext _context;

    public AuctionService(AuctionContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Auction>> GetAuctionsAsync()
    {
        return await _context.Auctions.Include(a => a.Bids).ToListAsync();
    }

    public async Task<Auction> GetAuctionByIdAsync(int id)
    {
        return await _context.Auctions.Include(a => a.Bids).FirstOrDefaultAsync(a => a.AuctionId == id);
    }

    public async Task CreateAuctionAsync(Auction auction)
    {
        _context.Auctions.Add(auction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAuctionAsync(Auction auction)
    {
        _context.Auctions.Update(auction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAuctionAsync(int id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction != null)
        {
            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
        }
    }
}
