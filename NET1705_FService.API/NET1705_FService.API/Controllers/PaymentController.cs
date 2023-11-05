using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Services.Interface;
using System.Net.WebSockets;
using System.Text.Json;

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
                var uri = HttpContext.Request.Host.ToString();
                if (response != null)
                {
                    var status = await _vnpayService.PaymentExecute(response);
                    string urlParameters = response.ToUrlParameters();
                    if (status)
                    {
                        if (uri.Contains("localhost"))
                        {
                            return Redirect("http://localhost:3000/payment/success?" + urlParameters);
                        } 
                        else
                        {
                            return Redirect("https://fservices.vercel.app/payment/success?" + urlParameters);
                        }
                    }
                    if (uri.Contains("localhost"))
                    {
                        return Redirect("http://localhost:3000/payment/error?" + urlParameters);
                    }
                    else
                    {
                        return Redirect("https://fservices.vercel.app/payment/error?" + urlParameters);
                    }
                }
                return NotFound();
            }
            catch
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
