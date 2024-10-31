using Auktion.Core;

namespace Auktion.Models.Auctions;

public class AuctionDetails
{
    public String Title { get; set; }
    public String Description { get; set; }
    public decimal StartPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ICollection<BidVm> Bids { get; set; }

    public static AuctionDetails ToAuctionVm(Auction auction)
    {
        AuctionDetails AVM = new AuctionDetails()
        {
            Title = auction.Title,
            Description = auction.Description,
            StartPrice = auction.StartingPrice,
            StartTime = auction.StartTime,
            EndTime = auction.EndTime,
            Bids = auction.Bids.Select(b => BidVm.toBidVm(b)).ToList()  // Convert each Bid to BidVm
        };
        return AVM;
    }
}