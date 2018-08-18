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
        public static bool IsValidOrderNumber(string number) => Regex.IsMatch(number, "^[0-9]{9}$");

        public static bool IsValidPurchaseOrder(string number) => Regex.IsMatch(number, "^[0-9]{8}$");

        public static bool IsValidInvoiceNumber(string number) => Regex.IsMatch(number, "^[0-9]{5}$");

        public static BusinessHourState DetermineBusinessHourState(TimeZoneName zone, DateTimeOffset? date, string locale)
        {
            if (!date.HasValue)
            {
                return BusinessHourState.Unknown;
            }

            if (date.Value.Date.Equals(HolidayHelper.Christmas)
                || date.Value.Date.Equals(HolidayHelper.NewYears)
                || date.Value.Date.Equals(HolidayHelper.Easter)

                || (Equals(locale, "us") & date.Value.Date.Equals(HolidayHelper.UnitedStates.Independence))
                || (Equals(locale, "uw") & date.Value.Date.Equals(HolidayHelper.UnitedStates.Thanksgiving))
                || (Equals(locale, "us") & date.Value.Date.Equals(HolidayHelper.UnitedStates.LaborDay))
                || (Equals(locale, "us") & date.Value.Date.Equals(HolidayHelper.UnitedStates.MemorialDay))
                || (Equals(locale, "us") & date.Value.Date.Equals(HolidayHelper.UnitedStates.Independence))

                || (Equals(locale, "mx") & date.Value.Date.Equals(HolidayHelper.Mexico.BenitoJuarez))
                || (Equals(locale, "mx") & date.Value.Date.Equals(HolidayHelper.Mexico.Constitution))
                || (Equals(locale, "mx") & date.Value.Date.Equals(HolidayHelper.Mexico.Independence))
                || (Equals(locale, "mx") & date.Value.Date.Equals(HolidayHelper.Mexico.LaborDay))
                || (Equals(locale, "mx") & date.Value.Date.Equals(HolidayHelper.Mexico.Revolution))

                || (Equals(locale, "ca") & date.Value.Date.Equals(HolidayHelper.Canada.Independence))
                || (Equals(locale, "ca") & date.Value.Date.Equals(HolidayHelper.Canada.Thanksgiving))
                || (Equals(locale, "ca") & date.Value.Date.Equals(HolidayHelper.Canada.LabourDay)))
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