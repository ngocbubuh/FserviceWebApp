using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using Newtonsoft.Json;

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
        public async Task<IActionResult> GetAllServiceAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var services = await _serviceService.GetAllServicesAsync(paginationParameter);
                var metadata = new
                {
                    services.TotalCount,
                    services.PageSize,
                    services.CurrentPage,
                    services.TotalPages,
                    services.HasNext,
                    services.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
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
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddServiceAsync(NET1705_FService.Repositories.Models.Service newService)
        {
            try
            {
                var result = await _serviceService.AddServiceAsync(newService);
                if (result.Status.Equals("Success"))
                {
                    var service = await _serviceService.GetServiceAsync(int.Parse(result.Message));
                    return Ok(service);
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
        public async Task<IActionResult> UpdateServiceAsync(int id, NET1705_FService.Repositories.Models.Service updateService)
        {
            try
            {
                var result = await _serviceService.UpdateServiceAsync(id, updateService);
                if (result.Status.Equals("Success"))
                {
                    var service = await _serviceService.GetServiceAsync(int.Parse(result.Message));
                    return Ok(service);
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
        public async Task<IActionResult> DeleteServiceAsync(int id)
        {
            try
            {
                var result = await _serviceService.DeleteServiceAsync(id);
                if (result.Status.Equals("Success"))
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
