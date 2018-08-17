using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LowesBot.Services
{
    public enum TimeZoneName
    {
        Hawaiian,
        Alaskan,
        Pacific,
        Mountain,
        Central,
        Eastern,
        Atlantic,
        Newfoundland,
        Unknown
    }

    public static class TimeZoneHelper
    {
        /*
        Usage = var zone = DetermineZone(DateTime.Now);

        16/08/2018 00:00:00 -01:00 = Unknown
        16/08/2018 00:00:00 -01:30 = Unknown
        16/08/2018 00:00:00 -02:00 = Unknown
        16/08/2018 00:00:00 -02:30 = Newfoundland
        16/08/2018 00:00:00 -03:00 = Atlantic
        16/08/2018 00:00:00 -03:30 = Unknown
        16/08/2018 00:00:00 -04:00 = Eastern
        16/08/2018 00:00:00 -04:30 = Unknown
        16/08/2018 00:00:00 -05:00 = Central
        16/08/2018 00:00:00 -05:30 = Unknown
        16/08/2018 00:00:00 -06:00 = Mountain
        16/08/2018 00:00:00 -06:30 = Unknown
        16/08/2018 00:00:00 -07:00 = Pacific
        16/08/2018 00:00:00 -07:30 = Unknown
        16/08/2018 00:00:00 -08:00 = Alaskan
        16/08/2018 00:00:00 -08:30 = Unknown
        16/08/2018 00:00:00 -09:00 = Unknown
        16/08/2018 00:00:00 -09:30 = Unknown
        16/08/2018 00:00:00 -10:00 = Hawaiian
        16/08/2018 00:00:00 -10:30 = Unknown
        */

        public static TimeZoneName DetermineZone(DateTimeOffset? date)
        {
            if (!date.HasValue)
            {
                return TimeZoneName.Unknown;
            }

            foreach (var zone in ((TimeZoneName[])Enum.GetValues(typeof(TimeZoneName))).Where(x => x != TimeZoneName.Unknown))
            {
                if (TestOffset(zone)) return zone;
            }
            return TimeZoneName.Unknown;

            bool TestOffset(TimeZoneName zone)
            {
                var info = GetZoneInfo(zone);
                var offset = info.IsDaylightSavingTime(date.Value)
                    ? info.BaseUtcOffset.Add(TimeSpan.FromHours(1))
                    : info.BaseUtcOffset;
                return Equals(date.Value.Offset, offset);
            }

            TimeZoneInfo GetZoneInfo(TimeZoneName zone)
            {
                switch (zone)
                {
                    case TimeZoneName.Hawaiian: return TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time");
                    case TimeZoneName.Alaskan: return TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time");
                    case TimeZoneName.Pacific: return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    case TimeZoneName.Mountain: return TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
                    case TimeZoneName.Central: return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                    case TimeZoneName.Eastern: return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    // cananda-specific
                    case TimeZoneName.Atlantic: return TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
                    case TimeZoneName.Newfoundland: return TimeZoneInfo.FindSystemTimeZoneById("Newfoundland Standard Time");
                    case TimeZoneName.Unknown: throw new NotSupportedException("TimeZone.Unknown");
                    default: throw new TimeZoneNotFoundException();
                }
            }
        }
    }
}