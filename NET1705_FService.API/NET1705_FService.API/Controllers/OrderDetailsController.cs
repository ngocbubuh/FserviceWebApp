using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Services.Interface;
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

        [HttpGet("{apartmentPackageId}")]
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
    }
}
