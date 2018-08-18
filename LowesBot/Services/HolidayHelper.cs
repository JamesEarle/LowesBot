using System;
using System.Linq;

namespace LowesBot.Services
{
public static class HolidayHelper
{
    public static DateTime NewYears => new DateTime(DateTime.Now.Year, 1, 1).Date;
    public static DateTime Christmas => new DateTime(DateTime.Now.Year, 12, 25).Date;
    public static DateTime GoodFriday => GetEaster(DateTime.Now.Year).AddDays(-2);
    public static DateTime Easter => GetEaster(DateTime.Now.Year);
    public static class Canada
    {
        public static DateTime LabourDay => new DateTime(DateTime.Now.Year, 5, 1).Date;
        public static DateTime Independence => new DateTime(DateTime.Now.Year, 7, 1).Date;
        public static DateTime Thanksgiving => new DateTime(DateTime.Now.Year, 11, 8).Date;
    }
    public static class Mexico
    {
        public static DateTime LaborDay => new DateTime(DateTime.Now.Year, 5, 1).Date;
        public static DateTime Constitution => new DateTime(DateTime.Now.Year, 2, 5).Date;
        public static DateTime Independence => new DateTime(DateTime.Now.Year, 9, 16).Date;
        public static DateTime BenitoJuarez => FindDay(DateTime.Now.Year, 3, Count.Third, DayOfWeek.Monday);
        public static DateTime Revolution => FindDay(DateTime.Now.Year, 11, Count.Third, DayOfWeek.Monday);
    }
    public static class UnitedStates
    {
        public static DateTime Independence => new DateTime(DateTime.Now.Year, 7, 4).Date;
        public static DateTime LaborDay => FindDay(DateTime.Now.Year, 9, Count.First, DayOfWeek.Monday);
        public static DateTime MemorialDay => FindDay(DateTime.Now.Year, 5, Count.Last, DayOfWeek.Monday);
        public static DateTime Thanksgiving => FindDay(DateTime.Now.Year, 11, Count.Third, DayOfWeek.Thursday);
    }

    public enum Count : int { First = 1, Seecond = 2, Third = 3, Last = 4 }
    static DateTime FindDay(int year, int month, Count count, DayOfWeek dayOfWeek)
    {
        if (count == Count.Last)
        {
            var day = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            while (day.DayOfWeek != dayOfWeek)
            {
                day = day.AddDays(-1);
            }
            return day.Date;
        }
        else
        {
            var month_day =
                (from day in Enumerable.Range(1, 30)
                    where new DateTime(year, month, day).DayOfWeek == dayOfWeek
                    select day).ElementAt((int)count);
            return new DateTime(year, 11, month_day).Date;
        }
    }

    static DateTime GetEaster(int year)
    {
        // g is the position within the 19 year lunar cycle; known as the golden number. 
        var g = year % 19;
            
        // c is the century. 
        var c = year / 100;
            
        // h is the number of days between the equinox and the next full moon. 
        var h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            
        // i is the number of days between the full moon after the equinox and the first sunday after that full moon.
        var i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

        var day = 0;
        day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;

        var month = 0;
        month = 3;

        if (day > 31)
        {
            month++;
            day -= 31;
        }

        return new DateTime(year, month, day);
    }
}
}