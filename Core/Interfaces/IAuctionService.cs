
using System.Collections.ObjectModel;

namespace Auktion.Core.Interfaces;

public interface IAuctionService
{
    Collection<Auction> GetAuctions();
    Auction GetAuctionById(int id);
    void CreateAuction(Auction auction);
    void UpdateAuction(int id, string description);
    void DeleteAuction(int id);
    List<Auction> GetUserBidsOnOngoingAuctions(string userId, string search = null);
    IEnumerable<Auction> GetWonAuctions(string userId);

}
