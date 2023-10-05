using FServiceAPI.Repositories;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _repo;

        public BannerService(IBannerRepository repo)
        {
            _repo = repo;
        }
        public async Task<ResponseModel> AddBannerAsync(Banner banner)
        {
            var result = await _repo.AddBannerAsync(banner);
            if (result != 0)
            {
                return new ResponseModel { Status = "Success", Message = result.ToString() };
            }
            return new ResponseModel { Status = "Error", Message = "Error! Try again." };
        }

        public async Task<ResponseModel> DeleteBannerAsync(int id)
        {
            var deleteBanner = await _repo.GetBannerAsync(id);
            if (deleteBanner == null)
            {
                return new ResponseModel { Status = "Error", Message = $"Not found Banner Id {id}" };
            }
            var result = await _repo.DeleteBannerAsync(id);
            if (result == 0)
            {
                return new ResponseModel { Status = "Error", Message = $"Can not delete banner: ID = {deleteBanner.Id}" };
            }
            return new ResponseModel { Status = "Success", Message = $"Delete successfully banner: ID = {deleteBanner.Id}" };
        }

        public async Task<Banner> GetBannerById(int id)
        {
            var package = await _repo.GetBannerAsync(id);
            return package;
        }

        public async Task<List<Banner>> GetBannerByPage(string page)
        {
            var banners = await _repo.GetBannerByPageAsync(page);
            return banners;
        }

        public async Task<ResponseModel> UpdateBannerAsync(int id, Banner banner)
        {
            if (id == banner.Id)
            {
                var result = await _repo.UpdateBannerAsync(id, banner);
                if (result != 0)
                {
                    return new ResponseModel { Status = "Success", Message = result.ToString() };
                }
                return new ResponseModel { Status = "Error", Message = "Update error." };
            }
            return new ResponseModel { Status = "Error", Message = "Id invalid" };
        }
    }
}
