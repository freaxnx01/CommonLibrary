using System;
using System.Globalization;

namespace Library
{
    public  static class DateTimeHelper
    {
        #region GetWeekOfYear
        public static int GetWeekOfYear(DateTime date)
        {
            const string CULTURE = "de-CH";
            CultureInfo ci = new CultureInfo(CULTURE);
            System.Globalization.Calendar cal = ci.Calendar;
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
        #endregion

        public static int GetNumberOfCalendarWeeks(int year)
        {
            // 1. KW des Folgejahres - 1. KW des betrachteten Jahres
            TimeSpan diff = GetFirstDayOfCalendarWeek(year + 1, 1) - GetFirstDayOfCalendarWeek(year, 1);
            return diff.Days / 7;
        }

        public const int DaysPerWeek = 7;
        public const int DaysPerYear = 365;

        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }

        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        #region GetFirstDayOfCalendarWeek
        public static DateTime GetFirstDayOfCalendarWeek(int week)
        {
            return GetFirstDayOfCalendarWeek(DateTime.Today.Year, week);
        }

        public static DateTime GetFirstDayOfCalendarWeek(int year, int week)
        {
            /*
            Die Berechnung der Kalenderwochen folgt der ISO-Norm 8601, europaweit umgesetzt in EN 28601, in Deutschland in DIN 1355
            Sinngemaess gilt Folgendes:
            - Eine Kalenderwoche beginnt immer mit einem Montag.
            - Die erste Kalenderwoche eines Jahres ist die, in der der vierte Januar liegt.
            */

            //Datum 4.Januar des  betr. Jahres
            DateTime vierterJanuar = new DateTime(year, 1, 4);
            //Nummer des Wochentags 1=Mo..7=So
            int woTag4J = (((int)vierterJanuar.DayOfWeek + 6) % 7) + 1;
            //Dann beginnt die KW1 x Tage vorher
            DateTime DatumErsteWoche = vierterJanuar.AddDays(1 - woTag4J);
            //Die gesuchte KW beginnt dann KW-1 Wochen später:
            return DatumErsteWoche.AddDays((week - 1) * 7);
        }
        #endregion

        #region GetLastDayOfCalendarWeek
        public static DateTime GetLastDayOfCalendarWeek(int week)
        {
            return GetLastDayOfCalendarWeek(DateTime.Today.Year, week);
        }

        public static DateTime GetLastDayOfCalendarWeek(int year, int week)
        {
            return GetFirstDayOfCalendarWeek(year, week).AddDays(6);
        }
        #endregion

        public static int GetCalendarWeekByDate(DateTime date)
        {
            int kw = 0;
            int jahr = date.Year;

            // Erster Tag der ersten Woche dieses Jahres
            DateTime tag1 = GetFirstDayOfCalendarWeek(jahr, 1);

            // Erster Tag der ersten Woche des Folgejahres
            DateTime tag2 = GetFirstDayOfCalendarWeek(jahr + 1, 1);

            // Gehört der Tag zur ersten Woche des Folgejahres?
            if (date >= tag2)
            {
                // Ja, KW 1 des Folgejahres zurückgeben
                kw = 1;
                jahr = jahr + 1;
            }
            else if (date >= tag1)
            {
                // Ja, KW aus Differenz zum 1. Tag der ersten Woche berechnen
                kw = (date - tag1).Days / 7 + 1;
            }
            else
            {
                // Nein, letzte KW des Vorjahres zurückgeben
                kw = GetNumberOfCalendarWeeks(jahr - 1);
                jahr = jahr - 1;
            }

            return kw;
        }

        #region GetFirstDayOfMonth
        public static DateTime GetFirstDayOfMonth(int month)
        {
            if (!(month >= 1 && month <= 12))
            {
                throw new ArgumentOutOfRangeException("month");
            }

            DateTime date = new DateTime(DateTime.Today.Year, month, 1);
            return GetFirstDayOfMonth(date);
        }

        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        #endregion

        #region GetLastDayOfMonth
        public static DateTime GetLastDayOfMonth(int month)
        {
            if (!(month >= 1 && month <= 12))
            {
                throw new ArgumentOutOfRangeException("month");
            }

            return GetLastDayOfMonth(new DateTime(DateTime.Today.Year, month, 1));
        }

        public static DateTime GetLastDayOfMonth(DateTime date)
        {
            DateTime dateGet = date;
            dateGet = dateGet.AddMonths(1);
            dateGet = dateGet.AddDays(-(dateGet.Day));
            return dateGet;
        }
        #endregion
    }
}
