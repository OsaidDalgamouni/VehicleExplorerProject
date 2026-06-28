using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ResponseResult<List<MakeDto>>> GetMakesAsync(string? search);
        Task<ResponseResult<List<VehicleTypeDto>>> GetVehicleTypesForMakeAsync(int makeId);

        Task<ResponseResult<List<ModelDto>>> GetModelsAsync(int makeId, int year, string vehicleType, int pageNumber , int pageSize );

    }
}
