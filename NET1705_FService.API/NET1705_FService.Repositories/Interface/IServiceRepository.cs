using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public interface IServiceRepository
    {
        public Task<List<Service>> GetAllServiceAsync();
        public Task<Service> GetServiceAsync(int id);
        public Task<int> AddServiceAsync(Service service);
        public Task<int> UpdateServiceAsync(int id, Service service);
        public Task<int> DeleteServiceAsync(int id);
    }
}
