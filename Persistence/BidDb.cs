using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auktion.Persistence;

public class BidDb
{
    [Key]
    public int BidId { get; set; }
    
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
    public string UserId { get; set; }
    
    [ForeignKey("AuctionId")]
    public AuctionDb Auction { get; set; }

    public int AuctionId { get; set; }
}