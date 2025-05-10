using System.Collections;
using System.Collections.Generic;
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
    }

}