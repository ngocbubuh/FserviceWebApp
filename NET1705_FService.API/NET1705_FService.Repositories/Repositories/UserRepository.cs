using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public UserRepository(FserviceApiDatabaseContext context)
        {
            _context = context;
        }

        public async Task<string> DeleteAccountAsync(string id)
        {
            var deleteAcc = _context.Accounts!.SingleOrDefault(x => x.Id == id);
            if (deleteAcc != null)
            {
                //_context.Accounts.Update(new Accounts
                //{
                //    Id = deleteAcc.Id,
                //    Name = deleteAcc.Name,
                //    PhoneNumber = deleteAcc.PhoneNumber,
                //    Email = deleteAcc.Email,
                //    Address = deleteAcc.Address,
                //    DateOfBirth = deleteAcc.DateOfBirth,
                //    Avatar = deleteAcc.Avatar,
                //    Status = false
                //});
                deleteAcc.Status = false;
                _context.Accounts.Update(deleteAcc);
                await _context.SaveChangesAsync();
                return deleteAcc.Id;
            }
            return string.Empty;
        }

        public async Task<Accounts> GetAccountAsync(string id)
        {
            var acc = await _context.Accounts
                .FirstOrDefaultAsync(p => p.Id == id && p.Status == true);
            return acc;
        }

        public async Task<PagedList<Accounts>> GetAllAccountAsync(PaginationParameter paginationParameter)
        {
            var acc = await _context.Accounts!.Where(p => p.Status == true)
                .OrderBy(p => p.Id).ToListAsync();
            return PagedList<Accounts>.ToPagedList(acc,
                paginationParameter.PageNumber,
                paginationParameter.PageSize); ;
        }

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
