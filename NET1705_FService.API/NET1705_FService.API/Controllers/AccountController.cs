using FServiceAPI.Models;
using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using NET1715_FService.Service.Services;

namespace NET1715_FService.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            //try
            //{
                var result = await accountService.SignInAsync(model);
                if (string.IsNullOrEmpty(result))
                {
                    return Unauthorized();
                }
                return Ok(result);
            //} catch
            //{
            //    return Unauthorized();
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
