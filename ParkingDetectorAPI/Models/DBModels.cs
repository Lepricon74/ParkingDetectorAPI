using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingDetectorAPI.Models.DBModels
{
    public class Parking
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int TotalParkingSpaces { get; set; }
        public int FreeParkingSpaces { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }
        public string Camera { get; set; }
        
    }
}
