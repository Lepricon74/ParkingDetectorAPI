using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingDetectorAPI.Models.ResponsesModels
{
    public class ParkingShortResponse
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int TotalParkingSpaces { get; set; }
        public int FreeParkingSpaces { get; set; }
    }
}
