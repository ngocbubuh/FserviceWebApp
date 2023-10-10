using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NET1705_FService.Repositories.Helper;
using System.Text.RegularExpressions;

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
            package.UnsignName = StringExtensions.ConvertToUnSign(package.Name);
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

        public async Task<PagedList<Package>> GetAllPackagesAsync(PaginationParameter paginationParameter)
        {
            var allPackages = _context.Packages.Where(y => y.Status == true).AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                allPackages = allPackages.Where(p => p.Name.Contains(paginationParameter.Search) || p.UnsignName.Contains(paginationParameter.Search));
            }

            if (!string.IsNullOrEmpty(paginationParameter.Sort))
            {
                switch (paginationParameter.Sort)
                {
                    case "price_asc":
                        allPackages = allPackages.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        allPackages = allPackages.OrderByDescending(p => p.Price);
                        break;
                    case "name_asc":
                        allPackages = allPackages.OrderBy(p => p.Name);
                        break;
                    case "name_desc":
                        allPackages = allPackages.OrderByDescending(p => p.Name);
                        break;
                }
            }

            var packages = await allPackages.ToListAsync();

            return PagedList<Package>.ToPagedList(packages,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
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
