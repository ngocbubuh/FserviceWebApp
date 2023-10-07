using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;

namespace NET1705_FService.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> AddOrderAsync(OrderModel order)
        {
            try
            {
                if (order.Type != "extra")
                {
                    var result = await _orderService.AddOrderAsync(order);
                    if (result.Status.Equals("Success"))
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);

                }
                else
                {
                    var result = await _orderService.AddExtraOrderAsync(order);
                    if (result.Status.Equals("Success"))
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        
    }
}
