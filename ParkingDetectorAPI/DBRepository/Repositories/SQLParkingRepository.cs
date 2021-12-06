using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingDetectorAPI.Models.ResponsesModels;
using ParkingDetectorAPI.Models.DBModels;
using ParkingDetectorAPI.DBRepository.Interfaces;

namespace ParkingDetectorAPI.DBRepository.Repositories
{
    public class SQLParkingRepository : IParkingRepository
    {
		private ParkingDetectorAPIContext db;
		public SQLParkingRepository(ParkingDetectorAPIContext _db)
		{
			//Переменная контекста для связи с БД
			db = _db;
		}

		public Parking GetParkingDetail(int parkingId)
		{
			var result = db.Parkings.FirstOrDefault(parking => parking.Id == parkingId);
			return result;
		}

		public List<ParkingShortResponse> GetParkingsList()
		{
			//выбираем все парковки из таблицы Parkings и сразу преобразовываем в тип ParkingShortResponse
			return db.Parkings.Select(parking => new ParkingShortResponse { 
							Id = parking.Id,
							Address = parking.Address,
							TotalParkingSpaces= parking.TotalParkingSpaces,
							FreeParkingSpaces = parking.FreeParkingSpaces})
						.ToList();
		}

		public string UpdateFreeParkingSpaces(int parkingId, int newValueFreeSpaces)
		{
			//Получаем парковку по Id
			Parking parkingForUpdate = db.Parkings.FirstOrDefault(parking => parking.Id == parkingId);
			//Проверяем, что парковка существует
			if (parkingForUpdate == null)
			{
				return "There is no such parking";
			}
			parkingForUpdate.FreeParkingSpaces = newValueFreeSpaces;
			//Обновляем данные
			db.Parkings.Update(parkingForUpdate);
			Save();
			return "Success";
		}

		//Сохранение изменений
		private void Save()
		{
			db.SaveChanges();
		}
	}
}
