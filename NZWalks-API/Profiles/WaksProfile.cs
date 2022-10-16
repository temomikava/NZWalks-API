using AutoMapper;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Profiles
{
    public class WaksProfile:Profile
    {
        public WaksProfile()
        {
            CreateMap<Walk,WalkDTO>().ReverseMap();
        }
    }
}
