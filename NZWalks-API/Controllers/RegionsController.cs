using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async  Task<IActionResult> GetAllRegionsAsync()
        {
            var regionsDTO = mapper.Map<IEnumerable<RegionDTO>>(await regionRepository.GetAllAsync());
            return Ok(regionsDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region==null)
            {
                return NotFound();
            }
            var regionDTO=mapper.Map<RegionDTO>(region);
            return Ok(regionDTO);
        }
    }
}
