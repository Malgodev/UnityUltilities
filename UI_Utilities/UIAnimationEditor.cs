using UnityEditor;
using UnityEngine;

namespace Malgo.Utilities.UI
{
    [CustomEditor(typeof(UIAnimation), true)]
    public class UIAnimationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw all default properties automatically
            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true); // Skip script field

            while (property.NextVisible(false))
            {
                if (property.name == "scaleAnimationSettings")
                {
                    continue;
                }
                if (property.name == "moveAnimationSettings")
                {
                    continue;
                }
                if (property.name == "rotateAnimationSettings")
                {
                    continue;
                }

                EditorGUILayout.PropertyField(property, true);
            }

            // Find the animation type list
            SerializedProperty animListProp = serializedObject.FindProperty("uiAnimationType");
            bool hasMove = false;
            bool hasRotate = false;
            bool hasScale = false;

            for (int i = 0; i < animListProp.arraySize; i++)
            {
                SerializedProperty item = animListProp.GetArrayElementAtIndex(i);
                if (item.enumValueIndex == (int)AnimationType.Scale)
                {
                    hasScale = true;
                }
                else if (item.enumValueIndex == (int)AnimationType.Move)
                {
                    hasMove = true;
                }
                else if (item.enumValueIndex == (int)AnimationType.Rotate)
                {
                    hasRotate = true;
                }
            }

            if (hasMove)
            {
                SerializedProperty moveSettings = serializedObject.FindProperty("moveAnimationSettings");
                EditorGUILayout.PropertyField(moveSettings, true);
            }

            if (hasRotate)
            {
                SerializedProperty rotateSettings = serializedObject.FindProperty("rotateAnimationSettings");
                EditorGUILayout.PropertyField(rotateSettings, true);
            }

            if (hasScale)
            {
                SerializedProperty scaleSettings = serializedObject.FindProperty("scaleAnimationSettings");
                EditorGUILayout.PropertyField(scaleSettings, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}