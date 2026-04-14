using AutoMapper;
using Test.Models;
using Test.DTOs.Post;
using Test.DTOs.User;

namespace Test.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Post → PostResponse
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.Name));

            // User → UserResponse
            CreateMap<User, UserResponse>();
        }
    }
}