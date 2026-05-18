using Microsoft.AspNetCore.Mvc;
using Trycore.EVM.Application.DTOs;
using Trycore.EVM.Application.Interfaces;

namespace Trycore.EVM.API.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/activities")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Guid projectId,
            CreateActivityDto dto)
        {
            var result = await _activityService.CreateAsync(projectId, dto);

            return Created(string.Empty, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByProject(Guid projectId)
        {
            var result = await _activityService.GetByProjectIdAsync(projectId);

            return Ok(result);
        }
    }
}
