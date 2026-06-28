using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace VehicleExplorer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("getMakes")]
        public async Task<ResponseResult<List<MakeDto>>> GetMakes([FromQuery] string? search)
        {
            return await _vehicleService.GetMakesAsync(search);
        }

        [HttpGet("getVehicleTypesForMake/{makeId}")]
        public async Task<ResponseResult<List<VehicleTypeDto>>> GetVehicleTypesForMake(int makeId)
        {
            return await _vehicleService.GetVehicleTypesForMakeAsync(makeId);

        }

        [HttpGet("getModels")]
        public async Task<ResponseResult<List<ModelDto>>> GetModels( int makeId, int year,string vehicleType,int pageNumber , int pageSize )
        {
            return await _vehicleService.GetModelsAsync(makeId, year, vehicleType, pageNumber, pageSize);

            
        }
    }
}
