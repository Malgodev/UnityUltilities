using UnityEditor;
using UnityEngine;
namespace Malgo.Utilities.Misc
{
    [CreateAssetMenu(fileName = "SceneSwitcherConfiguration", menuName = "DevTools/Scene Switcher Configuration", order = 1)]
    public class SceneSwitcherConfiguration : ScriptableObject
    {
        public SceneAsset[] slots;
        public SceneAsset[] multiSceneSlots;
    }
}