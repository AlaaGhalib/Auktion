
using System.Collections.ObjectModel;
using Auktion.Core;
using Auktion.Core.Interfaces;
using AutoMapper;
namespace Auktion.Persistence
{
    public class BidPersistence : IBidPersistence
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;

        public BidPersistence(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Method to get all bids
        public Collection<Bid> GetBids()
        {
            var bidDbs = _context.Bids.ToList();
            return _mapper.Map<Collection<Bid>>(bidDbs);
        }

        // Method to get a bid by ID
        public Bid GetBidById(int id)
        {
            var bidDb = _context.Bids.FirstOrDefault(b => b.BidId == id);
            return _mapper.Map<Bid>(bidDb);
        }

        // Method to create a bid
        public void CreateBid(Bid bid)
        {
            var bidDb = _mapper.Map<BidDb>(bid);
            _context.Bids.Add(bidDb);
            _context.SaveChanges();
        }
        public void UpdateBid(Bid bid)
        {
            var bidDb = _context.Bids.Find(bid.BidId);
            if (bidDb == null)
                throw new NullReferenceException("Bid could not be found");

            _context.Entry(bidDb).CurrentValues.SetValues(bid);
            _context.SaveChanges();
        }
        public void DeleteBid(int id)
        {
            var bidDb = _context.Bids.Find(id);
            if (bidDb != null)
            {
                _context.Bids.Remove(bidDb);
                _context.SaveChanges();
            }
        }
    }
}
