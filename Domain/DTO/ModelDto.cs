using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ModelDto
    {
        public int Make_ID { get; set; }
        public string Make_Name { get; set; } = string.Empty;
        public int Model_ID { get; set; }
        public string Model_Name { get; set; } = string.Empty;
    }
}
