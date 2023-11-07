using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.API.Repository.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly FserviceApiDatabaseContext _context;
        private readonly IMapper _mapper;

        public ApartmentRepository(FserviceApiDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Apartment> GetApartmentByIdAsync(int id)
        {
            var apartment = await _context.Apartments
                .Include(a => a.Type.Building)
                //.Include(a => a.Floor.Building)
                //.Include(a => a.ApartmentPackages)
                .SingleOrDefaultAsync(a => a.Id == id);
            return apartment;
        }

        public async Task<List<ApartmentModel>> GetApartmentsByUserNameAsync(string userName)
        {
            var apartments = _context.Apartments
                .Include(a => a.Account).Where(a => a.Account.UserName == userName)
                .Include(a => a.Type.Building)
                .AsQueryable();
            return apartments.ProjectTo<ApartmentModel>(_mapper.ConfigurationProvider).ToList();
        }

        public async Task<List<ApartmentModel>> GetApartmentsAsync(int? floorId, int? typeId, string? username)
        {
            if (floorId != null && typeId != null)
            {
                var apartments = _context.Apartments.Where(a => a.FloorId == floorId && a.TypeId == typeId).AsQueryable();
                return apartments.ProjectTo<ApartmentModel>(_mapper.ConfigurationProvider).ToList();
            }
            else if (!string.IsNullOrEmpty(username))
            {
                var apartments = _context.Apartments
                .Include(a => a.Account).Where(a => a.Account.UserName == username)
                .Include(a => a.Type.Building).AsQueryable();
                return apartments.ProjectTo<ApartmentModel>(_mapper.ConfigurationProvider).ToList();
            }
            return null;
        }

        public async Task<int> RegisApartmentAsync(int id, string accountId)
        {
            var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);
            apartment.AccountId = accountId;
            _context.Update(apartment);
            await _context.SaveChangesAsync();
            return apartment.Id;
        }
    }
}
