using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
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
        private readonly IMapper _mapper;

        public UserRepository(FserviceApiDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                if (!deleteAcc.EmailConfirmed)
                {
                    _context.Accounts.Remove(deleteAcc);
                    await _context.SaveChangesAsync();
                    return deleteAcc.Id;
                }
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

        public async Task<AccountsModel> GetAccountByUsernameAsync(string UserName)
        {
            var acc = await _context.Accounts
                .SingleOrDefaultAsync(a => a.UserName == UserName && a.Status == true);
            var showAcc = _mapper.Map<AccountsModel>(acc);
            return showAcc;
        }

        public async Task<PagedList<Accounts>> GetAllAccountAsync(PaginationParameter paginationParameter)
        {
            var allAccounts = _context.Accounts.Where(y => y.Status == true).AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                allAccounts = allAccounts.Where(p => p.Email.Contains(paginationParameter.Search));
            }

            if (!string.IsNullOrEmpty(paginationParameter.Sort))
            {
                switch (paginationParameter.Sort)
                {
                    case "email_asc":
                        allAccounts = allAccounts.OrderBy(p => p.Email);
                        break;
                    case "email_desc":
                        allAccounts = allAccounts.OrderByDescending(p => p.Email);
                        break;
                    case "name_asc":
                        allAccounts = allAccounts.OrderBy(p => p.Name);
                        break;
                    case "name_desc":
                        allAccounts = allAccounts.OrderByDescending(p => p.Name);
                        break;
                }
            }

            var acc = await allAccounts.ToListAsync();
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
