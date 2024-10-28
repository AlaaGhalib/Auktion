using System.ComponentModel.DataAnnotations.Schema;

namespace Auktion.Persistence;

public class BidDb
{
    public int BidId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }

    [ForeignKey("AuctionId")]
    public AuctionDb Auction { get; set; }

    public int AuctionId { get; set; }
}