﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using NET1715_FService.Service.Inteface;
using NET1715_FService.Service.Services;
using Newtonsoft.Json;

namespace NET1705_FService.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService accountService;

        public UserController(IUserService accService)
        {
            accountService = accService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllAccountAsync([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var accounts = await accountService.GetAllAccountAsync(paginationParameter);
                var metadata = new
                {
                    accounts.TotalCount,
                    accounts.PageSize,
                    accounts.CurrentPage,
                    accounts.TotalPages,
                    accounts.HasNext,
                    accounts.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(accounts);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("/account-by-id/{id}")]
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

        [HttpGet("{name}")]
        [Authorize]
        public async Task<IActionResult> GetAccountByUsername(string name)
        {
            try
            {
                var account = await accountService.GetAccountByUsernameAsync(name);
                return account == null ? NotFound() : Ok(account);
            }
            catch { return BadRequest(); }
        }

        [HttpGet("/account-by-role")]
        [Authorize]
        public async Task<IActionResult> GetAccountByRole([FromQuery] PaginationParameter pagination, RoleModel role)
        {
            try
            {
                var accounts = await accountService.GetAccountsByRole(pagination, role);
                if (!accounts.Any())
                {
                    return NotFound();
                }
                var metadata = new
                {
                    accounts.TotalCount,
                    accounts.PageSize,
                    accounts.CurrentPage,
                    accounts.TotalPages,
                    accounts.HasNext,
                    accounts.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(accounts);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAccount(string id, AccountsModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = await accountService.UpdateAccountAsync(id, model);
                    if (result.Status.Equals("Success"))
                    {
                        var account = await accountService.GetAccountByUsernameAsync(result.Message);
                        return Ok(account);
                    }
                    return NotFound(result);
                } return ValidationProblem(ModelState);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(string id)
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
