using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.API.Helper;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Services.Interface;

namespace NET1705_FService.API.Controllers
{
    [Route("api/usepackages")]
    [ApiController]
    public class UsingPackagesController : ControllerBase
    {
        private readonly IOrderDetailsService _orderDetailsService;

        public UsingPackagesController(IOrderDetailsService orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }

        [HttpPost]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> UseServiceOnPackage(UsingPackageModel usingPackage)
        {
            try
            {
                if (!Validation.CheckPhoneNumber(usingPackage.CustomerPhone))
                {
                    return BadRequest(
                        new ResponseModel
                        {
                            Status = "Error",
                            Message = "Phone number is invalid."
                        }
                        );
                }
                if (!Validation.CheckName(usingPackage.CustomerName))
                {
                    return BadRequest(
                        new ResponseModel
                        {
                            Status = "Error",
                            Message = "Name is short."
                        }
                        );
                }
                var result = await _orderDetailsService.AddOrderDetails(usingPackage);
                if (result.Status.Equals("Error"))
                {
                    return NotFound(result);
                }
                var orderDetails = await _orderDetailsService.GetOrderDetailsByIdAsync(int.Parse(result.Message));
                return Ok(orderDetails);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
