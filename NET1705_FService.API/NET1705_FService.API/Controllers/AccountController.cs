using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using NET1715_FService.Service.Services;

namespace NET1715_FService.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService repo)
        {
            accountService = repo;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            try
            {
                var result = await accountService.SignUpAsync(model);
                if (result != null)
                {
                    return Ok(result);
                }
                return Unauthorized();
            } catch { return BadRequest(); }
        }

        [HttpPost("SignUp-Admin")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SignUpAdmin(SignUpModel model)
        {
            try
            {
                var result = await accountService.SignUpAdminAsync(model);
                if (result != null)
                {
                    return Ok(result);
                }
                return Unauthorized();
            }
            catch { return BadRequest(); }
        }

        [HttpPost("SignUp-Staff")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SignUpStaff(SignUpModel model)
        {
            try
            {
                var result = await accountService.SignUpStaffAsync(model);
                if (result != null)
                {
                    return Ok(result);
                }
                return Unauthorized();
            }
            catch { return BadRequest(); }
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            try
            {
                var result = await accountService.SignInAsync(model);
                if (string.IsNullOrEmpty(result))
                {
                    return Unauthorized();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
