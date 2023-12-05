﻿using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FServiceAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FserviceApiDatabaseContext _context;
        private readonly UserManager<Accounts> accountManager;
        private readonly SignInManager<Accounts> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(
            UserManager<Accounts> accountManager, 
            SignInManager<Accounts> signInManager, 
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository)
        {
            this.accountManager = accountManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }

        public async Task<ResponseModel> ConfirmEmail(string token, string email)
        {
            var user = await accountManager.FindByNameAsync(email);
            if (user != null) 
            {
                var result = await accountManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    return new ResponseModel
                    {
                        Status = "Success",
                        Message = "Email Verification Successfully!"
                    };
                }
                return new ResponseModel
                {
                    Status = "Error",
                    Message = "Email Verification Failed!"
                };
            }
            return new ResponseModel
            {
                Status = "Error",
                Message = "User does not existed!"
            };
        }

        public async Task<Accounts> GetAccountByUserName(string userName)
        {
            var account = await accountManager.FindByNameAsync(userName);
            return account;
        }

        public async Task<List<Accounts>> GetAllStaffsAsync()
        {
            bool roleExists = await roleManager.RoleExistsAsync("staff");

            if (!roleExists)
            {
                return null;
            }

            var staffAccounts = await accountManager.GetUsersInRoleAsync("staff");

            return staffAccounts.Where(a => a.Status == true).ToList();
        }

        public async Task<AuthenticationResponseModel> SignInAsync(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            var account = await accountManager.FindByNameAsync(model.Email);

            if (result.Succeeded) 
            {
                var roles = await accountManager.GetRolesAsync(account);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                //_ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
                //var token = new JwtSecurityToken(
                //    issuer: configuration["JWT:ValidIssuer"],
                //    audience: configuration["JWT:ValidAudience"],
                //    expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                //    claims: authClaims,
                //    signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
                //    );

                //Tạo token
                var token = CreateToken(authClaims);

                //Tạo refresh token
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                account.RefreshToken = refreshToken;
                account.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await accountManager.UpdateAsync(account);

                return new AuthenticationResponseModel 
                { 
                    Status = true,
                    Message = "Đăng nhập thành công!",
                    JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expired = token.ValidTo,
                    JwtRefreshToken = refreshToken,
                };
            } else if (result.IsNotAllowed)
            {
                var token = accountManager.GenerateEmailConfirmationTokenAsync(account);
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Email xác nhận đã được gửi đến tài khoản email bạn đã đăng ký, vui lòng xác thực tài khoản để đăng nhập!",
                    VerifyEmailToken = token
                };
            } else
            {
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Sai tài khoản hoặc mật khẩu!",
                };
            }
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            _ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Token không hợp lệ!");
            return principal;
        }
        public async Task<AuthenticationResponseModel> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Yêu cầu không hợp lệ!"
                };
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid access token or refresh token!"
                };
            }

            string username = principal.Identity.Name;

            var user = await accountManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid access token or refresh token!"
                };
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await accountManager.UpdateAsync(user);

            return new AuthenticationResponseModel
            {
                Status = true,
                Message = "Refresh Token successfully!",
                JwtToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                Expired = newAccessToken.ValidTo,
                JwtRefreshToken = newRefreshToken
            };
        }

        //public async Task<ResponseModel> SignUpAdminAsync(SignUpModel model)
        //{
        //    var userExist = await accountManager.FindByNameAsync(model.Email);
        //    if (userExist == null)
        //    {
        //        var user = new Accounts
        //        {
        //            Name = model.Name,
        //            PhoneNumber = model.PhoneNumber,
        //            Address = model.Address,
        //            DateOfBirth = model.DateOfBirth,
        //            Email = model.Email,
        //            UserName = model.Email,
        //            Status = true
        //        };

        //        var result = await accountManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            if (!await roleManager.RoleExistsAsync(RoleModel.ADMIN.ToString()))
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(RoleModel.ADMIN.ToString()));
        //            }
        //            if (await roleManager.RoleExistsAsync(RoleModel.ADMIN.ToString()))
        //            {
        //                await accountManager.AddToRoleAsync(user, RoleModel.ADMIN.ToString());
        //            }
        //            //Tạo TK Admin tự động xác thực email
        //            var token = accountManager.GenerateEmailConfirmationTokenAsync(user);
        //            var confirm = await accountManager.ConfirmEmailAsync(user, token.Result);
        //            if(confirm.Succeeded)
        //            {
        //                return new ResponseModel { Status = "Success", Message = "Register Admin Successfully!" };
        //            }
        //            return new ResponseModel { Status = "Error", Message = "Cannot authentication Admin Account!" };
        //        }
        //        return new ResponseModel { Status = "Error", Message = "Cannot create account in database!" };
        //    }
        //    return new ResponseModel { Status = "Error", Message = "Username is already exist" };
        //}

        public async Task<ResponseModel> SignUpAsync(SignUpModel model)
        {
            var userExist = await accountManager.FindByNameAsync(model.Email);
            if (userExist == null)
            {
                var user = new Accounts
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    UserName = model.Email,
                    Status = true,
                    CreateDate = DateTime.Now,
                };

                var result = await accountManager.CreateAsync(user, model.Password);
                string errorMessage = null;
                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(RoleModel.USER.ToString()))
                    {
                        await roleManager.CreateAsync(new IdentityRole(RoleModel.USER.ToString()));
                    }
                    if (await roleManager.RoleExistsAsync(RoleModel.USER.ToString()))
                    {
                        await accountManager.AddToRoleAsync(user, RoleModel.USER.ToString());
                    }
                    var token = accountManager.GenerateEmailConfirmationTokenAsync(user);
                    return new ResponseModel { Status = "Success", Message = "Đăng ký tài khoản thành công, vui lòng kiểm tra email để xác thực tài khoản!", ConfirmEmailToken = token };
                }
                //If failed
                foreach (var error in result.Errors)
                {
                    errorMessage = error.Description;
                }
                return new ResponseModel { Status = "Error", Message = errorMessage };
            }
            return new ResponseModel { Status = "Error", Message = "Tài khoản đã tồn tại!" };

        }

        public async Task<ResponseModel> SignUpInternalAsync(SignUpModel model, RoleModel role)
        {
            var userExist = await accountManager.FindByNameAsync(model.Email);
            string errorMessage = null;
            if (userExist == null)
            {
                var user = new Accounts
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    UserName = model.Email,
                    Status = true
                };


                if (role.Equals(RoleModel.ADMIN) || role.Equals(RoleModel.STAFF) || role.Equals(RoleModel.USER))
                {
                    await accountManager.CreateAsync(user, model.Password);

                    if (!await roleManager.RoleExistsAsync(role.ToString()))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                    }
                    if (await roleManager.RoleExistsAsync(role.ToString()))
                    {
                        await accountManager.AddToRoleAsync(user, role.ToString());
                    }
                    //Tạo TK Admin tự động xác thực email
                    var token = accountManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirm = await accountManager.ConfirmEmailAsync(user, token.Result);
                    if (confirm.Succeeded)
                    {
                        return new ResponseModel { Status = "Success", Message = $"Đăng ký tài khoản {role} Thành công!" };
                    }

                    foreach (var error in confirm.Errors)
                    {
                        errorMessage = error.Description;
                    }
                    return new ResponseModel { Status = "Error", Message = errorMessage };
                }
                return new ResponseModel { Status = "Error", Message = $"Đăng ký thất bại, role {role} không hỗ trợ bởi hệ thống!" };
            }
            return new ResponseModel { Status = "Error", Message = "Tài khoản đã tồn tại!" };
        }

        public async Task<ResponseModel> ChangePassword(ChangePasswordModel model)
        {
            var account = await accountManager.FindByNameAsync(model.Email);
            if(account == null)
            {
                return new ResponseModel
                {
                    Status = "Error",
                    Message = "Không tìm thấy tài khoản!"
                };
            }
            var result = await accountManager.ChangePasswordAsync(account, model.CurrentPassword, model.NewPassword);

            string errorMessage = null;
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    errorMessage = error.Description;
                }

                return new ResponseModel
                {
                    Status = "Success",
                    Message = errorMessage
                };
            }
            return new ResponseModel { Status = "Success", Message = "Đổi mật khẩu thành công!" };
        }


        //public async Task<ResponseModel> SignUpStaffAsync(SignUpModel model)
        //{
        //    var userExist = await accountManager.FindByNameAsync(model.Email);
        //    if (userExist != null)
        //    {
        //        return new ResponseModel { Status = "Error", Message = "Username is already exist!" };
        //    }
        //    var user = new Accounts
        //    {
        //        Name = model.Name,
        //        PhoneNumber = model.PhoneNumber,
        //        Address = model.Address,
        //        DateOfBirth = model.DateOfBirth,
        //        Email = model.Email,
        //        UserName = model.Email,
        //        Status = true
        //    };
        //    var result = await accountManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        if (!await roleManager.RoleExistsAsync(RoleModel.STAFF.ToString()))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(RoleModel.STAFF.ToString()));
        //        }
        //        if (await roleManager.RoleExistsAsync(RoleModel.STAFF.ToString()))
        //        {
        //            await accountManager.AddToRoleAsync(user, RoleModel.STAFF.ToString());
        //        }
        //    }

        //    return new ResponseModel { Status = "Sucess", Message = "Register Staff successfully!" };
        //}
    }
}
