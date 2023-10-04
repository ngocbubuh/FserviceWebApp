using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public interface IBannerRepository
    {
        public Task<Banner> GetBannerByPageAsync(string page);
        public Task<Banner> GetBannerAsync(int id);
        public Task<int> AddBannerAsync(Banner banner);
        public Task<int> DeleteBannerAsync(int id);
        public Task<int> UpdateBannerAsync(int id, Banner banner);
    }
}
