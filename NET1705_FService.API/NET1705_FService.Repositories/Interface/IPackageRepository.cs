using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.API.Repository.Inteface
{
    public interface IPackageRepository
    {
        public Task<PagedList<Package>> GetAllPackagesAsync(string search, string sort, PaginationParameter paginationParameter);
        public Task<Package> GetPackageAsync(int id);
        public Task<int> AddPackageAsync(Package package);
        public Task<int> UpdatePackageAsync(int id, Package package);
        public Task<int> DeletePackageAsync(int id);
    }
}
