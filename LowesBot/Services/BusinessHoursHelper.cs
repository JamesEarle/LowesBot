using System;

namespace LowesBot.Services
{
    public enum BusinessHourState
    {
        Open,
        Closing,
        Closed
    }

    public static class BusinessHoursHelper
    {
        public static BusinessHourState DetermineState(TimeZoneName zone, DateTimeOffset date)
        {
            if (date.Date.Equals(HolidayHelper.Christmas)
                || date.Date.Equals(HolidayHelper.NewYears)
                || date.Date.Equals(HolidayHelper.UnitedStates.Independence)
                || date.Date.Equals(HolidayHelper.UnitedStates.Thanksgiving)
                || date.Date.Equals(HolidayHelper.UnitedStates.LaborDay)
                || date.Date.Equals(HolidayHelper.UnitedStates.MemorialDay)
                || date.Date.Equals(HolidayHelper.UnitedStates.Independence)
                || date.Date.Equals(HolidayHelper.Canada.Independence))
            {
                return BusinessHourState.Closed;
            }

            switch (zone)
            {
                case TimeZoneName.Hawaiian when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Alaskan when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Pacific when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Mountain when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Central when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Eastern when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Atlantic when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Newfoundland when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
                case TimeZoneName.Unknown when (TimeBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(22))): return BusinessHourState.Open;
            }

            switch (zone)
            {
                case TimeZoneName.Hawaiian when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Alaskan when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Pacific when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Mountain when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Central when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Eastern when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Atlantic when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Newfoundland when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
                case TimeZoneName.Unknown when (TimeBetween(TimeSpan.FromHours(22), TimeSpan.FromHours(23))): return BusinessHourState.Closing;
            }

            return BusinessHourState.Closed;

            bool TimeBetween(TimeSpan start, TimeSpan end)
            {
                var now = date.TimeOfDay;
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