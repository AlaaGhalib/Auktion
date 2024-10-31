using System.Collections.ObjectModel;

namespace Auktion.Core.Interfaces;

public interface IBidService
{
    Collection<Bid> GetBids();
    Bid GetBidById(int Id);
    void CreateBid(Bid Bid);
    void UpdateBid(Bid Bid);
    void DeleteBid(int id);
}