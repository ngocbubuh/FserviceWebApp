using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Services.Interface;

namespace NET1705_FService.API.Controllers
{
    [Route("api/apartment-packages")]
    [ApiController]
    public class ApartmentPackagesController : ControllerBase
    {
        private readonly IApartmentPackageService _apartmentPackageService;

        public ApartmentPackagesController(IApartmentPackageService apartmentPackageService) 
        { 
            _apartmentPackageService = apartmentPackageService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllApartmentPackages() 
        { 
            try
            {
                var apartmentPackages = await _apartmentPackageService.GetAllApartmentPackagesAsync();
                if (!apartmentPackages.Any()) 
                { 
                    return NotFound();
                }
                return Ok(apartmentPackages);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<IActionResult> GetApartmentPackageById(int id)
        {
            try
            {
                var apartmentPackage = await _apartmentPackageService.GetApartmentPackageByIdAsync(id);
                return apartmentPackage == null ? NotFound() : Ok(apartmentPackage);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
