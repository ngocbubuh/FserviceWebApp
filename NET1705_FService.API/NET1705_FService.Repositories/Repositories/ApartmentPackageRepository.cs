using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Repositories
{
    public class ApartmentPackageRepository : IApartmentPackageRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public ApartmentPackageRepository(FserviceApiDatabaseContext context) 
        { 
            _context = context;
        }
        public async Task<int> AddApartmentPackageAsync(ApartmentPackage apartmentPackage)
        {
            if (_context == null)
            {
                return 0;
            }
            _context.Add(apartmentPackage);
            await _context.SaveChangesAsync();
            return apartmentPackage.Id;
        }

        public async Task<List<ApartmentPackage>> GetAllApartmentPackagesAsync()
        {
            var apartmentPackages = await _context.ApartmentPackages.Where(a => a.PackageStatus.Equals("Active")).ToListAsync();
            foreach (var apartmentPackage in apartmentPackages)
            {
                if (apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }
            return apartmentPackages;
        }

        public async Task<ApartmentPackage> GetApartmentPackageByIdAsync(int id)
        {
            var apartmentPackage = await _context.ApartmentPackages
                .Include(a => a.ApartmentPackageServices)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (apartmentPackage.EndDate < DateTime.Now.Date)
            {
                apartmentPackage.PackageStatus = "Expired";
                _context.Update(apartmentPackage);
                await _context.SaveChangesAsync();
            }

            return apartmentPackage;
        }

        public async Task<int> UpdateApartmentPackageAsync(int id, ApartmentPackage apartmentPackage)
        {
            if (id == apartmentPackage.Id)
            {
                _context.Update(apartmentPackage);
                await _context.SaveChangesAsync();
                return apartmentPackage.Id;
            }
            return 0;
        }

    }
}
