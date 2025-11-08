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
        
        public static Vector2 WordPositionToLocalRect(Vector3 position, RectTransform rectTransform)
        {
            Vector3 local = rectTransform.InverseTransformPoint(position);
            Vector3 normalized = Rect.PointToNormalized(rectTransform.rect, local);

            return normalized;
        }

        public static TransformData WorldTransformToUITransform(
            Transform target,
            Vector3 worldSize,
            Camera cam,
            RectTransform rectTransform,
            Canvas canvas)
        {
            TransformData result = new TransformData();

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Vector3 screenCenter = cam.WorldToScreenPoint(target.position);

            Vector3 rightWorld = target.position + target.right * (worldSize.x * 0.5f);
            Vector3 upWorld = target.position + target.up * (worldSize.y * 0.5f);

            Vector3 screenRight = cam.WorldToScreenPoint(rightWorld);
            Vector3 screenUp = cam.WorldToScreenPoint(upWorld);

            float screenWidth = Mathf.Abs(screenRight.x - screenCenter.x) * 2f;
            float screenHeight = Mathf.Abs(screenUp.y - screenCenter.y) * 2f;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,
                screenCenter, cam, out Vector2 localPoint);

            result.position = localPoint;

            // Debug.Log(result.position);
            
            float scaleFactor = canvas.scaleFactor;
            result.scale = new Vector3(screenWidth / scaleFactor, screenHeight / scaleFactor, 1f);

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