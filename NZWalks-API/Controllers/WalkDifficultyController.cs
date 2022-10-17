using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkDifficultyController : ControllerBase
    {
        private readonly ISqlRepository<WalkDifficulty,Guid> walkDifficultyRepo;
        private readonly IMapper mapper;

        public WalkDifficultyController(ISqlRepository<WalkDifficulty,Guid> walkDifficultyRepo, IMapper mapper)
        {
            this.walkDifficultyRepo = walkDifficultyRepo;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] AddWalkdiffucultyRequest request)
        {
            if (!ValidateAddWalkDifficultyAsync(request))
            {
                return BadRequest(ModelState);
            }
            var walkDifficulty = new WalkDifficulty()
            {
                Code = request.Code
            };

            var responce=await walkDifficultyRepo.AddAsync(walkDifficulty);
            return Ok(mapper.Map<WalkDifficultyDTO>(responce));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkDifficyltyAsync(Guid id)
        {
            var walkDifficulty=await walkDifficultyRepo.GetAsync(id);
            if (walkDifficulty==null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDifficultyDTO>(walkDifficulty));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            var walkDifficulties=await walkDifficultyRepo.GetAllAsync();
            return Ok(mapper.Map<IEnumerable<WalkDifficultyDTO>>(walkDifficulties));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty =await walkDifficultyRepo.DeleteAsync(id);
            if (walkDifficulty==null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDifficultyDTO>(walkDifficulty));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, UpdateWalkDiffucultyRequest request)
        {
            if (!ValidateUpdateWalkDifficultyAsync(request))
            {
                return BadRequest(ModelState);
            }
            var walkDifficulty = new WalkDifficulty()
            {
                Code = request.Code
            };
            var responce=await walkDifficultyRepo.UpdateAsync(id, walkDifficulty);
            if (responce == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDifficultyDTO>(responce));
        }

        #region

        private bool ValidateAddWalkDifficultyAsync(AddWalkdiffucultyRequest addWalkdiffucultyRequest)
        {
            if (addWalkdiffucultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkdiffucultyRequest),
                    $"{nameof(addWalkdiffucultyRequest)} Data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkdiffucultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkdiffucultyRequest.Code),
                    $"{nameof(addWalkdiffucultyRequest.Code)} cannot be null or emmpty or white space.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(UpdateWalkDiffucultyRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"{nameof(updateRegionRequest)} Data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null or emmpty or white space.");
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
