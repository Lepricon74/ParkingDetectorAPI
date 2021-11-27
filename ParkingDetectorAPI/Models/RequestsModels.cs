using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ParkingDetectorAPI.Models.RequestsModels
{
    public class NewFreeParkingSpacesRequest
    {
        [Required]
        public int Value { get; set; }


    }
}
