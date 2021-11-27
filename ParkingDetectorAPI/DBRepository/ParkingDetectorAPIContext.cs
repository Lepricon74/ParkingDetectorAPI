using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingDetectorAPI.Models.DBModels;

namespace ParkingDetectorAPI.DBRepository
{
    public class ParkingDetectorAPIContext : DbContext
    {
        public ParkingDetectorAPIContext(DbContextOptions<ParkingDetectorAPIContext> options) : base(options)
        {

        }

        public DbSet<Parking> Parkings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Parking>().HasKey(u => new { u.Id });
        }
    }
}
