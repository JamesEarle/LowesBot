using System;
using System.Linq;

namespace LowesBot.Services
{
    public static class HolidayHelper
    {
        public static DateTime Christmas => new DateTime(DateTime.Now.Year, 12, 25).Date;
        public static DateTime NewYears => new DateTime(DateTime.Now.Year, 1, 1).Date;
        public static class Canada
        {
            public static DateTime Independence => new DateTime(DateTime.Now.Year, 7, 1).Date;
        }
        public static class UnitedStates
        {
            public static DateTime Independence => new DateTime(DateTime.Now.Year, 7, 4).Date;
            public static DateTime LaborDay
            {
                get
                {
                    var day = new DateTime(DateTime.Now.Year, 9, 1);
                    var dayOfWeek = day.DayOfWeek;
                    while (dayOfWeek != DayOfWeek.Monday)
                    {
                        day = day.AddDays(1);
                        dayOfWeek = day.DayOfWeek;
                    }
                    return day.Date;
                }
            }
            public static DateTime MemorialDay
            {
                get
                {
                    var day = new DateTime(DateTime.Now.Year, 5, 31);
                    var dayOfWeek = day.DayOfWeek;
                    while (dayOfWeek != DayOfWeek.Monday)
                    {
                        day = day.AddDays(-1);
                        dayOfWeek = day.DayOfWeek;
                    }
                    return day.Date;
                }
            }
            public static DateTime Thanksgiving
            {
                get
                {
                    var thanksgiving =
                        (from day in Enumerable.Range(1, 30)
                         where new DateTime(DateTime.Now.Year, 11, day).DayOfWeek == DayOfWeek.Thursday
                         select day).ElementAt(3);
                    var thanksgivingDay = new DateTime(DateTime.Now.Year, 11, thanksgiving);
                    return thanksgivingDay.Date;
                }
            }
        }
    }
}