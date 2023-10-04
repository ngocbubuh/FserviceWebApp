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
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _repo;

        public FloorService(IFloorRepository repo)
        {
            _repo = repo;
        }
        public Task<List<Floor>> GetFloorsByBuildingId(int buildingId)
        {
            var floors = _repo.GetAllFloorsOfBuilding(buildingId);
            return floors;
        }
    }
}
