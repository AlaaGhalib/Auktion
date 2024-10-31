
using System.Collections.ObjectModel;
using Auktion.Core;
using Auktion.Core.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auktion.Persistence;

public class AuctionPersistence : IAuctionPersistence
{
    private AuctionDbContext _context;
    private IMapper _mapper;
    public AuctionPersistence(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public Collection<Auction> GetAuctions()
    {
        var auctionDbs = _context.Auctions.Include(a => a.Bids).ToList();
        return _mapper.Map<Collection<Auction>>(auctionDbs);
    }

    public Auction GetAuctionById(int id)
    {
        var auctionDb = _context.Auctions.Include(a => a.Bids).FirstOrDefault(a => a.AuctionId == id);
        return _mapper.Map<Auction>(auctionDb);
    }

    public void CreateAuction(Auction auction)
    {
        var auctionDb = _mapper.Map<AuctionDb>(auction);
        _context.Auctions.Add(auctionDb);
        _context.SaveChanges();
    }

    public void UpdateAuction(Auction auction)
    {
        var auctionDb = _context.Find<AuctionDb>(auction.AuctionId);
        if (auctionDb == null)
            throw new NullReferenceException("Auction could not be found");

        _context.Entry(auctionDb).CurrentValues.SetValues(auction);
        _context.SaveChanges();
    }

    public void DeleteAuction(int id)
    {
        var auction = _context.Auctions.Find(id);
        if (auction != null)
        {
            _context.Auctions.Remove(auction);
            _context.SaveChanges();
        }
    }
    
}