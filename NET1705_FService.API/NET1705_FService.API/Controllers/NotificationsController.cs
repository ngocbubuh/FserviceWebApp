using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Services.Interface;
using NET1705_FService.API.Helper;
using NET1705_FService.Services.Services;
using Newtonsoft.Json;
using System.Security.Claims;

namespace NET1705_FService.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notiService;
        private readonly IUserService _userService;

        public NotificationsController(INotificationService service, IUserService userService)
        {
            _notiService = service;
            _userService = userService;
        }

        [Authorize]
        [HttpGet("account", Name = "GetByAccountId")]
        public async Task<IActionResult> GetAllNotificationByAccountId([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = AuthenTools.GetCurrentEmail(identity);
                if (userName == null)
                {
                    return Unauthorized("Token is expired!");
                }
                var notifications = await _notiService.GetAllNotificationsByAccountIdAsync(paginationParameter, userName);
                if (notifications == null)
                {
                    return NotFound();
                }
                var metadata = new
                {
                    notifications.TotalCount,
                    notifications.PageSize,
                    notifications.CurrentPage,
                    notifications.TotalPages,
                    notifications.HasNext,
                    notifications.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(notifications);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                var notification = await _notiService.GetNotificationById(id);
                return notification != null ? Ok(notification) : NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("numbers-unread", Name = "Get numbers of unread notification")]
        public async Task<IActionResult> GetNumbersUnReadNotification()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = AuthenTools.GetCurrentEmail(identity);
                if (userName == null)
                {
                    return Unauthorized("Token is expired!");
                }
                int result = await _notiService.GetNumbersOfUnReadNotification(userName);
                if (result == -1)
                {
                    ResponseModel resError = new ResponseModel
                    {
                        Status = "Error",
                        Message = "Cannot get numbers of unread notification",
                    };
                    return NotFound(resError);
                }
                ResponseModel res = new ResponseModel
                {
                    Status = "Success",
                    Message = result.ToString(),
                };
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("account", Name = "MarkAllReadByAccountId")]
        public async Task<IActionResult> MarkAllReadByAccountId()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var userName = AuthenTools.GetCurrentEmail(identity);
                if (userName == null)
                {
                    return Unauthorized("Token is expired!");
                }
                int result = await _notiService.MarkAllNotificationByAccountIdIsRead(userName);
                if (result == 0)
                {
                    ResponseModel resError = new ResponseModel
                    {
                        Status = "Error",
                        Message = "Cannot mark notifications is read",
                    };
                    return NotFound(resError);
                }
                ResponseModel res = new ResponseModel
                {
                    Status = "Success",
                    Message = "Mark all notifications is read successfully",
                };
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("{id}", Name = "MarkReadById")]
        public async Task<IActionResult> MarkReadById(int id)
        {
            try
            {
                int result = await _notiService.MarkNotificationIsReadById(id);
                if (result == 0)
                {
                    ResponseModel resError = new ResponseModel
                    {
                        Status = "Error",
                        Message = "Cannot mark this notification is read",
                    };
                    return NotFound(resError);
                }
                ResponseModel res = new ResponseModel
                {
                    Status = "Success",
                    Message = "Mark this notification is read successfully",
                };
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
