using System.Collections.ObjectModel;
using Auktion.Core.Interfaces;
using System.Linq;
using Auktion.Persistence;

namespace Auktion.Core;

public class AuctionService : IAuctionService
{
    private readonly IGenericPersistence<AuctionDb,Auction> _auctionPersistence;

    public AuctionService(IGenericPersistence<AuctionDb,Auction> auctionPersistence)
    {
        _auctionPersistence = auctionPersistence;
    }
    public Collection<Auction> GetAuctions()
    {
        var currentDateTime = DateTime.Now;
        var ongoingAuctions = _auctionPersistence
            .Get(a => a.EndTime > currentDateTime, a => a.Bids) 
            .OrderBy(a => a.EndTime)
            .ToList();
        foreach (var auction in ongoingAuctions)
        {
            auction.Bids = auction.Bids?.OrderByDescending(b => b.Amount).ToList() ?? new List<Bid>();
        }
        return new Collection<Auction>(ongoingAuctions);
    }

    public Collection<Auction> GetAllAuctions()
    {
        var auctions = _auctionPersistence.Get().OrderBy(a => a.EndTime).ToList();
        return new Collection<Auction>(auctions);
    }

    public Auction GetAuctionById(int id)
    {
        var auction = _auctionPersistence.GetById(id, a => a.Bids);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {id} not found.");
        }

        auction.Bids = auction.Bids?.OrderByDescending(b => b.Amount).ToList() ?? new List<Bid>();
        return auction;
    }


    public void CreateAuction(Auction auction)
    {
        _auctionPersistence.Create(auction);
    }

    public void UpdateAuction(int auctionId, string newDescription)
    {
        var auction = _auctionPersistence.GetById(auctionId);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
        }
        auction.Description = newDescription;
        _auctionPersistence.Update(auction);
    }

    public void DeleteAuction(int id)
    {
        _auctionPersistence.Remove(id);
    }

    public List<Auction> GetUserBidsOnOngoingAuctions(string userId, string search = null)
    {
        var currentDateTime = DateTime.UtcNow;
        var auctions = _auctionPersistence.Get(
            a => a.StartTime <= currentDateTime && a.EndTime >= currentDateTime && 
                 a.Bids.Any(b => b.UserId == userId),
            a => a.Bids 
        );
        if (!string.IsNullOrWhiteSpace(search))
        {
            auctions = new Collection<Auction>(
                auctions.Where(a => a.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                    a.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList()
            );
        }
        return auctions.ToList();
    }

    public IEnumerable<Auction> GetWonAuctions(string userId)
    {
        var currentDateTime = DateTime.UtcNow;
        var auctions = _auctionPersistence.Get(
            a => a.EndTime <= currentDateTime &&
                 a.Bids.Any(b => b.UserId == userId && 
                                 b.Amount == a.Bids.Max(maxB => maxB.Amount)),
            a => a.Bids 
        );
        return auctions.ToList();
    }
    public IEnumerable<Auction> GetAuctionsByUserId(string userId)
    {
        return _auctionPersistence.Get().Where(a => a.OwnerId == userId).ToList();
    }
}
