using System;

namespace GPS_Listener.Models
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Speed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}