using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
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
                //Delete mềm, ko xóa khỏi database
                //dbContext.Banners.Update(new Banner
                //{
                //    Id = deleteBanner.Id,
                //    Title = deleteBanner.Title,
                //    Description = deleteBanner.Description,
                //    Image = deleteBanner.Image,
                //    Page = deleteBanner.Page,
                //    Status = false
                //});
                deleteBanner.Status = false;
                dbContext.Banners.Update(deleteBanner);
                await dbContext.SaveChangesAsync();
                return deleteBanner.Id;
            }
            return 0;
        }

        public async Task<Banner> GetBannerAsync(int id)
        {
            //Get status == true
            var banner = await dbContext.Banners
                .FirstOrDefaultAsync(b => b.Id == id && b.Status == true);
            return banner;
        }

        public async Task<List<Banner>> GetBannerByPageAsync(string page)
        {
            //Get status == true
            var banners = await dbContext.Banners!.Where(b => b.Status == true && b.Page == page).ToListAsync();
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
