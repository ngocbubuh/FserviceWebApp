using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Inteface
{
    public interface IFloorService
    {
        public Task<List<Floor>> GetFloorsByBuildingId(int buildingId);
    }
}
