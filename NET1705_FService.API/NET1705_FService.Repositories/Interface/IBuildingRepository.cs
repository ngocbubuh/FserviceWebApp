using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.API.Repository.Inteface
{
    public interface IBuildingRepository
    {
        public Task<List<Building>> GetAllBuildingAsync();
        //public Task<Building> GetBuildingAsync(int id);
        //public Task<int> AddBuildingAsync(Building building);
        //public Task<int> UpdateBuildingAsync(int id, Package package);
        //public Task<int> DeleteBuildingAsync(int id);
    }
}
