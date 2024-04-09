﻿using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Inteface
{
    public interface IAccountService
    {
        public Task<ResponseModel> SignUpAsync(SignUpModel model);
        public Task<AuthenticationResponseModel> SignInAsync(SignInModel model);
        //public Task<ResponseModel> SignUpStaffAsync(SignUpModel model);
        //public Task<ResponseModel> SignUpAdminAsync(SignUpModel model);
        public Task<ResponseModel> SignUpInternalAsync(SignUpModel model, RoleModel role);
        public Task<ResponseModel> ConfirmEmail(string token, string email);
        public Task<ResponseModel> ChangePassword(ChangePasswordModel model);
        public Task<AuthenticationResponseModel> RefreshToken(TokenModel tokenModel);
        public Task<bool> UpdateDeviceToken(string accountId, string deviceToken);
    }
}
