﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheatherSolution.Domain.DTO
{
    public class CreateWheatherDTO
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Temperature { get; set; }
        public decimal MinimumTemperature { get; set; }
        public decimal MaximumTemperature { get; set; }
        public string? City { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
