using System.Collections.ObjectModel;
using Auktion.Core.Interfaces;

namespace Auktion.Core;

public class BidService : IBidService
{
    private readonly IBidPersistence _persistence;

    public BidService(IBidPersistence persistence)
    {
        _persistence = persistence;
    }
    
    public Collection<Bid> GetBids()
    {
        var sortedBids = _persistence.GetBids()
            .OrderByDescending(b => b.Amount) 
            .ToList();
        return new Collection<Bid>(sortedBids);
    }

    public Bid GetBidById(int Id)
    {
        return _persistence.GetBidById(Id);
    }

    public void CreateBid(Bid Bid)
    {
        _persistence.CreateBid(Bid);
    }

    public void UpdateBid(Bid Bid)
    {
        _persistence.UpdateBid(Bid);
    }

    public void DeleteBid(int id)
    {
        _persistence.DeleteBid(id);
    }
}