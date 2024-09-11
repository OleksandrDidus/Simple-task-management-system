using Api.Data.DTO;
using Api.Data.Model;
using AutoMapper;

namespace Api.Data.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));
            CreateMap<UserDto, User>();
        }
    }
}
