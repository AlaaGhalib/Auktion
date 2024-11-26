using System.Collections.ObjectModel;
using Auktion.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Auktion.Core;

public class AuctionService : IAuctionService
{
    private readonly IAuctionPersistence _persistence;

    public AuctionService(IAuctionPersistence persistence)
    {
        _persistence = persistence;
    }

    public Collection<Auction> GetAuctions()
    {
        var currentDateTime = DateTime.Now;
        var ongoingAuctions = _persistence.GetAuctions()
            .Where(a => a.EndTime > currentDateTime) 
            .OrderBy(a => a.EndTime)                
            .ToList();
        foreach (var auction in ongoingAuctions)
        {
            auction.Bids = new List<Bid>(
                auction.Bids.OrderByDescending(b => b.Amount)
            );
        }
        return new Collection<Auction>(ongoingAuctions);
    }

    public Auction GetAuctionById(int id)
    {
        var auction = _persistence.GetAuctionById(id);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {id} not found.");
        }
        auction.Bids = auction.Bids != null 
            ? new List<Bid>(auction.Bids.OrderByDescending(b => b.Amount))
            : new List<Bid>(); 
        return auction;
    }


    public void CreateAuction(Auction auction)
    {
        _persistence.CreateAuction(auction);
    }

    public void UpdateAuction(int auctionId, string newDescription)
    {
        var auction = _persistence.GetAuctionById(auctionId);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
        }
        auction.Description = newDescription;
        _persistence.UpdateAuction(auction);
    }

    public void DeleteAuction(int id)
    {
        _persistence.DeleteAuction(id);
    }
    public List<Auction> GetUserBidsOnOngoingAuctions(string userId, string search = null)
    {
        var currentDateTime = DateTime.UtcNow;
        var query = _persistence.GetAuctions()
            .Where(a => a.StartTime <= currentDateTime && a.EndTime >= currentDateTime)
            .Where(a => a.Bids.Any(b => b.UserId == userId));
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a => a.Title.Contains(search) || a.Description.Contains(search));
        }
        return query.ToList();
    }
    public IEnumerable<Auction> GetWonAuctions(string userId)
    {
        var auctions = _persistence.GetAuctions()  
            .Where(a => a.EndTime <= DateTime.Now && 
                        a.Bids.Any(b => b.UserId == userId && 
                                        b.Amount == a.Bids.Max(maxB => maxB.Amount)))
            .ToList();
        return auctions;
    }
}
