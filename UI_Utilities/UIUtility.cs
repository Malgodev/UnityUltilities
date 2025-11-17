using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Malgo.Utilities
{
    public static class UIUtility
    {
        public struct TransformData
        {
            public Vector3 position;
            public Vector3 scale;
        }
        
        public static Vector2 WordPositionToLocalRect(Vector2 position, RectTransform rectTransform)
        {
            Vector2 local = rectTransform.InverseTransformPoint(position);
            Vector2 normalized = Rect.PointToNormalized(rectTransform.rect, local);

            return normalized;
        }
        
        public static Vector3 WordPositionToUIPosition(Vector3 position, Camera cam)
        {
            Vector3 screenPosition = cam.WorldToScreenPoint(position);
            return screenPosition;
        }

        // Note that this only works for Screen Space - Overlay canvas
        public static TransformData WorldTransformToUITransform(
            Transform target,
            Vector3 worldSize,
            Camera cam)
        {
            TransformData result = new TransformData();

            Vector3 screenPos = WordPositionToUIPosition(target.position, cam);
            Vector3 botLeftPos = target.position - worldSize / 2;
            Vector3 topRightPos = target.position + worldSize / 2;
            
            Vector3 screenBotLeftPos = WordPositionToUIPosition(botLeftPos, cam);
            Vector3 screenTopRightPos = WordPositionToUIPosition(topRightPos, cam);

            result.position = screenPos;
            result.scale = screenTopRightPos - screenBotLeftPos;
            
            return result;
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