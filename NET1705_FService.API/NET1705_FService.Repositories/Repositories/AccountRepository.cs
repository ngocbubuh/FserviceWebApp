using FServiceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        public AccountRepository(UserManager<Accounts> accountManager, SignInManager<Accounts> signInManager, IConfiguration configuration)
        {
            this.accountManager = accountManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [Authorize]
        public async Task<string> DeleteAccountAsync(string id)
        {
            var deleteAcc = _context.Accounts!.SingleOrDefault(x => x.Id == id);
            if (deleteAcc != null)
            {
                _context.Accounts.Remove(deleteAcc);
                await _context.SaveChangesAsync();
                return deleteAcc.Id;
            }
            return string.Empty;
        }

        [Authorize]
        public async Task<Accounts> GetAccountAsync(string id)
        {
            var acc = await _context.Accounts
                .FirstOrDefaultAsync(p => p.Id == id);
            return acc;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if(!result.Succeeded) 
            {
                return String.Empty;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new Accounts
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                Role = model.Role,
                Email = model.Email,
                UserName = model.Email,
            };
            return await accountManager.CreateAsync(user, model.Password);
        }

        [Authorize]
        public async Task<string> UpdateAccountAsync(string id, Accounts account)
        {
            if (id == account.Id)
            {
                _context.Accounts!.Update(account);
                await _context.SaveChangesAsync();
                return account.Id;
            }
            return string.Empty;
        }
    }
}
