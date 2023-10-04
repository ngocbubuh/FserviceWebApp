using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.API.Repository.Inteface
{
    public interface IFloorRepository
    {
        public Task<List<Floor>> GetAllFloorsOfBuilding(int buidingId);
    }
}
