using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;

namespace NET1715_FService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBannerAsync(string page)
        {
            try
            {
                var banner = await _bannerService.GetBannerByPage(page);
                return banner != null ? Ok(banner) : NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBannerAsync(Banner newBanner)
        {
            try
            {
                var result = await _bannerService.AddBannerAsync(newBanner);
                if (result.Status.Equals("Success"))
                {
                    var banner = await _bannerService.GetBannerById(int.Parse(result.Message));
                    return Ok(banner);
                }
                return BadRequest(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBannerAsync(int id, Banner updateBanner)
        {
            try
            {
                var result = await _bannerService.UpdateBannerAsync(id,updateBanner);
                if (result.Status.Equals("Success"))
                {
                    var banner = await _bannerService.GetBannerById(int.Parse(result.Message));
                    return Ok(banner);
                }
                return NotFound(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackageAsync(int id)
        {
            try
            {
                var result = await _bannerService.DeleteBannerAsync(id);
                if (result.Status.Equals("Success"))
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
