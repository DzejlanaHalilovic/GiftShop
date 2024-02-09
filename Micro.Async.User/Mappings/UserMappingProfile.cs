using AutoMapper;
using Micro.Async.User.Contracts.User.Request;

namespace Micro.Async.User.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {

            CreateMap<RegisterRequest, Models.User>();
            CreateMap<UpdateRequest, Models.User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => srcMember != null));

        }
    }
}
