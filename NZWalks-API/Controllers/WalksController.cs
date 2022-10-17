using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISqlRepository<Walk,Guid> walksRepository;
        private readonly ISqlRepository<WalkDifficulty, Guid> walkDifficultyRepository;
        private readonly ISqlRepository<Region,Guid> regionRepository;

        public WalksController(IMapper mapper, ISqlRepository<Walk,Guid> walksRepository,
            ISqlRepository<Region,Guid> regioRepository, ISqlRepository<WalkDifficulty,Guid> walkDifficultyRepository)
        {
            this.mapper = mapper;
            this.regionRepository = regioRepository;
            this.walksRepository = walksRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }


        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkAsync([FromRoute] Guid id)
        {
            var walk = await walksRepository.GetAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(walk));
        }
        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalks()
        {
            var responce = await walksRepository.GetAllAsync();
            return Ok(mapper.Map<IEnumerable<WalkDTO>>(responce));
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            if (! await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }
            var walk = new Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            var responce = await walksRepository.AddAsync(walk);
            return Ok(mapper.Map<WalkDTO>(responce));
        }


        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            var responce = await walksRepository.DeleteAsync(id);
            if (responce == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(responce));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            if (!await ValidateUpdateWalkAsyc(updateWalkRequest))
            {
                return BadRequest(ModelState);
            }
            var walk = new Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
                RegionId = updateWalkRequest.RegionId
            };
            var responce= await walksRepository.UpdateAsync(id, walk);
            if (responce==null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(responce));
        }

        #region Private Methods

        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest addWalkRequest)
        {
            
            var region =await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                   $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty=await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                   $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsyc(UpdateWalkRequest updateWalkRequest)
        {
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                   $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                   $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        #endregion

    }
}
