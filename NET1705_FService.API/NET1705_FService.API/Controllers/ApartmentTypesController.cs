using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1715_FService.Service.Inteface;

namespace NET1715_FService.API.Controllers
{
    [Route("api/types")]
    [ApiController]
    public class ApartmentTypesController : ControllerBase
    {
        private readonly IApartmentTypeService _apartmentTypeService;

        public ApartmentTypesController(IApartmentTypeService apartmentTypeService)
        {
            _apartmentTypeService = apartmentTypeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetApartmentTypes(int? buildingId)
        {
            try
            {
                var apartmentTypes = await _apartmentTypeService.GetAllApartmentTypesAsync(buildingId);
                if (!apartmentTypes.Any())
                {
                    return NotFound();
                }
                return Ok(apartmentTypes);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
