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

        public Task<List<Building>> GetAllBuildings()
        {
            var buildings = _repo.GetAllBuildingAsync();
            return buildings;
        }
    }
}
