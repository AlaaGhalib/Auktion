namespace Auktion.Core;

public class Bid
{
    public int BidId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
    public string UserId { get; set; }
    public int AuctionId { get; set; }
}