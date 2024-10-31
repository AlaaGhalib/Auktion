using System.ComponentModel.DataAnnotations;
using Auktion.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Auktion.Models.Auctions;

public class AuctionVm
{
    public int AuctionId { get; set; }
    public String Title { get; set; }
    public String Description { get; set; }
    public decimal StartPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ICollection<BidVm> Bids { get; set; }

    public static AuctionVm ToAuctionVm(Auction auction)
    {
        AuctionVm AVM = new AuctionVm
        {
            AuctionId = auction.AuctionId,
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