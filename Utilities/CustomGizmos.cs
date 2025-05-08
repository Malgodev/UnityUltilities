using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Malgo.Utilities
{
    public class CustomGizmos
    {
        public static void DrawText(Vector3 position, string text, Color textColor, float backgroundAlpha = 1f)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);

            style.normal.textColor = textColor;

            Texture2D backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, backgroundAlpha));
            backgroundTexture.Apply();
            style.normal.background = backgroundTexture;

            Handles.Label(position, text, style);
        }
    }
}
#endif
