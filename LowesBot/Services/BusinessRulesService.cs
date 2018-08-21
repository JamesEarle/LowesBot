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

    public enum Country { UnitedStates, Canada, Mexico, Unknown }

    public static class BusinessRulesService
    {
        public static bool TryParseOrderNumber(object value, out string number)
        {
            if (value != null
                && Regex.IsMatch(value.ToString(), "^[0-9]{9}$"))
            {
                number = value.ToString();
                return true;
            }
            else
            {
                number = default(string);
                return false;
            }
        }

        public static bool ValidatePurchaseOrderNumber(string number)
            => Regex.IsMatch(number, "^[0-9]{8}$");

        public static bool ValidateInvoiceNumber(string number)
            => Regex.IsMatch(number, "^[0-9]{5}$");

        public static Country DetermineCountry(string language)
        {
            if (language.ToLower().EndsWith("mx")) { return Country.Mexico; }
            else if (language.ToLower().EndsWith("us")) { return Country.UnitedStates; }
            else if (language.ToLower().EndsWith("ca")) { return Country.Canada; }
            else { return Country.Unknown; }
        }

        public static BusinessHourState DetermineBusinessHourState(TimeZoneName zone, DateTimeOffset? date, Country country)
        {
            if (!date.HasValue)
            {
                return BusinessHourState.Unknown;
            }

            if (date.Value.Date.Equals(HolidayHelper.Christmas)
                || date.Value.Date.Equals(HolidayHelper.NewYears)
                || date.Value.Date.Equals(HolidayHelper.Easter)

                || (Equals(country, Country.UnitedStates) && date.Value.Date.Equals(HolidayHelper.UnitedStates.Independence))
                || (Equals(country, Country.UnitedStates) && date.Value.Date.Equals(HolidayHelper.UnitedStates.Thanksgiving))
                || (Equals(country, Country.UnitedStates) && date.Value.Date.Equals(HolidayHelper.UnitedStates.LaborDay))
                || (Equals(country, Country.UnitedStates) && date.Value.Date.Equals(HolidayHelper.UnitedStates.MemorialDay))
                || (Equals(country, Country.UnitedStates) && date.Value.Date.Equals(HolidayHelper.UnitedStates.Independence))

                || (Equals(country, Country.Mexico) && date.Value.Date.Equals(HolidayHelper.Mexico.BenitoJuarez))
                || (Equals(country, Country.Mexico) && date.Value.Date.Equals(HolidayHelper.Mexico.Constitution))
                || (Equals(country, Country.Mexico) && date.Value.Date.Equals(HolidayHelper.Mexico.Independence))
                || (Equals(country, Country.Mexico) && date.Value.Date.Equals(HolidayHelper.Mexico.LaborDay))
                || (Equals(country, Country.Mexico) && date.Value.Date.Equals(HolidayHelper.Mexico.Revolution))

                || (Equals(country, Country.Canada) && date.Value.Date.Equals(HolidayHelper.Canada.Independence))
                || (Equals(country, Country.Canada) && date.Value.Date.Equals(HolidayHelper.Canada.Thanksgiving))
                || (Equals(country, Country.Canada) && date.Value.Date.Equals(HolidayHelper.Canada.LabourDay)))
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