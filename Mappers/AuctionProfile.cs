using Auktion.Core;
using Auktion.Models.Auctions;
using Auktion.Persistence;
using AutoMapper;

namespace Auktion.Mappers;

public class AuctionProfile : Profile
{
    public AuctionProfile()
    {
        CreateMap<AuctionDb, Auction>().ReverseMap();
        CreateMap<AuctionVm, Auction>()
            .ForMember(dest => dest.Bids, opt => opt.Ignore()) 
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore()).ReverseMap();
    }
}