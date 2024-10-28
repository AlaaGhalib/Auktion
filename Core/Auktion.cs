using Auktion.Areas.Identity.Data;

namespace Auktion.Core;
public class Auction
{
    public int AuctionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string OwnerId { get; set; }
    public AuktionUser Owner { get; set; }  // Koppling till användaren

    public ICollection<Bid> Bids { get; set; }  // Relationen till bud
}

public class Bid
{
    public int BidId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }

    public string UserId { get; set; }
    public AuktionUser User { get; set; }  // Koppling till användaren

    public int AuctionId { get; set; }
    public Auction Auction { get; set; }  // FK till auktionen
}