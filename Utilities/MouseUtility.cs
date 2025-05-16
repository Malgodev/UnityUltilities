using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Malgo.Utilities
{
    public class MouseUtility
    {
        public static Vector2 Get2DWorldMousePosition(Camera camera)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);
            return new Vector2(worldPos.x, worldPos.y);
        }

        public static Vector3 Get3DWorldMousePosition(Camera camera, Vector3 originPosition)
        {
            Vector3 mousePos = Input.mousePosition;
            float depth = Vector3.Dot((originPosition - camera.transform.position), camera.transform.forward);
            mousePos.z = depth;
            return camera.ScreenToWorldPoint(mousePos);
        }

        public static Vector3 GetMouseWorldPositionForDragging(Camera camera, Vector3 originPosition, float heightOffset = 0)
        {
            Vector3 worldMousePosition = Get3DWorldMousePosition(camera, originPosition);
            Vector3 offset = worldMousePosition - originPosition;

            offset.z = heightOffset;

            return offset + originPosition;
        }
    }
}
