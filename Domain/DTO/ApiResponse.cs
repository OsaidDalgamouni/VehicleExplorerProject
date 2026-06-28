using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ApiResponse<T>
    {
        public int Count { get; set; }

        public string? Message { get; set; }

        public string? SearchCriteria { get; set; }

        public List<T> Results { get; set; } = [];
    }
}
