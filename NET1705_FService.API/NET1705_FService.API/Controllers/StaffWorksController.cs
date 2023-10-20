using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using NET1705_FService.Services.Services;
using Newtonsoft.Json;

namespace NET1705_FService.API.Controllers
{
    [Route("api/staffworks")]
    [ApiController]
    public class StaffWorksController : ControllerBase
    {
        private readonly IOrderDetailsService _orderDetailsService;

        public StaffWorksController(IOrderDetailsService orderDetailsService) 
        {
            _orderDetailsService = orderDetailsService;
        }

        [HttpGet("{staffId}")]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> GetAllTaskForStaff([FromQuery] PaginationParameter paginationParameter, string staffId)
        {
            try
            {
                var tasks = await _orderDetailsService.GetAllTaskForStaffAsync(paginationParameter, staffId);
                var metadata = new
                {
                    tasks.TotalCount,
                    tasks.PageSize,
                    tasks.CurrentPage,
                    tasks.TotalPages,
                    tasks.HasNext,
                    tasks.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(tasks);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> UpdateTaskAsync(int id, OrderDetailModel model)
        {
            try
            {
                var updateId = await _orderDetailsService.UpdateTaskAsync(id, model);
                if (updateId == 0)
                {
                    return BadRequest("Cannot update");
                }
                var task = await _orderDetailsService.GetOrderDetailsByIdAsync(updateId);
                return Ok(task);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
