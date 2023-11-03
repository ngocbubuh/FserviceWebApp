using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Services.Interface;
using System.Net.WebSockets;

namespace NET1705_FService.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnpayService _vnpayService;

        public PaymentController(IVnpayService vnpayService) 
        {
            _vnpayService = vnpayService;
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn([FromQuery] VnpayModel response)
        {
            try
            {
                if (response != null)
                {
                    var status = await _vnpayService.PaymentExecute(response);
                    if (status)
                    {
                        return Ok(response);
                    }
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CallBackVnpayUrl(VnpayModel vnpayModel)
        {
            var status = await _vnpayService.PaymentExecute(vnpayModel);
            if (status)
            {
                return Ok(new ResponseModel { Status = "Success", Message = "Payment successfully" });
            }
            ResponseModel reps = new ResponseModel { Status = "Error", Message = "Invalid information." };
            return BadRequest(reps);
        }
    }
}
