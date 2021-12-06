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
using ParkingDetectorAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ParkingDetectorAPI.Controllers
{
    [Route("api/v1/parkings")]
    public class ParkingsController : Controller
    {
        IParkingRepository parkingRepository;
        IHubContext<NewFreeValueHub> hubContext;

        public ParkingsController(ParkingDetectorAPIContext dbcontext, IHubContext<NewFreeValueHub> _hubContext)
        {
            // Создаем репозиторий для взаимодействия с таблицей Parkings
            parkingRepository = new SQLParkingRepository(dbcontext);
            //Создаем хаб
            hubContext = _hubContext;
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
            //Достаем данные из тела запроса
            var stream = new StreamReader(Request.Body);
            var body = stream.ReadToEndAsync().Result;
            NewFreeParkingSpacesRequest newFreeParkingSpaces;
            //Пытаемся получить объект класса newFreeParkingSpaces из данных запроса
            try
            {
                newFreeParkingSpaces = JsonConvert.DeserializeObject<NewFreeParkingSpacesRequest>(body);
            }
            catch (JsonReaderException e)
            {
                return new JsonResult(e.ToString());
            }
            //Проверяем данные на корректность
            if (newFreeParkingSpaces == null || TryValidateModel(newFreeParkingSpaces) == false) return new JsonResult("Content Error!");
            if (newFreeParkingSpaces.Value==-1) return new JsonResult("Incorrect JSON format!");
            //Обновляем значение в БД у данной парковки
            string result = parkingRepository.UpdateFreeParkingSpaces(parkingId, newFreeParkingSpaces.Value);
            //Оповещаем клиентов используя хаб
            NotifyClientsAsync(parkingId, newFreeParkingSpaces.Value);
            return new JsonResult(result);
        }

        //Метод оповещения всех подключенных клиентов
        private async void NotifyClientsAsync(int parkingId, int newFreeValueSpaces)
        {
                //Отправляем ID парковки и новое значение свободных мест
                await hubContext.Clients.All.SendAsync("Notify", parkingId, newFreeValueSpaces);
        }

        [Route("{parkingId:int}/camera")]
        [HttpGet]
        public IActionResult GetCurrentCameraImage(int parkingId)
        {
            //Получение текущего времени в секундах без учета часов
            DateTime now = DateTime.Now;      
            int curSec = (now.Minute*60+now.Second);
            //Делим час на интервалы по 300 секунд, т.е. каждый час содержит 12 периодов
            //переменная cur5MinIntInSec принимает значения от 0 до 299 и обнуляется каждые 5 минут
            int cur5MinIntInSec = curSec % 300;
            //делим каждый 300 секундный интервал на отрезки времени по 10 секунд(время одного изображения) 
            //каждые 5 минут переменная cameraImageId будет принимать значения от 0 до 29, каждое из которых будет постоянно в течении 10 секунд
            int cameraImageId = cur5MinIntInSec / 10;
            //Формируем путь к изображению
            string cameraPicturePath = "~/cameras/" + parkingId.ToString() + "/" + cameraImageId.ToString()+ ".jpg";
            //Возвращаем картинку
            return RedirectPermanent(cameraPicturePath);
        }
    }
        
}
