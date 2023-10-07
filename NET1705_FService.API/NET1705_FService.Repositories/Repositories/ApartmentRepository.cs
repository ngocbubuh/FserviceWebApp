using Microsoft.EntityFrameworkCore;
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

        public ApartmentRepository(FserviceApiDatabaseContext context)
        {
            _context = context;
        }
        public async Task<Apartment> GetApartmentByIdAsync(int id)
        {
            var apartment = await _context.Apartments
                .Include(a => a.Floor)
                .Include(a => a.ApartmentPackages)
                .SingleOrDefaultAsync(a => a.Id == id);
            return apartment;
        }

        public async Task<List<Apartment>> GetApartmentsOnFloorAsync(int floorId, int typeId)
        {
            var apartments = await _context.Apartments.Where(a => a.FloorId == floorId && a.TypeId == typeId).ToListAsync();
            return apartments;
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
