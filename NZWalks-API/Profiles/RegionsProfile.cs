using AutoMapper;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Profiles
{
    public class RegionsProfile:Profile
    {
        public RegionsProfile()
        {
            CreateMap<Region, RegionDTO>().ForMember(x => x.Iddddd, y => y.MapFrom(z => z.Id));
        }
    }
}
