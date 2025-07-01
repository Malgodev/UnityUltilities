using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Malgo.Utilities
{
    public class VectorUtility
    {
        /// <summary>
        /// Returns a value between 0 and 1 that represents how far 'value' is between 'a' and 'b'.
        /// </summary>
        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 ab = b - a;
            Vector3 av = value - a;

            float abLengthSquared = ab.sqrMagnitude;

            if (abLengthSquared == 0f)
                return 0f; // Avoid divide by zero; treat as at start

            float t = Vector3.Dot(av, ab) / abLengthSquared;

            return t;
        }
    }
}