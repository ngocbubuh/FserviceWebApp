using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Helper;
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

        public async Task<PagedList<ApartmentPackage>> GetAllApartmentPackagesAsync(PaginationParameter paginationParameter)
        {
            //var apartmentPackages = await _context.ApartmentPackages.Where(a => a.PackageStatus.Equals("Active")).ToListAsync();
            var apartmentPackages = await _context.ApartmentPackages.ToListAsync();
            foreach (var apartmentPackage in apartmentPackages)
            {
                if (apartmentPackage.PackageStatus == "Active" && apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }
            return PagedList<ApartmentPackage>.ToPagedList(apartmentPackages,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<ApartmentPackage> GetApartmentPackageByIdAsync(int id)
        {
            var apartmentPackage = await _context.ApartmentPackages
                .Include(a => a.ApartmentPackageServices).ThenInclude(ad => ad.Service)
                .Include(a => a.Apartment)
                .Include(a => a.Package)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (apartmentPackage != null)
            {
                if (apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }

            return apartmentPackage;
        }

        // check before buy new package
        public async Task<bool> CheckApartmentPackagesByApartmentAndPackage(int apartmentId, int packageId)
        {
            var apartmentPackages = await _context.ApartmentPackages
                .Where(a => a.ApartmentId == apartmentId)
                .ToListAsync();
            foreach (var apartmentPackage in apartmentPackages)
            {
                if (apartmentPackage.PackageStatus == "Active"
                    && apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }
            var activePackages = apartmentPackages.Where(a => a.PackageId == packageId && a.PackageStatus == "Active");
            if (activePackages.Any())
            {
                return false;
            }
            return true;
        }

        public async Task<PagedList<ApartmentPackage>> GetApartmentPackagesByApartmentId(PaginationParameter paginationParameter, int apartmentId)
        {
            var apartmentPackages = await _context.ApartmentPackages
                .Where(a => a.ApartmentId == apartmentId)
                //.Include(a => a.ApartmentPackageServices)
                .Include(a => a.Package)
                .ToListAsync();
            foreach (var apartmentPackage in apartmentPackages)
            {
                if (apartmentPackage.PackageStatus == "Active"
                    && apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }
            return PagedList<ApartmentPackage>.ToPagedList(apartmentPackages,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
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

        public async Task<bool> DeleteApartmentPackage(int orderId)
        {
            var apartmentPackage = _context.ApartmentPackages
                .Include(a => a.ApartmentPackageServices)
                .FirstOrDefault(a => a.OrderId == orderId && a.PackageStatus == "Disable");
            if (apartmentPackage != null)
            {
                _context.ApartmentPackages.Remove(apartmentPackage);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
