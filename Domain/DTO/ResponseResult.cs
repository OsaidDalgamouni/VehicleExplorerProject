using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ResponseResult<T>
    {

        public int TotalCount { get; set; }
        public T? Data { get; set; }
        public ResultStatus Status { get; set; }
        public List<string>? Errors { get; set; }
    }
}
