using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingDetectorAPI.Models.ResponsesModels;
using ParkingDetectorAPI.Models.DBModels;

namespace ParkingDetectorAPI.DBRepository.Interfaces
{
    public interface IParkingRepository 
    {
        List<ParkingShortResponse> GetParkingsList();
        Parking GetParkingDetail(int parkingId);

        public string UpdateFreeParkingSpaces(int parkingId, int newValueFreeSpaces);
    }
}
