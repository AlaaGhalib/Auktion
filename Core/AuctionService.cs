using System.Collections.ObjectModel;
using Auktion.Core.Interfaces;

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
        return _persistence.GetAuctions();
    }

    public Auction GetAuctionById(int id)
    {
        return _persistence.GetAuctionById(id);
    }

    public void CreateAuction(Auction auction)
    {
        _persistence.CreateAuction(auction);
    }

    public void UpdateAuction(Auction auction)
    {
        _persistence.UpdateAuction(auction);
    }

    public void DeleteAuction(int id)
    {
        _persistence.DeleteAuction(id);
    }
}
