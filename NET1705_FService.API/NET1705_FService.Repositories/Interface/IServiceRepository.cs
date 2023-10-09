using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public interface IServiceRepository
    {
        public Task<PagedList<Service>> GetAllServiceAsync(PaginationParameter paginationParameter);
        public Task<Service> GetServiceAsync(int id);
        public Task<int> AddServiceAsync(Service service);
        public Task<int> UpdateServiceAsync(int id, Service service);
        public Task<int> DeleteServiceAsync(int id);
    }
}
