using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi
{
    public class Helper
    {
        
        public static DateTime DateNow()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo myZone = TimeZoneInfo.CreateCustomTimeZone("THAI", new TimeSpan(+7, 0, 0), "Thai", "Thai");
            DateTime custDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, myZone);
            return custDateTime;
        }

        public static DateTime UtcToThaiDate(DateTime utcDate)
        {
            TimeZoneInfo myZone = TimeZoneInfo.CreateCustomTimeZone("THAI", new TimeSpan(+7, 0, 0), "Thai", "Thai");
            DateTime custDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, myZone);
            return custDateTime;
        }

    }
}