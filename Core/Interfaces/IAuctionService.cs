namespace Auktion.Core.Interfaces;

public interface IAuctionService
{
    Task<IEnumerable<Auction>> GetAuctionsAsync();
    Task<Auction> GetAuctionByIdAsync(int id);
    Task CreateAuctionAsync(Auction auction);
    Task UpdateAuctionAsync(Auction auction);
    Task DeleteAuctionAsync(int id);
}
