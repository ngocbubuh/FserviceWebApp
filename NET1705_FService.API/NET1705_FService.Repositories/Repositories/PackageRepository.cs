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
    public class PackageRepository : IPackageRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public PackageRepository(FserviceApiDatabaseContext context)
        {
            _context = context;
        }
        public async Task<int> AddPackageAsync(Package package)
        {
            if (_context.Packages == null)
            {
                return 0;
            }
            package.Status = true;
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
            return package.Id;
        }

        public async Task<int> DeletePackageAsync(int id)
        {
            var deletePackage = _context.Packages!.SingleOrDefault(x => x.Id == id && x.Status == true);
            if (deletePackage != null)
            {
                deletePackage.Status = false;
                _context.Packages.Update(deletePackage);
                await _context.SaveChangesAsync();
                return deletePackage.Id;
            }
            return 0;
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            var packages = await _context.Packages!
                .Where(p => p.Status == true)
                .ToListAsync();
            return packages;
        }

        public async Task<Package> GetPackageAsync(int id)
        {
            var package = await _context.Packages
                .Include(p => p.PackageDetails)
                .FirstOrDefaultAsync(p => p.Id == id && p.Status == true);
            return package;
        }

        public async Task<int> UpdatePackageAsync(int id, Package package)
        {
            if (id == package.Id)
            {
                _context.Packages!.Update(package);
                await _context.SaveChangesAsync();
                return package.Id;
            }
            return 0;
        }
    }
}
