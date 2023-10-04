using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1715_FService.Service.Inteface;

namespace NET1715_FService.API.Controllers
{
    [Route("api/buildings")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuildings()
        {
            try
            {
                var buildings = await _buildingService.GetAllBuildings();
                if (!buildings.Any())
                {
                    return NotFound();
                }
                return Ok(buildings);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
