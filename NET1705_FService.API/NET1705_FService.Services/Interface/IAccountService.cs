using Microsoft.AspNetCore.Identity;
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
        public Task<ResponseModel> SignUpStaffAsync(SignUpModel model);
        public Task<ResponseModel> SignUpAdminAsync(SignUpModel model);
    }
}
