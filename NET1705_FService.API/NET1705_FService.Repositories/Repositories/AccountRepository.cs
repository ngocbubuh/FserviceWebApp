using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            RoleManager<IdentityRole> roleManager)
        {
            this.accountManager = accountManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
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

            return staffAccounts.ToList();
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded) 
            {
                return "Error! Incorrect Username or Password!";
            }

            var account = await accountManager.FindByNameAsync(model.Email);
            var roles = await accountManager.GetRolesAsync(account);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResponseModel> SignUpAdminAsync(SignUpModel model)
        {
            var userExist = await accountManager.FindByNameAsync(model.Email);
            if (userExist != null)
            {
                return new ResponseModel { Status = "Error", Message = "Username is already exist" };
            }

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

            var result = await accountManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(RoleModel.ADMIN.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleModel.ADMIN.ToString()));
                }
                if (await roleManager.RoleExistsAsync(RoleModel.ADMIN.ToString()))
                {
                    await accountManager.AddToRoleAsync(user, RoleModel.ADMIN.ToString());
                }
            }
            return new ResponseModel { Status = "Success", Message = "Register Admin Successfully!" };

        }

        public async Task<ResponseModel> SignUpAsync(SignUpModel model)
        {
            var userExist = await accountManager.FindByNameAsync(model.Email);
            if (userExist != null)
            {
                return new ResponseModel { Status = "Error", Message = "Username is already exist!" };
            }
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

            var userMail = new Accounts
            {
                Name = model.Name,
                Email = model.Email,
            };


            var result = await accountManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(RoleModel.USER.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleModel.USER.ToString()));
                }
                if (await roleManager.RoleExistsAsync(RoleModel.USER.ToString()))
                {
                    await accountManager.AddToRoleAsync(user, RoleModel.USER.ToString());
                }
            }

            return new ResponseModel { Status = "Sucess", Message = "Register successfully!" };
        }

        public async Task<ResponseModel> SignUpStaffAsync(SignUpModel model)
        {
            var userExist = await accountManager.FindByNameAsync(model.Email);
            if (userExist != null)
            {
                return new ResponseModel { Status = "Error", Message = "Username is already exist!" };
            }
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
            var result = await accountManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(RoleModel.STAFF.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleModel.STAFF.ToString()));
                }
                if (await roleManager.RoleExistsAsync(RoleModel.STAFF.ToString()))
                {
                    await accountManager.AddToRoleAsync(user, RoleModel.STAFF.ToString());
                }
            }

            return new ResponseModel { Status = "Sucess", Message = "Register successfully!" };
        }
    }
}
