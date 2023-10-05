using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using NET1715_FService.Service.Inteface;
using NET1715_FService.Service.Services;

namespace NET1705_FService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService accountService;

        public UserController(IUserService accService)
        {
            accountService = accService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccountAsync()
        {
            //try
            {
                var accounts = await accountService.GetAllAccountAsync();
                return Ok(accounts);
            }
            //catch
            //{
            //    return BadRequest();
            //}
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAccount(string id)
        {
            try
            {
                var account = await accountService.GetAccountAsync(id);
                return account == null ? NotFound() : Ok(account);
            }
            catch { return BadRequest(); }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAccount(string id, Accounts model)
        {
            try
            {
                var result = await accountService.UpdateAccountAsync(id, model);
                if (result.Status.Equals("Success"))
                {
                    var account = await accountService.GetAccountAsync(result.Message);
                    return Ok(account);
                }
                return NotFound(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePackage(string id)
        {
            try
            {
                var result = await accountService.DeleteAccountAsync(id);
                if (result.Status.Equals("Success"))
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
