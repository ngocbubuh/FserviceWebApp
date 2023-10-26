using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using Newtonsoft.Json;

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
        public async Task<IActionResult> GetAllApartmentPackages([FromQuery] PaginationParameter paginationParameter) 
        { 
            try
            {
                var apartmentPackages = await _apartmentPackageService.GetAllApartmentPackagesAsync(paginationParameter);
                if (!apartmentPackages.Any()) 
                { 
                    return NotFound();
                }
                var metadata = new
                {
                    apartmentPackages.TotalCount,
                    apartmentPackages.PageSize,
                    apartmentPackages.CurrentPage,
                    apartmentPackages.TotalPages,
                    apartmentPackages.HasNext,
                    apartmentPackages.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
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

        [HttpGet("apartment{id}")]
        [Authorize(Roles = "ADMIN, USER")]
        public async Task<IActionResult> GetApartmentPackagesByApartmentId([FromQuery] PaginationParameter paginationParameter, int id)
        {
            try
            {
                var apartmentPackages = await _apartmentPackageService.GetApartmentPackagesByApartmentId(paginationParameter, id);
                if (!apartmentPackages.Any())
                {
                    return NotFound();
                }
                var metadata = new
                {
                    apartmentPackages.TotalCount,
                    apartmentPackages.PageSize,
                    apartmentPackages.CurrentPage,
                    apartmentPackages.TotalPages,
                    apartmentPackages.HasNext,
                    apartmentPackages.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(apartmentPackages);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
