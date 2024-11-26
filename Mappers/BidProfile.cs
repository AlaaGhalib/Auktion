using Auktion.Core;
using Auktion.Models.Auctions;
using Auktion.Persistence;
using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Auktion.Mappers;

public class BidProfile : Profile
{
    public BidProfile()
    {
        CreateMap<BidDb, Bid>().ReverseMap();
        CreateMap<Bid, BidVm>().ReverseMap();
        CreateMap<BidDb, Bid>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time));

    }
}