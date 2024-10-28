
using System.Collections.ObjectModel;

namespace Auktion.Core.Interfaces;

public interface IAuctionService
{
    Collection<Auction> GetAuctions();
    Auction GetAuctionById(int id);
    void CreateAuction(Auction auction);
    void UpdateAuction(Auction auction);
    void DeleteAuction(int id);
}
