using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Services.Interface;

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

        [HttpPost]
        public async Task<IActionResult> CallBackVnpayUrl(VnpayModel vnpayModel)
        {
            var status = await _vnpayService.PaymentExecute(vnpayModel);
            if (status)
            {
                return Ok(new ResponseModel { Status = "Success", Message = "Payment successfully" });
            }
            ResponseModel reps = new ResponseModel { Status = "Error", Message = "Error..." };
            return BadRequest(reps);
        }
    }
}
