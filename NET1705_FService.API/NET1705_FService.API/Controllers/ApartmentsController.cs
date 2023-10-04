using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1715_FService.Service.Inteface;

namespace NET1715_FService.API.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentService _apartmentService;

        public ApartmentsController(IApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApartmentsOnFloor(int floorId, int typeId)
        {
            try
            {
                var apartments = await _apartmentService.GetApartmentOnFloorAsync(floorId, typeId);
                if (!apartments.Any())
                {
                    return NotFound();
                }
                return Ok(apartments);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartmentById(int id)
        {
            try
            {
                var apartment = await _apartmentService.GetApartmentByIdAsync(id);
                if (apartment == null)
                {
                    return NotFound();
                }
                return Ok(apartment);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
