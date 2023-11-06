using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using Newtonsoft.Json;

namespace NET1705_FService.API.Controllers
{
    [Route("api/orders")]
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
                IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var acceptedUrls = _configuration["AcceptPaymentUrl:Url"];
                string[] urls = acceptedUrls.Split(',');
                string vnpReturnUrl = null;

                foreach (string url in urls)
                {
                    if (order.CallBackUrl == url)
                    {
                        vnpReturnUrl = url;
                    }
                }
                if (string.IsNullOrEmpty(vnpReturnUrl))
                {
                    return BadRequest(new ResponseModel { Status = "Error", Message = "Return url invalid" });
                }
                if (order.Type != "extra")
                {
                    var result = await _orderService.AddOrderAsync(order, HttpContext);
                    if (result.Status.Equals("Success"))
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);

                }
                else
                {
                    var result = await _orderService.AddExtraOrderAsync(order, HttpContext);
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

        //[HttpPost]
        ////[Authorize(Roles = "USER")]
        //public async Task<IActionResult> AddOrderAsync(OrderModel order)
        //{

        //        if (order.Type != "extra")
        //        {
        //            var result = await _orderService.AddOrderAsync(order, HttpContext);
        //            if (result.Status.Equals("Success"))
        //            {
        //                return Ok(result);
        //            }
        //            return BadRequest(result);

        //        }
        //        else
        //        {
        //            var result = await _orderService.AddExtraOrderAsync(order);
        //            if (result.Status.Equals("Success"))
        //            {
        //                return Ok(result);
        //            }
        //            return BadRequest(result);
        //        }
        //}

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllOrdersAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync(paginationParameter);
                var metadata = new
                {
                    orders.TotalCount,
                    orders.PageSize,
                    orders.CurrentPage,
                    orders.TotalPages,
                    orders.HasNext,
                    orders.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                if (!orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> GetOrdersByUserName([FromQuery] PaginationParameter paginationParameter, string username)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserName(paginationParameter, username);
                var metadata = new
                {
                    orders.TotalCount,
                    orders.PageSize,
                    orders.CurrentPage,
                    orders.TotalPages,
                    orders.HasNext,
                    orders.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                if (!orders.Any())
                {
                    ResponseModel resp = new ResponseModel { Status = "Error", Message = $"Not found order of user name: {username}" };
                    return NotFound(resp);
                }
                return Ok(orders);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateOrderPayment(int id, Order order)
        {
            try
            {
                var result = await _orderService.UpdateOrderAsync(id, order);
                if (result.Status.Equals("Error"))
                {
                    return NotFound(result);
                }
                var updateOrder = await _orderService.GetOrderByIdAsync(int.Parse(result.Message));
                return Ok(updateOrder);
            }
            catch
            {
                return BadRequest();
            }
        }
        
    }
}
