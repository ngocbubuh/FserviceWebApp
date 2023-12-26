using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Repositories;

namespace NET1705_FService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebasesController : ControllerBase
    {
        private readonly IFirebaseRepository _firebaseRepository;

        public FirebasesController(IFirebaseRepository firebaseRepository) 
        {
            _firebaseRepository = firebaseRepository;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SendMessageCloud([FromQuery] string title, [FromQuery] string body, [FromQuery] string token)
        {
            try
            {
                var res = await _firebaseRepository.PushNotificationFireBaseToken(title, body, token);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
