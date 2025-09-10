using UnityEngine;

namespace Malgo.Utilities
{
    public class UIUtility
    {
        public static Vector2 WordPositionToLocalRect(Vector2 position, RectTransform rectTransform)
        {
            Vector2 local = rectTransform.InverseTransformPoint(position);
            Vector2 normalized = Rect.PointToNormalized(rectTransform.rect, local);

            return normalized;
        }
    }
}