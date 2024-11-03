using System.ComponentModel.DataAnnotations;
using Auktion.Areas.Identity.Data;

namespace Auktion.Core;
public class Auction
{
    public int AuctionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string OwnerId { get; set; }

    public ICollection<Bid> Bids { get; set; }  // Relationen till bud
}