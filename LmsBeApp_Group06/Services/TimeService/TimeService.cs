using System;
using System.Linq;

namespace LmsBeApp_Group06.Services.TimeService
{
    public class TimeService : ITimeService
    {
        public DateTime ConvertToLocalTime(DateTime dateTime, string zoneId)
        {
            if (String.IsNullOrWhiteSpace(zoneId))
                zoneId = "SE Asia Standard Time";
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            return TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), timeZoneInfo);
        }

        public DateTime ConvertToUtc(DateTime dateTime, string zoneId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(zoneId))
                    zoneId = "SE Asia Standard Time";
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
                return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified), timeZoneInfo);
            }
            catch
            {
                return dateTime;
            }
        }

        public TimeZoneInfo GetZoneInfo(string zoneId)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateZoneId(string zoneId)
        {
            var zoneInfos = TimeZoneInfo.GetSystemTimeZones();
            return zoneInfos.Any(x => x.Id == zoneId.Trim());
        }
    }
}
