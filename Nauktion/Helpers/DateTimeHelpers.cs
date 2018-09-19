using System;
using System.Globalization;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Nauktion.Helpers
{
    public class DateTimeHelpers
    {
        [Pure, NotNull]
        public static string FormatRemainingTime(DateTime end)
        {
            DateTime now = DateTime.Now;
            TimeSpan span = end - now;

            // Same day
            if (now.Date == end.Date)
            {
                if (Math.Abs(span.TotalHours) < 1)
                {
                    return end < now
                        ? $"för {-span.Minutes} minuter sedan"
                        : $"om {span.Minutes} minuter";
                }

                return end < now
                    ? $"for {(int) -span.TotalHours} timmar sedan"
                    : $"om {(int) span.TotalHours} timmar";
            }

            if (now.Date.AddDays(1) == end.Date)
            {
                if (end.Hour < 11)
                    return "imorgon bitti";
                if (end.Hour > 16)
                    return "imorgon kväll";
                return "imorgon vid lunchtid";
            }

            if (now.Date.AddDays(-1) == end.Date)
            {
                if (end.Hour < 11)
                    return "igår bitti";
                if (end.Hour > 16)
                    return "igår kväll";
                return "igår vid lunchtid";
            }

            // Same week
            int weekDiff = GetWeekDifference(now, end);
            switch (weekDiff)
            {
                case -1:
                    return $"på {end:dddd}en förra veckan"; // torsdagen
                case 0:
                    return end < now
                        ? $"i {end:dddd}s denna vecka"  // i torsdags
                        : $"på {end:dddd} denna vecka"; // på torsdag
                case 1:
                    return $"på {end:dddd} nästa vecka"; // torsdag
                case 2:
                    return $"på {end:dddd} om två veckor"; // torsdag
            }

            // Months?
            int monthsDiff = GetMonthDifference(now, end);
            switch (monthsDiff)
            {
                case -1:
                    return $"den {end:d} förra månaden, {end:MMMM}";
                case 0:
                    return end < now
                        ? $"för {-span.Days} dagar sedan nu i {end:MMMM}"
                        : $"om {span.Days} dagar nu i {end:MMMM}";
                case 1:
                    return $"den {end:d} nästa månad, {end:MMMM}";
            }

            // Year?
            int yearDiff = now.Year - end.Year;
            switch (yearDiff)
            {
                case -1:
                    return $"förra året i {end:MMMM}";
                case 1:
                    return $"nästa år i {end:MMMM}";
            }

            if (yearDiff > 1)
                return $"om {yearDiff} år";
            if (yearDiff < -1)
                return $"för {-yearDiff} år sedan";

            if (monthsDiff > 1)
                return $"om {monthsDiff} månader";
            if (monthsDiff < -1)
                return $"för {-monthsDiff} månader sedan";

            return end < now
                ? "för ett tag sedan"
                : "om ett tag";
        }

        /// <summary>
        /// If <paramref name="a"/> and <paramref name="b"/> is during same week, returns 0.
        /// </summary>
        public static int GetWeekDifference(DateTime a, DateTime b)
        {
            a = GetStartOfWeek(a);
            b = GetStartOfWeek(b);
            int days = (b - a).Days;
            return days / 7;
        }

        public static DateTime GetStartOfWeek(DateTime date)
        {
            int dayOfWeek = ((int)date.DayOfWeek + 6) % 7;
            return date.Date.AddDays(-dayOfWeek);
        }

        public static int GetMonthDifference(DateTime a, DateTime b)
        {
            int monthA = a.Month + a.Year * 12;
            int monthB = b.Month + b.Year * 12;
            return monthB - monthA;
        }

        public static DateTime GetStartOfMonth(DateTime date)
        {
            return date.Date.AddDays(-date.Day);
        }
    }
}