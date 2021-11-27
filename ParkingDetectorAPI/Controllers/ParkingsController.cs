using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingDetectorAPI.DBRepository.Interfaces;
using ParkingDetectorAPI.DBRepository;
using ParkingDetectorAPI.DBRepository.Repositories;
using ParkingDetectorAPI.Models.RequestsModels;
using Newtonsoft.Json;
using System.IO;

namespace ParkingDetectorAPI.Controllers
{
    [Route("api/v1/parkings")]
    public class ParkingsController : Controller
    {
        IParkingRepository parkingRepository;

        public ParkingsController(ParkingDetectorAPIContext dbcontext)
        {
            parkingRepository = new SQLParkingRepository(dbcontext);
        }

        [Route("")]
        [HttpGet]
        public JsonResult GetParkingsList()
        {
            return new JsonResult(parkingRepository.GetParkingsList());
        }

        [Route("{parkingId:int}")]
        [HttpGet]
        public JsonResult GetParkingDetail(int parkingId)
        {
            return new JsonResult(parkingRepository.GetParkingDetail(parkingId));
        }    

        [Route("{parkingId:int}/update")]
        [HttpPost]
        public JsonResult UpdateFreeParkingSpaces(int parkingId)
        {
            var stream = new StreamReader(Request.Body);
            var body = stream.ReadToEndAsync().Result;
            NewFreeParkingSpacesRequest newFreeParkingSpaces;
            try
            {
                newFreeParkingSpaces = JsonConvert.DeserializeObject<NewFreeParkingSpacesRequest>(body);
            }
            catch (JsonReaderException e)
            {
                return new JsonResult(e.ToString());
            }
            if (newFreeParkingSpaces == null || TryValidateModel(newFreeParkingSpaces) == false) return new JsonResult("Content Error!");
            if (newFreeParkingSpaces.Value==-1) return new JsonResult("Incorrect JSON format!");
            string result = parkingRepository.UpdateFreeParkingSpaces(parkingId, newFreeParkingSpaces.Value);
            return new JsonResult(result);
        }

        [Route("{parkingId:int}/camera")]
        [HttpGet]

        public IActionResult GetCurrentCameraImage(int parkingId)
        {
            DateTime now = DateTime.Now;
            int curSec = (now.Minute*60+now.Second);
            int cur5MinIntInSec= curSec % 300;
            int cameraImageId = cur5MinIntInSec / 10;
            string cameraPicturePath = "~/cameras/" + parkingId.ToString() + "/" + cameraImageId.ToString()+ ".jpg";
            return RedirectPermanent(cameraPicturePath);
        }
    }
        
}
