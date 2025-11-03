using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        public static Vector3 LocalRectToWorldPosition(RectTransform uiElement, Canvas canvas, Camera camera)
        {
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, uiElement.position);
        
            // Convert screen position to world position
            // The Z distance determines how far from the camera the world position will be
            float distanceFromCamera = canvas.planeDistance;
            Vector3 worldPos = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distanceFromCamera));
        
            return worldPos;
        }
        
        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
        
        public static bool IsPointerOverUIObject(Object exclusive)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
    }
}