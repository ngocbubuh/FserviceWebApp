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
    [Route("api/orderdetails")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _service;

        public OrderDetailsController(IOrderDetailsService service) 
        {
            _service = service;
        }

        [HttpGet("apartment-package/{apartmentPackageId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetailByApartmentPackageId([FromQuery]PaginationParameter paginationParameter, int apartmentPackageId)
        {
            try
            {
                var orderDetails = await _service.GetOrderDetailByApartmentPackageId(paginationParameter, apartmentPackageId);
                var metadata = new
                {
                    orderDetails.TotalCount,
                    orderDetails.PageSize,
                    orderDetails.CurrentPage,
                    orderDetails.TotalPages,
                    orderDetails.HasNext,
                    orderDetails.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(orderDetails);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{orderDetailId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetailById(int orderDetailId)
        {
            try
            {
                var orderDetail = await _service.GetOrderDetailsByIdAsync(orderDetailId);
                return orderDetail != null ? Ok(orderDetail) : NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "Confirm")]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> ConfirmStaffWork(int id, OrderDetailModel model)
        {
            try
            {
                var updateId = await _service.ConfirmStaffWork(id, model);
                if (updateId == 0)
                {
                    return BadRequest("Cannot update");
                }
                var task = await _service.GetOrderDetailsByIdAsync(updateId);
                return Ok(task);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
