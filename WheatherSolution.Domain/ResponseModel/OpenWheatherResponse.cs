using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheatherSolution.Domain.ResponseModel
{
    public class OpenWheatherResponse
    {
        [JsonProperty("coord")]
        public Coord Coordinates { get; set; }

        [JsonProperty("main")]
        public MainData Main { get; set; }

        [JsonProperty("name")]
        public string City { get; set; }
    }
    public class Coord
    {
        [JsonProperty("lat")]
        public decimal Latitude { get; set; }

        [JsonProperty("lon")]
        public decimal Longitude { get; set; }
    }
    public class MainData
    {
        [JsonProperty("temp")]
        public decimal Temperature { get; set; }

        [JsonProperty("temp_min")]
        public decimal MinimumTemperature { get; set; }

        [JsonProperty("temp_max")]
        public decimal MaximumTemperature { get; set; }
    }
}
