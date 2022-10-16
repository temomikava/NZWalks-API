using AutoMapper;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Profiles
{
    public class WalksProfile:Profile
    {
        public WalksProfile()
        {
            CreateMap<Walk,WalkDTO>().ReverseMap();
            CreateMap<WalkDifficulty,WalkDifficultyDTO>().ReverseMap();
        }
    }
}
