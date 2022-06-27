using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Asset.API.Helpers
{
    public static class ArabicNumeralHelper
    {
        public static string ConvertNumerals(this string input)
        {
     
                return input.Replace('0', '\u06f0')
                        .Replace('1', '\u06f1')
                        .Replace('2', '\u06f2')
                        .Replace('3', '\u06f3')
                        .Replace('4', '\u06f4')
                        .Replace('5', '\u06f5')
                        .Replace('6', '\u06f6')
                        .Replace('7', '\u06f7')
                        .Replace('8', '\u06f8')
                        .Replace('9', '\u06f9');
          
        }

        public static string ConvertNumber(string englishNumber)
        {
            string theResult = "";
            foreach (char ch in englishNumber)
            {
                theResult += (char)(1776 + char.GetNumericValue(ch));
            }
            return theResult;
        }
        public static string toArabicNumber(string input)
        {
            var arabic = new string[10] { "۰", "۱", "۲", "۳", "٤", "٥", "٦", "۷", "۸", "۹" };
            for (int j = 0; j < arabic.Length; j++)
            {
                input = input.Replace(j.ToString(), arabic[j]);
            }
            return input;
        }

    }
}
