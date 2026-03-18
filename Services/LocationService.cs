using GPS_Listener.Data;
using GPS_Listener.Models;
using System;

namespace GPS_Listener.Services   // ✅ IMPORTANT
{
    public static class LocationService
    {
        public static void Save(string imei, double lat, double lon, int speed)
        {
            using (var db = new GpsDbContext())
            {
                var location = new LocationEntity
                {
                    IMEI = imei,
                    Latitude = lat,
                    Longitude = lon,
                    Speed = speed,
                    Timestamp = DateTime.UtcNow
                };

                db.Locations.Add(location);
                db.SaveChanges();
            }
        }
    }
}