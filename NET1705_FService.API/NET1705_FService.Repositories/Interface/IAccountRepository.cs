using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public interface IAccountRepository
    {
        public Task<ResponseModel> SignUpAsync(SignUpModel model);
        public Task<AuthenticationResponseModel> SignInAsync(SignInModel model);
        //public Task<ResponseModel> SignUpStaffAsync(SignUpModel model);
        //public Task<ResponseModel> SignUpAdminAsync(SignUpModel model);
        public Task<ResponseModel> SignUpInternalAsync(SignUpModel model, RoleModel role);
        public Task<ResponseModel> ConfirmEmail(string token, string email);
        public Task<AuthenticationResponseModel> RefreshToken(TokenModel tokenModel);

        //Báo quá báo
        public Task<List<Accounts>> GetAllStaffsAsync();

        public Task<Accounts> GetAccountByUserName(string userName);
    } 
}
