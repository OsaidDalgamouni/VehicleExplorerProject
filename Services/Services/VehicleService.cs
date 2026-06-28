using Domain.DTO;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Domain.Enums;

namespace Services.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public VehicleService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }
        public async Task<ResponseResult<List<MakeDto>>> GetMakesAsync(string? search = null)
        {
            try
            {
                const string cacheKey = "vehicle-makes";

                if (!_cache.TryGetValue(cacheKey, out List<MakeDto>? makes))
                {
                    var response = await GetAsync<MakeDto>("getallmakes?format=json");

                    if (response.Status == ResultStatus.Failed)
                    {
                        return new ResponseResult<List<MakeDto>>
                        {
                            Status = ResultStatus.Failed,
                            Errors = response.Errors
                        };
                    }

                    makes = response.Data?.Results;

                    if (makes == null || !makes.Any())
                    {
                        return new ResponseResult<List<MakeDto>>
                        {
                            Status = ResultStatus.SuccessWithWaringn,
                            Data = null,
                            TotalCount = 0,
                            Errors = new List<string> { "No vehicle makes were returned from the API." }
                        };
                    }

                    _cache.Set(
                        cacheKey,
                        makes,
                        new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                        });
                }

                List<MakeDto> result = string.IsNullOrWhiteSpace(search)
                    ? makes.Take(50).ToList()
                    : makes
                        .Where(x => !string.IsNullOrWhiteSpace(x.Make_Name) &&
                                    x.Make_Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                        .Take(20)
                        .ToList();

                return new ResponseResult<List<MakeDto>>
                {
                    Status = ResultStatus.Success,
                    Data = result,
                    TotalCount = result.Count
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<List<MakeDto>>
                {
                    Status = ResultStatus.Failed,
                    Data = null,
                    Errors = new List<string> { ex.Message + "/" + ex?.InnerException + "/" + ex?.StackTrace }

                };
            }
          
        }
        public async Task<ResponseResult<List<VehicleTypeDto>>> GetVehicleTypesForMakeAsync(int makeId)
        {
            try
            {
                List<VehicleTypeDto> vehicleTypes = new List<VehicleTypeDto>();

                var response = await GetAsync<VehicleTypeDto>($"GetVehicleTypesForMakeId/{makeId}?format=json");

                if (response.Status == ResultStatus.Failed)
                {
                    return new ResponseResult<List<VehicleTypeDto>>
                    {
                        Status = ResultStatus.Failed,
                        Errors = response.Errors
                    };
                }

                vehicleTypes = response.Data?.Results;

                if (vehicleTypes == null || !vehicleTypes.Any())
                {
                    return new ResponseResult<List<VehicleTypeDto>>
                    {
                        Status = ResultStatus.SuccessWithWaringn,
                        Data = null,
                        TotalCount = 0,
                        Errors = new List<string> { "No vehicle types were returned from the API." }
                    };
                }

                return new ResponseResult<List<VehicleTypeDto>> { Status = ResultStatus.Success, Data = vehicleTypes ,TotalCount= vehicleTypes.Count() };

            }
            catch (Exception ex)
            {
                return new ResponseResult<List<VehicleTypeDto>>
                {
                    Status = ResultStatus.Failed,
                    Data = null,
                    Errors = new List<string> { ex.Message + "/" + ex?.InnerException + "/" + ex?.StackTrace }

                };
            }

          
        }
        public async Task<ResponseResult<List<ModelDto>>> GetModelsAsync( int makeId,int year, string vehicleType,  int pageNumber, int pageSize)
        {
            try
            {
                string encodedVehicleType = Uri.EscapeDataString(vehicleType);

                var response = await GetAsync<ModelDto>(
                    $"GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}/vehicletype/{encodedVehicleType}?format=json");

                if (response.Status != ResultStatus.Success)
                {
                    return new ResponseResult<List<ModelDto>>
                    {
                        Status = ResultStatus.Failed,
                        Errors = response.Errors
                    };
                }

                var models = response.Data?.Results ?? new List<ModelDto>();

                if (!models.Any())
                {
                    return new ResponseResult<List<ModelDto>>
                    {
                        Status = ResultStatus.SuccessWithWaringn,
                        TotalCount = 0,
                        Data = null,
                        Errors = new List<string> { "No vehicle models were returned from the API." }
                    };
                }

                return new ResponseResult<List<ModelDto>>
                {
                    Status = ResultStatus.Success,
                    TotalCount = models.Count,
                    Data = models
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList()
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<List<ModelDto>>
                {
                    Status = ResultStatus.Failed,
                    Data = null,
                    Errors = new List<string> { ex.Message + "/" + ex?.InnerException + "/" + ex?.StackTrace }

                };
            }
           
        }
        private async Task<ResponseResult<ApiResponse<T>>> GetAsync<T>(string endpoint)
        {
            try
            {
                var httpResponse = await _httpClient.GetAsync(endpoint);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadAsStringAsync();

                    return new ResponseResult<ApiResponse<T>>
                    {
                        Status = ResultStatus.Failed,
                        Errors = new List<string>  {  $"GET '{endpoint}' failed.", $"Status Code: {(int)httpResponse.StatusCode} ({httpResponse.StatusCode})",
                                  string.IsNullOrWhiteSpace(errorContent)? "The server returned an empty error response." : errorContent }
                    };
                }

                var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<T>>();

                return new ResponseResult<ApiResponse<T>>
                {
                    Status = ResultStatus.Success,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult<ApiResponse<T>>
                {
                    Status = ResultStatus.Failed,
                    Errors = new List<string> {$"An unexpected error occurred: {ex.Message}"}
                };
            }
        }
    }
}
