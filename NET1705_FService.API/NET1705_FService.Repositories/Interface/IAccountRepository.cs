using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public interface IAccountRepository
    {
        public Task<ResponseModel> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        public Task<ResponseModel> SignUpStaffAsync(SignUpModel model);
        public Task<ResponseModel> SignUpAdminAsync(SignUpModel model);
    }
}
