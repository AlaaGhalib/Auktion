using System.Collections.ObjectModel;
using Auktion.Core.Interfaces;
using Auktion.Persistence;

namespace Auktion.Core;

public class BidService : IBidService
{
    private readonly IGenericPersistence<BidDb,Bid> _bidPersistence;
    public BidService(IGenericPersistence<BidDb,Bid> bidPersistence)
    {
        _bidPersistence = bidPersistence;
    }

    public Collection<Bid> GetBids()
    {
        var sortedBids = _bidPersistence.Get()
            .OrderByDescending(b => b.Amount)
            .ToList();
        return new Collection<Bid>(sortedBids);
    }

    public Bid GetBidById(int id)
    {
        var bid = _bidPersistence.GetById(id);
        if (bid == null)
        {
            throw new KeyNotFoundException($"Bid with ID {id} not found.");
        }
        return bid;
    }

    public void CreateBid(Bid bid)
    {
        _bidPersistence.Create(bid);
    }

    public void UpdateBid(Bid bid)
    {
        _bidPersistence.Update(bid);
    }

    public void DeleteBid(int id)
    {
        _bidPersistence.Remove(id);
    }
}