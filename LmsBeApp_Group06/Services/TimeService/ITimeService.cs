using System;

namespace LmsBeApp_Group06.Services.TimeService
{
    public interface ITimeService
    {
        DateTime ConvertToUtc(DateTime dateTime, string zoneId);
        DateTime ConvertToLocalTime(DateTime dateTime, string zoneId);
        TimeZoneInfo GetZoneInfo(string zoneId);
        bool ValidateZoneId(string zoneId);
    }
}
