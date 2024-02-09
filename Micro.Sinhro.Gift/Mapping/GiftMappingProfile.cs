using AutoMapper;
using Micro.Sinhro.Gift.Contracts.Gift.Request;

namespace Micro.Sinhro.Gift.Mapping
{
    public class GiftMappingProfile : Profile
    {
        public GiftMappingProfile()
        {
            CreateMap<CreateRequest, Models.Gift>();
            CreateMap<UpdateRequest, Models.Gift>();
        }
    }
}
