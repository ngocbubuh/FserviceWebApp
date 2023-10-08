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
    public class BuildingRepository : IBuildingRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public BuildingRepository(FserviceApiDatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AddBuildingAsync(Building building)
        {
            if (_context == null){
                return 0;
            }
            _context.Add(building);
            await _context.SaveChangesAsync();
            return building.Id;
        }

        public async Task<int> DeleteBuildingAsync(int id)
        {
            var delBuilding = _context.Buildings!.SingleOrDefault(b => b.Id == id && b.Status == true);
            delBuilding.Status = false;
            _context.Update(delBuilding);
            await _context.SaveChangesAsync();
            return delBuilding.Id;
        }

        public async Task<List<Building>> GetAllBuildingAsync()
        {
            var buildings = await _context.Buildings
                .Where(b => b.Status == true)
                .ToListAsync();
            return buildings;
        }

        public async Task<Building> GetBuildingAsync(int id)
        {
            var building = _context.Buildings!.SingleOrDefault(b => b.Id == id && b.Status == true);
            return building;
        }

        public async Task<int> UpdateBuildingAsync(int id, Building building)
        {
            _context.Update(building);
            await _context.SaveChangesAsync();
            return building.Id;
        }
    }
}
