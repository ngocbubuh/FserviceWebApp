using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;

namespace NET1715_FService.Service.Inteface
{
    public interface IBannerService
    {
        public Task<Banner> GetBannerByPage(string page);
        public Task<Banner> GetBannerById(int id);
        public Task<ResponseModel> AddBannerAsync(Banner banner);
        public Task<ResponseModel> UpdateBannerAsync(int id, Banner banner);
        public Task<ResponseModel> DeleteBannerAsync(int id);
    }
}
