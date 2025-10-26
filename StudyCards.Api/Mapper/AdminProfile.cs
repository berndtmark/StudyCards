using AutoMapper;
using StudyCards.Api.Models.Response;
using StudyCards.Domain.Entities;

namespace StudyCards.Api.Mapper;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<User, UserDetails>()
            .ForMember(dest => dest.UserCreated, src => src.MapFrom(p => p.CreatedDate));
    }
}
