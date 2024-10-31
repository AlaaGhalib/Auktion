using Auktion.Core;

namespace Auktion.Models.Auctions;

public class BidVm
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public static BidVm toBidVm(Bid bid)
    {
        BidVm BVM = new BidVm();
        BVM.Amount = bid.Amount;
        BVM.Date = bid.Time;
        return BVM;
    }
}