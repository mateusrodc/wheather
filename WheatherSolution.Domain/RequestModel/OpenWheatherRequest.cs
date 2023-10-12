using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheatherSolution.Domain.RequestModel
{
    public class OpenWheatherRequest
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
