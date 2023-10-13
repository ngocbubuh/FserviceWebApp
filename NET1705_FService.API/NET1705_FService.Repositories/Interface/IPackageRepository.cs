using NET1705_FService.Repositories.Helper;
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
        public Task<PagedList<Package>> GetAllPackagesAsync(PaginationParameter paginationParameter);
        public Task<Package> GetPackageAsync(int id, int? typeId);
        public Task<int> AddPackageAsync(Package package);
        public Task<int> UpdatePackageAsync(int id, Package package);
        public Task<int> DeletePackageAsync(int id);

        public Task<double> GetPricePackageByTypeIdAsync(int packageId, int typeId);
    }
}
