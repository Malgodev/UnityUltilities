using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Malgo.Utilities
{
    public class CustomGizmos
    {
        private static Texture2D backgroundTexture;

            public static void DrawText(Vector3 position, string text, Color textColor, float backgroundAlpha = 1f)
            {
#if UNITY_EDITOR
                if (backgroundTexture == null)
                {
                    backgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
                    {
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Point
                    };
                }

                // Update pixel color only if needed
                backgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, backgroundAlpha));
                backgroundTexture.Apply();

                var style = new GUIStyle(GUI.skin.label)
                {
                    normal =
                    {
                        textColor = textColor,
                        background = backgroundTexture
                    }
                };

                Handles.Label(position, text, style);
#endif
            }
        public static void DrawWireSphere(Vector3 position, float radius, Color color)
        {
            Handles.color = color;

            Handles.DrawWireArc(position, Vector3.up, Vector3.forward, 360, radius);
            Handles.DrawWireArc(position, Vector3.right, Vector3.up, 360, radius);
            Handles.DrawWireArc(position, Vector3.forward, Vector3.right, 360, radius);
        }
    }
}
#endif
