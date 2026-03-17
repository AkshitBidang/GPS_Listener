//using System;
//using System.Net.Sockets;
//using System.Linq;
//using GPS_Listener.Models;

//public static class LocationHandler
//{
//    public static void Handle(byte[] data, TcpClient client)
//    {
//        try
//        {
//            // Find session using client
//            var session = SessionManager.Sessions.Values
//                .FirstOrDefault(x => x.Client == client);

//            if (data.Length < 20)
//            {
//                Console.WriteLine("Invalid Location Packet");
//                return;
//            }

//            // Extract latitude & longitude
//            int latRaw = BitConverter.ToInt32(
//                data.Skip(11).Take(4).Reverse().ToArray());

//            int lonRaw = BitConverter.ToInt32(
//                data.Skip(15).Take(4).Reverse().ToArray());

//            double latitude = latRaw / 1800000.0;
//            double longitude = lonRaw / 1800000.0;

//            if (session != null)
//            {
//                session.LastLocation = DateTime.UtcNow;

//                Console.WriteLine($"Location from {session.Imei} → Lat: {latitude}, Lon: {longitude}");
//            }
//            else
//            {
//                Console.WriteLine($"Location (Unknown Device) → Lat: {latitude}, Lon: {longitude}");
//            }

//            // Optional: Google Maps link
//            Console.WriteLine($"Map: https://maps.google.com/?q={latitude},{longitude}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Location Parse Error: {ex.Message}");
//        }
//    }
//}




using System;
using System.Net.Sockets;
using System.Linq;
using GPS_Listener.Models;

public static class LocationHandler
{
    public static void Handle(byte[] data, TcpClient client)
    {
        try
        {
            var session = SessionManager.Sessions.Values
                .FirstOrDefault(x => x.Client == client);

            if (data.Length < 30)
            {
                Console.WriteLine("Invalid Location Packet");
                return;
            }

            int index = 4; // skip start + length + protocol

            // 📅 Date Time
            int year = data[index++] + 2000;
            int month = data[index++];
            int day = data[index++];
            int hour = data[index++];
            int minute = data[index++];
            int second = data[index++];

            string dateTime = $"{year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}";

            // 📡 GPS Info + Satellites
            byte gpsInfo = data[index++];
            int satellites = gpsInfo & 0x0F;

            // 📍 Latitude
            int latRaw = BitConverter.ToInt32(data.Skip(index).Take(4).Reverse().ToArray());
            index += 4;
            double latitude = latRaw / 1800000.0;

            // 📍 Longitude
            int lonRaw = BitConverter.ToInt32(data.Skip(index).Take(4).Reverse().ToArray());
            index += 4;
            double longitude = lonRaw / 1800000.0;

            // 🚗 Speed
            int speed = data[index++];

            // 🧭 Course & Status
            ushort courseStatus = BitConverter.ToUInt16(data.Skip(index).Take(2).Reverse().ToArray());
            index += 2;

            int course = courseStatus & 0x03FF; // last 10 bits

            bool gpsFixed = (courseStatus & (1 << 12)) != 0;
            bool eastLongitude = (courseStatus & (1 << 10)) == 0;
            bool northLatitude = (courseStatus & (1 << 11)) != 0;

            // 🌐 MCC
            int mcc = BitConverter.ToUInt16(data.Skip(index).Take(2).Reverse().ToArray());
            index += 2;

            // 🌐 MNC
            int mnc = data[index++];

            // 📡 LAC
            int lac = BitConverter.ToUInt16(data.Skip(index).Take(2).Reverse().ToArray());
            index += 2;

            // 📡 Cell ID
            int cellId = (data[index++] << 16) | (data[index++] << 8) | data[index++];

            // 🧠 Print everything
            string imei = session?.Imei ?? "Unknown";

            Console.WriteLine("\n========= LOCATION DATA =========");
            Console.WriteLine($"Device     : {imei}");
            Console.WriteLine($"Date Time  : {dateTime}");
            Console.WriteLine($"Satellites : {satellites}");
            Console.WriteLine($"Latitude   : {latitude}");
            Console.WriteLine($"Longitude  : {longitude}");
            Console.WriteLine($"Speed      : {speed} km/h");
            Console.WriteLine($"Course     : {course}°");
            Console.WriteLine($"GPS Fixed  : {gpsFixed}");
            Console.WriteLine($"MCC        : {mcc}");
            Console.WriteLine($"MNC        : {mnc}");
            Console.WriteLine($"LAC        : {lac}");
            Console.WriteLine($"Cell ID    : {cellId}");
            Console.WriteLine($"Map        : https://maps.google.com/?q={latitude},{longitude}");
            Console.WriteLine("=================================\n");

            if (session != null)
            {
                session.LastLocation = DateTime.UtcNow;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Location Parse Error: {ex.Message}");
        }
    }
}