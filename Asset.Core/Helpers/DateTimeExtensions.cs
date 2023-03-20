using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Helpers
{
    public static class DateTimeExtensions
    {

        public static string ToDateString(this DateTime from, DateTime to)
        {
            DateTime today = to;

            int months = today.Month - from.Month;
            int years = today.Year - from.Year;

            if (today.Month < to.Month || (today.Month == to.Month && today.Day < to.Day))
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            int days = (today - from.AddMonths((years * 12) + months)).Days;



            return years + "  Years   " + months + "  Months   " + days + "  Days ";

            //DateTime today = to;

            //int months = today.Month - from.Month;
            //int years = today.Year - from.Year;

            //if (today.Day < from.Day)
            //{
            //    months--;
            //}

            //if (months < 0)
            //{
            //    years--;
            //    months += 12;
            //}

            //int days = (today - from.AddMonths((years * 12) + months)).Days;

            //return string.Format("{0} year{1}, {2} month{3} and {4} day{5}",
            //                     years, (years == 1) ? "" : "s",
            //                     months, (months == 1) ? "" : "s",
            //                     days, (days == 1) ? "" : "s");
        }

        public static string ToDateStringAr(this DateTime from, DateTime to)
        {
            DateTime today = to;

            int months = today.Month - from.Month;
            int years = today.Year - from.Year;

          
            if (today.Month < to.Month || (today.Month == to.Month && today.Day < to.Day))
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            int days = (today - from.AddMonths((years * 12) + months)).Days;


            return string.Format("{0} {1},{2} {3},{4} {5}",
                                 years, (years == 1) ? "سنة" : (years == 2) ? "سنتين" : "سنوات",
                                 months, (months == 1) ? "شهر" : (months == 2) ? "شهرين" : "شهور",
                                 days, (days == 1) ? "يوم" : (days == 2) ? "يومين":"أيام");

        }




        public static string CountDateTime(this DateTime from, DateTime to)
        {
            //DateTime dateTimeToday = DateTime.UtcNow;
         //  DateTime birthDate = new DateTime(1956, 8, 27);

            TimeSpan difference = to.Subtract(from);
            TimeSpan difference2 = from.Subtract(to);
            var firstDay = new DateTime(1, 1, 1);

            //Console.WriteLine($"Age in seconds: {Math.Round(difference.TotalSeconds)}");

            //Console.WriteLine($"Age in minutes: {Math.Round(difference.TotalMinutes)}");

            //Console.WriteLine($"Age in hours: {Math.Round(difference.TotalHours)}");

            //Console.WriteLine($"Age in days: {Math.Round(difference.TotalDays)}");

            //Console.WriteLine($"Age in weeks: {System.Math.Ceiling(difference.TotalDays / 7)}");

            int totalYears = (firstDay + difference).Year - 1;
          //  Console.WriteLine($"Age in years: {totalYears}");

            int totalMonths = (totalYears * 12) + (firstDay + difference).Month - 1;
         //   Console.WriteLine($"Age in months: {totalMonths}");

            int runningMonths = totalMonths - (totalYears * 12);
          //  Console.WriteLine($"Age in {runningMonths}");

            int runningDays = (to - from.AddMonths((totalYears * 12) + runningMonths)).Days;
         //   Console.WriteLine($"Age in {runningDays}");

          //  Console.WriteLine($"Age is {totalYears} years, {runningMonths} months, {runningDays} days");


            return totalYears + "  سنة   " + runningMonths + "  شهر   " + runningDays + "  يوم ";


        }

        static public string calculateAge(DateTime birthDate, DateTime now)
        {
            birthDate = birthDate.Date;
            now = now.Date;

            var days = now.Day - birthDate.Day;
            if (days < 0)
            {
                var newNow = now.AddMonths(-1);
                days += (int)(now - newNow).TotalDays;
                now = newNow;
            }
            var months = now.Month - birthDate.Month;
            if (months < 0)
            {
                months += 12;
                now = now.AddYears(-1);
            }
            var years = now.Year - birthDate.Year;
            if (years == 0)
            {
                if (months == 0)
                    return days.ToString() + " days";
                else
                    return months.ToString() + " months";
            }
            return years.ToString();
        }

    }
}
