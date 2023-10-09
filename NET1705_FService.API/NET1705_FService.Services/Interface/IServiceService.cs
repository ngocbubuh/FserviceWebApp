using NET1705_FService.Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Inteface
{
    public interface IServiceService
    {
        public Task<PagedList<NET1705_FService.Repositories.Models.Service>> GetAllServicesAsync(PaginationParameter paginationParameter);
        public Task<NET1705_FService.Repositories.Models.Service> GetServiceAsync(int id);
        public Task<ResponseModel> AddServiceAsync(NET1705_FService.Repositories.Models.Service service);
        public Task<ResponseModel> UpdateServiceAsync(int id, NET1705_FService.Repositories.Models.Service service);
        public Task<ResponseModel> DeleteServiceAsync(int id);
    }
}
