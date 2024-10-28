using Auktion.Core;
using Auktion.Persistence;
using AutoMapper;

namespace Auktion.Mappers;

public class AuctionProfile : Profile
{
    public AuctionProfile()
    {
        CreateMap<AuctionDb, Auction>().ReverseMap();
    }
}