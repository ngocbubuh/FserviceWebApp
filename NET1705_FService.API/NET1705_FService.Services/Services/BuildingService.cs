using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _repo;

        public BuildingService(IBuildingRepository repo)
        {
            _repo = repo;
        }

        public async Task<ResponseModel> AddBuildingAsync(Building building)
        {
            var existBuilding = await _repo.GetBuildingAsync(building.Id);
            if (existBuilding != null)
            {
                return new ResponseModel { Status = "Error", Message = $"Building is already exist {building.Id}" };
            }
            var result = await _repo.AddBuildingAsync(building);
            if (result == 0)
            {
                return new ResponseModel { Status = "Error", Message = "Can not add a new building" };
            }
            return new ResponseModel { Status = "Success", Message = result.ToString() };
        }

        public async Task<ResponseModel> DeleteBuildingAsync(int id)
        {
            var delBuilding = await _repo.GetBuildingAsync(id);
            if (delBuilding == null)
            {
                return new ResponseModel { Status = "Error", Message = $"Not found building id {id}" };
            }
            var result = await _repo.DeleteBuildingAsync(id);
            if (result == 0)
            {
                return new ResponseModel { Status = "Error", Message = $"Can not delete building id {id}" };
            }
            return new ResponseModel { Status = "Success", Message = $"Delete successfully building {delBuilding.Name}" };
        }

        public Task<List<Building>> GetAllBuildingsAsync()
        {
            var buildings = _repo.GetAllBuildingAsync();
            return buildings;
        }

        public async Task<Building> GetBuildingByIdAsync(int id)
        {
            var building = await _repo.GetBuildingAsync(id);
            if (building == null)
            {
                return null;
            }
            return building;
        }

        public async Task<ResponseModel> UpdateBuildingAsync(int id, Building building)
        {
            if (id != building.Id)
            {
                return new ResponseModel { Status = "Error", Message = "Id was not match" };
            }
            var updateBuilding = await _repo.GetBuildingAsync(id);
            if (updateBuilding == null)
            {
                return new ResponseModel { Status = "Error", Message = $"Not found building id {id}" };
            }
            var result = await _repo.UpdateBuildingAsync(id, updateBuilding);
            if (result == 0) {
                return new ResponseModel { Status = "Error", Message = $"Can not update building id {id}" };
            }
            return new ResponseModel { Status = "Success", Message = result.ToString() };
        }
    }
}
