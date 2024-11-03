using System.ComponentModel.DataAnnotations;
using Auktion.Core;

namespace Auktion.Persistence;

public class AuctionDb
{
    [Key]
    public int AuctionId { get; set; }
    [MaxLength(128)]
    public string Title { get; set; }
    public string Description { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string OwnerId { get; set; }

    public ICollection<BidDb> Bids { get; set; }  // Relationen till bud
}