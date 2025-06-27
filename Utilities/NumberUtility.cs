using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Malgo.Utilities
{
    public static class NumberUtility
    {
        public static string NumberNormalize(float number)
        {
            if (number >= 1_000_000_000)
            {
                return (number / 1_000_000_000).ToString("F2") + "B";
            }
            else if (number >= 1_000_000)
            {
                return (number / 1_000_000).ToString("F2") + "M";
            }
            else if (number >= 1_000)
            {
                return (number / 1_000).ToString("F2") + "K";
            }
            else
            {
                return number.ToString("F2");
            }
        }

        private static readonly string[] suffixes = { "", "K", "M", "B", "T" };

        public static string NumberToString(double value, int decimalPlaces = 2)
        {
            if (value < 1000) return value.ToString("0." + new string('0', decimalPlaces));

            int suffixIndex = 0;
            while (value >= 1000)
            {
                value /= 1000;
                suffixIndex++;
            }

            if (suffixIndex < suffixes.Length)
                return value.ToString("0." + new string('0', decimalPlaces)) + suffixes[suffixIndex];

            string letterSuffix = ConvertToLetterNotation(suffixIndex - suffixes.Length);
            return value.ToString("0." + new string('0', decimalPlaces)) + letterSuffix;
        }

        private static string ConvertToLetterNotation(long index)
        {
            StringBuilder result = new StringBuilder();
            result.Insert(0, (char)('a' + (int)(index / 26)));
            result.Insert(1, (char)('a' + (int)(index % 26)));
            return result.ToString();
        }

        public static float ClampMaxFloat(float result)
        {
            if (float.IsInfinity(result) || result > float.MaxValue)
            {
                result = float.MaxValue;
            }

            if (float.IsNegativeInfinity(result) || result < float.MinValue)
            {
                result = float.MinValue;
            }

            return result;
        }

        public static float ClampMaxFloat(double result)
        {
            if (float.IsInfinity((float) result) || result > float.MaxValue)
            {
                result = float.MaxValue;
            }

            if (float.IsNegativeInfinity((float) result) || result < float.MinValue)
            {
                result = float.MinValue;
            }

            return (float) result;
        }
    }
}