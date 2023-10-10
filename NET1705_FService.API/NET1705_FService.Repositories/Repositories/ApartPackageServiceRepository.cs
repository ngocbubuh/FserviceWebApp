using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Repositories
{
    public class ApartPackageServiceRepository : IApartPackageServiceRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public ApartPackageServiceRepository(FserviceApiDatabaseContext context) 
        { 
            _context = context;
        }

        public async Task<int> AddApartPackageServiceAsync(ApartmentPackageService apartmentPackageService)
        {
            if (_context == null)
            {
                return 0;
            }
            _context.Add(apartmentPackageService);
            await _context.SaveChangesAsync();
            return apartmentPackageService.Id;
        }

        public async Task<int> UpdateApartPackageServiceAsync(int id, ApartmentPackageService apartmentPackageService)
        {
            if (_context == null)
            {
                return 0;
            }
            _context.Update(apartmentPackageService);
            await _context.SaveChangesAsync();
            return apartmentPackageService.Id;
        }
    }
}
