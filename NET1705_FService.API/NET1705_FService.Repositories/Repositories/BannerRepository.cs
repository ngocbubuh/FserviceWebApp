using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly FserviceApiDatabaseContext dbContext;

        public BannerRepository(FserviceApiDatabaseContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddBannerAsync(Banner banner)
        {
            if (dbContext.Banners == null)
            {
                return 0;
            }
            dbContext.Banners.Add(banner);
            await dbContext.SaveChangesAsync();
            return banner.Id;
        }

        public async Task<int> DeleteBannerAsync(int id)
        {
            var deleteBanner = dbContext.Banners!.SingleOrDefault(x => x.Id == id);
            if (deleteBanner != null)
            {
                dbContext.Banners.Remove(deleteBanner);
                await dbContext.SaveChangesAsync();
                return deleteBanner.Id;
            }
            return 0;
        }

        public async Task<Banner> GetBannerAsync(int id)
        {
            var banner = await dbContext.Banners
                .FirstOrDefaultAsync(b => b.Id == id);
            return banner;
        }

        public async Task<Banner> GetBannerByPageAsync(string page)
        {
            var banners = await dbContext.Banners.FirstOrDefaultAsync(b => b.Page == page);
            return banners; ;
        }

        public async Task<int> UpdateBannerAsync(int id, Banner banner)
        {
            if (id == banner.Id)
            {
                dbContext.Banners!.Update(banner);
                await dbContext.SaveChangesAsync();
                return banner.Id;
            }
            return 0;
        }
    }
}
