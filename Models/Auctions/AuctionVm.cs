using System.ComponentModel.DataAnnotations;
using Auktion.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Auktion.Models.Auctions;

public class AuctionVm
{
    public int AuctionId { get; set; }
    public String Title { get; set; }
    public String Description { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ICollection<BidVm>? Bids { get; set; }
    
}