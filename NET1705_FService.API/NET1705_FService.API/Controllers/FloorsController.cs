using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1715_FService.Service.Inteface;

namespace NET1715_FService.API.Controllers
{
    [Route("api/floors")]
    [ApiController]
    public class FloorsController : ControllerBase
    {
        private readonly IFloorService _floorService;
        public FloorsController(IFloorService floorService)
        {
            _floorService = floorService;
        }
        [HttpGet]
        public async Task<IActionResult> GetFloorsByBuildingId(int buidingId)
        {
            try
            {
                var floors = await _floorService.GetFloorsByBuildingId(buidingId);
                if (!floors.Any())
                {
                    return NotFound();
                }
                return Ok(floors);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
