﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Models.Domain;
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

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regionsDTO = mapper.Map<IEnumerable<RegionDTO>>(await regionRepository.GetAllAsync());
            return Ok(regionsDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id); 
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<RegionDTO>(region);
            return Ok(regionDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegion)
        {
            var region = new Region()
            {
                Code = addRegion.Code,
                Name = addRegion.Name, 
                Area = addRegion.Area,
                Lat = addRegion.Lat,
                Long = addRegion.Long,
                Population = addRegion.Population
            };
            var responce = await regionRepository.AddRegion(region);
            return CreatedAtAction(nameof(GetRegionAsync), new { id = responce.Id }, mapper.Map<RegionDTO>(responce));
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var responce= await regionRepository.DeleteAsync(id);
            if (responce==null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDTO>(responce));
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpadateRegionAsync([FromRoute]Guid id,[FromBody]UpdateRegionRequest updateRegionRequest)
        {
            var region = new Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };
            region= await regionRepository.UpdateAsync(id, region);
            if (region==null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDTO>(region));
        }
    }
}
