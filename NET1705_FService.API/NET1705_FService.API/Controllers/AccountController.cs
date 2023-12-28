using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.API.Helper;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using NET1715_FService.Service.Inteface;
using NET1715_FService.Service.Services;
using System.Security.Claims;

namespace NET1715_FService.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IMailService mailService;
        private readonly IUserService _userService;
        private readonly INotificationService _notiService;

        public AccountController(IAccountService repo, 
            IMailService mailService, 
            IUserService userService, 
            INotificationService notiService)
        {
            accountService = repo;
            this.mailService = mailService;
            _userService = userService;
            _notiService = notiService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await accountService.SignUpAsync(model);
                    if (result.Status.Equals("Success"))
                    {
                        //Config mail
                        var token = result.ConfirmEmailToken;
                        var confirmationEmail = Url.Action(nameof(ConfirmEmail), "Account", new { token = token.Result, email = model.Email }, Request.Scheme);
                        result.ConfirmEmailToken = null;
                        var messageRequest = new MailRequest
                        {
                            ToEmail = model.Email,
                            Subject = "FServices Confirmation Email",
                            Body = SendConfirmEmail.EnailContent(model.Email, confirmationEmail)
                        };
                        //Send Mail
                        await mailService.SendEmailAsync(messageRequest);
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return ValidationProblem(ModelState);
            }
            catch { return BadRequest(); }
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await accountService.ChangePassword(model);
                    if (result.Status.Equals("Success"))
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return ValidationProblem(ModelState);
            } catch { return BadRequest(); }
        }

        //[HttpPost("SignUp-Admin")]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<IActionResult> SignUpAdmin(SignUpModel model)
        //{
        //    try
        //    {
        //        var result = await accountService.SignUpAdminAsync(model);
        //        if (result.Status.Equals("Success"))
        //        {
        //            return Ok(result);
        //        }
        //        return Unauthorized(result);
        //    }
        //    catch { return BadRequest(); }
        //}

        //[HttpPost("SignUp-Staff")]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<IActionResult> SignUpStaff(SignUpModel model)
        //{
        //    try
        //    {
        //        var result = await accountService.SignUpStaffAsync(model);
        //        if (result.Status.Equals("Success"))
        //        {
        //            return Ok(result);
        //        }
        //        return Unauthorized(result);
        //    }
        //    catch { return BadRequest(); }
        //}

        [HttpPost("SignUp-Internal")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SignUpInternal(SignUpModel model, RoleModel role)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = await accountService.SignUpInternalAsync(model, role);
                    if (result.Status.Equals("Success"))
                    {
                        return Ok(result);
                    }
                    return Unauthorized(result);
                } return ValidationProblem(ModelState);
            }
            catch { return BadRequest(); }
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await accountService.SignInAsync(model);
                    if (result.Status.Equals(false))
                    {
                        if(result.Message.Equals("Email xác nhận đã được gửi đến tài khoản email bạn đã đăng ký, vui lòng xác thực tài khoản để đăng nhập!"))
                        {
                            var token = result.VerifyEmailToken;
                            var confirmationEmail = Url.Action(nameof(ConfirmEmail), "Account", new { token = token.Result, email = model.Email }, Request.Scheme);
                            result.VerifyEmailToken = null;
                            var messageRequest = new MailRequest
                            {
                                ToEmail = model.Email,
                                Subject = "FServices Confirmation Email",
                                Body = SendConfirmEmail.EnailContent(model.Email, confirmationEmail)
                            };
                            //Send Mail
                            await mailService.SendEmailAsync(messageRequest);
                        }
                        return Unauthorized(result);
                    }
                    return Ok(result);
                } return ValidationProblem(ModelState);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            try
            {
                var result = await accountService.RefreshToken(model);
                if (result.Status.Equals(false))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch 
            {
                return BadRequest();
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var result = await accountService.ConfirmEmail(token, email);
                if (result.Status.Equals(false))
                {
                    return Unauthorized(result);
                }
                var account = await _userService.GetAccountByUsernameAsync(email);
                if (account != null)
                {
                    Notification notification = new Notification 
                    { 
                        AccountId = account.Id,
                        CreateDate = DateTime.Now,
                        Type = "System",
                        Title = "Chào mừng bạn đến với FService",
                        Message = "Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. Hãy cùng nhau khám phá các tính năng nhé!"
                    };
                    await _notiService.AddNotificationByAccountId(account.Id, notification);
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // verify by token and get data in body token
        [Authorize]
        [HttpGet("Launch")]
        public async Task<ActionResult<AccountsModel>> Launch()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var extractedEmail = AuthenTools.GetCurrentEmail(identity);

            if (extractedEmail == null) return NotFound("Token is expired!");

            var result = await _userService.GetAccountByUsernameAsync(extractedEmail);

            return Ok(result);
        }

        // update device token for send message by firebase
        [Authorize]
        [HttpPut("update-device-token", Name = "Update device token")]
        public async Task<IActionResult> UpdateDeviceToken(DeviceTokenModal deviceToken)
        {
            try
            {
                var result = await accountService.UpdateDeviceToken(deviceToken.AccountId, deviceToken.DeviceToken);
                if (result)
                {
                    return Ok(new ResponseModel
                    {
                        Status = "Success",
                        Message = "Update token successfully"
                    });
                }
                return BadRequest(new ResponseModel
                {
                    Status = "Error",
                    Message = "Can not update token"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
