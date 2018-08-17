using System;
using System.Text.RegularExpressions;

namespace LowesBot.Services
{
    public enum BusinessHourState
    {
        Open,
        Closing,
        Closed,
        Unknown
    }

    public static class LowesHelper
    {
        public static bool IsValidOrderNumber(string number) => Regex.IsMatch(number, "\\d{,9}");

        public static bool IsValidPurchaseOrder(string number) => Regex.IsMatch(number, "\\d{,8}");

        public static bool IsValidInvoiceNumber(string number) => Regex.IsMatch(number, "\\d{,5}");

        public static BusinessHourState DetermineBusinessHourState(TimeZoneName zone, DateTimeOffset? date)
        {
            if (!date.HasValue)
            {
                return BusinessHourState.Unknown;
            }

            if (date.Value.Date.Equals(HolidayHelper.Christmas)
                || date.Value.Date.Equals(HolidayHelper.NewYears)
                || date.Value.Date.Equals(HolidayHelper.UnitedStates.Independence)
                || date.Value.Date.Equals(HolidayHelper.UnitedStates.Thanksgiving)
                || date.Value.Date.Equals(HolidayHelper.UnitedStates.LaborDay)
                || date.Value.Date.Equals(HolidayHelper.UnitedStates.MemorialDay)
                || date.Value.Date.Equals(HolidayHelper.UnitedStates.Independence)
                || date.Value.Date.Equals(HolidayHelper.Canada.Independence))
            {
                return BusinessHourState.Closed;
            }

            switch (zone)
            {
                case TimeZoneName.Hawaiian when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Alaskan when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Pacific when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Mountain when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Central when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Eastern when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Atlantic when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Newfoundland when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                case TimeZoneName.Unknown when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))):
                    return BusinessHourState.Open;
            }

            switch (zone)
            {
                case TimeZoneName.Hawaiian when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Alaskan when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Pacific when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Mountain when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Central when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Eastern when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Atlantic when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Newfoundland when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                case TimeZoneName.Unknown when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))):
                    return BusinessHourState.Closing;
            }

            return BusinessHourState.Closed;

            bool TimeBetween(TimeSpan start, TimeSpan end)
            {
                var now = date.Value.TimeOfDay;
                if (start < end)
                {
                    return start <= now && now <= end;
                }
                else
                {
                    return !(end < now && now < start);
                }
            }
        }
    }
}