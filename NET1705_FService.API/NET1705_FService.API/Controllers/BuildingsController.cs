using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1705_FService.API.Helper;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using System.Text.RegularExpressions;

namespace NET1715_FService.API.Controllers
{
    [Route("api/buildings")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBuildings()
        {
            try
            {
                var buildings = await _buildingService.GetAllBuildingsAsync();
                if (!buildings.Any())
                {
                    return NotFound();
                }
                return Ok(buildings);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBuildingById(int id) 
        { 
            try
            {
                var building = await _buildingService.GetBuildingByIdAsync(id);
                return building != null ? Ok(building) : NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddBuildingAsync(Building building)
        {
            try
            {
                if (Validation.GetBuildingName(building.Name) == false)
                {
                    ResponseModel resp = new ResponseModel { Status = "Error", Message = "Building name must be in the form Sxxx" };
                    return BadRequest(building.Name);
                }

                var result = await _buildingService.AddBuildingAsync(building);
                if (result.Status.Equals("Error"))
                {
                    return NotFound(result);
                }
                var addBuilding = await _buildingService.GetBuildingByIdAsync(int.Parse(result.Message));
                return Ok(addBuilding);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateBuildingAsync(int id, Building building)
        {
            try
            {
                if (Validation.GetBuildingName(building.Name) == false)
                {
                    ResponseModel resp = new ResponseModel { Status = "Error", Message = "Building name must be in the form Sxxx" };
                    return BadRequest(resp);
                }
                var result = await _buildingService.UpdateBuildingAsync(id, building);
                if (result.Status.Equals("Error"))
                {
                    return NotFound(result);
                }
                var updateBuilding = await _buildingService.GetBuildingByIdAsync(int.Parse(result.Message));
                return Ok(updateBuilding);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteBuildingAsync(int id)
        {
            try
            {
                var result = await _buildingService.DeleteBuildingAsync(id);
                if (result.Status.Equals("Error"))
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
