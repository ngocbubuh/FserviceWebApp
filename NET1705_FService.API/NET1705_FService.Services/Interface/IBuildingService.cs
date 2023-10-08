using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Inteface
{
    public interface IBuildingService
    {
        public Task<List<Building>> GetAllBuildingsAsync();

        public Task<Building> GetBuildingByIdAsync(int id);

        public Task<ResponseModel> AddBuildingAsync(Building building);

        public Task<ResponseModel> UpdateBuildingAsync(int id, Building building);

        public Task<ResponseModel> DeleteBuildingAsync(int id);
    }
}
