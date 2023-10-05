using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1715_FService.Service.Inteface;

namespace NET1715_FService.API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServiceAsync()
        {
            try
            {
                var services = await _serviceService.GetAllServicesAsync();
                return Ok(services);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceAsync(int id)
        {
            try
            {
                var package = await _serviceService.GetServiceAsync(id);
                return package != null ? Ok(package) : NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddPackageAsync(NET1705_FService.Repositories.Models.Service newService)
        {
            try
            {
                var result = await _serviceService.AddServiceAsync(newService);
                if (result.Status.Equals("Success"))
                {
                    var package = await _serviceService.GetServiceAsync(int.Parse(result.Message));
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
        public async Task<IActionResult> UpdatePackageAsync(int id, NET1705_FService.Repositories.Models.Service updateService)
        {
            try
            {
                var result = await _serviceService.UpdateServiceAsync(id, updateService);
                if (result.Status.Equals("Success"))
                {
                    var package = await _serviceService.GetServiceAsync(int.Parse(result.Message));
                    return Ok(package);
                }
                return NotFound(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackageAsync(int id)
        {
            try
            {
                var result = await _serviceService.DeleteServiceAsync(id);
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
