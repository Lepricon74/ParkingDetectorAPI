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
			db = _db;
		}

		public Parking GetParkingDetail(int parkingId)
		{

			var result = db.Parkings.FirstOrDefault(parking => parking.Id == parkingId);
			return result;
		}

		public List<ParkingShortResponse> GetParkingsList()
		{
			//выбираем все статьи из таблицы Articles
			return db.Parkings.Select(parking => new ParkingShortResponse { 
							Id = parking.Id,
							Address = parking.Address,
							TotalParkingSpaces= parking.TotalParkingSpaces,
							FreeParkingSpaces = parking.FreeParkingSpaces})
						.ToList();
		}

		public string UpdateFreeParkingSpaces(int parkingId, int newValueFreeSpaces)
		{
			Parking parkingForUpdate = db.Parkings.FirstOrDefault(parking => parking.Id == parkingId);
			if (parkingForUpdate == null)
			{
				return "There is no such parking";
			}
			parkingForUpdate.FreeParkingSpaces = newValueFreeSpaces;
			db.Parkings.Update(parkingForUpdate);
			Save();
			return "Success";
		}

		private void Save()
		{
			db.SaveChanges();
		}
	}
}
