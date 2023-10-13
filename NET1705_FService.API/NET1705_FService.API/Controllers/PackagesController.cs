using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using Newtonsoft.Json;

namespace NET1705_FService.API.Controllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;
        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPackages([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var packages = await _packageService.GetAllPackagesAsync(paginationParameter);
                var metadata = new
                {
                    packages.TotalCount,
                    packages.PageSize,
                    packages.CurrentPage,
                    packages.TotalPages,
                    packages.HasNext,
                    packages.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(packages);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageAsync(int id, int typeId)
        {
            try
            {
                var package = await _packageService.GetPackageAsync(id, typeId);
                return package != null ? Ok(package) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddPackageAsync(Package newPackage)
        {
            try
            {
                var result = await _packageService.AddPackageAsync(newPackage);
                if (result.Status.Equals("Success"))
                {
                    var package = await _packageService.GetPackageAsync(int.Parse(result.Message), 1);
                    return Ok(package);
                }
                return BadRequest(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdatePackageAsync(int id, Package updatePackage)
        {
            try
            {
                var result = await _packageService.UpdatePackageAsync(id, updatePackage);
                if (result.Status.Equals("Success"))
                {
                    //var package = await _packageService.GetPackageAsync(int.Parse(result.Message), 1);
                    return Ok( new ResponseModel { Status = "Success", Message = $"Update package {updatePackage.Name} successfully"}
                        );
                }
                return NotFound(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeletePackageAsync(int id)
        {
            try
            {
                var result = await _packageService.DeletePackageAsync(id);
                if (result.Status.Equals("Success"))
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
