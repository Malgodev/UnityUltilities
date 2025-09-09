using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace Malgo.Utilities.Misc
{
    public class SceneSwitcher
    {
        public static void SwitchToScene(int index)
        {
            SceneAsset[] scenes = AssetDatabase.FindAssets($"t:{nameof(SceneSwitcherConfiguration)}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<SceneSwitcherConfiguration>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault(config => config != null && config.slots != null && index >= 0 && index < config.slots.Length)
                ?.slots;
            if (scenes == null || scenes.Length == 0 || index < 0 || index >= scenes.Length)
            {
                return;
            }
            SceneAsset sceneToLoad = scenes[index];
            if (sceneToLoad == null)
            {
                return;
            }
            string scenePath = AssetDatabase.GetAssetPath(sceneToLoad);
            if (string.IsNullOrEmpty(scenePath))
            {
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }
            EditorSceneManager.OpenScene(scenePath);
        }
        [MenuItem("DevTools/Switch Scene/Slot 1 ^#&1")]
        public static void SwitchToScene_Slot1()
        {
            SwitchToScene(0);
        }
        [MenuItem("DevTools/Switch Scene/Slot 2 ^#&2")]
        public static void SwitchToScene_Slot2()
        {
            SwitchToScene(1);
        }
        [MenuItem("DevTools/Switch Scene/Slot 3 ^#&3")]
        public static void SwitchToScene_Slot3()
        {
            SwitchToScene(2);
        }
        [MenuItem("DevTools/Switch Scene/Slot 4 ^#&4")]
        public static void SwitchToScene_Slot4()
        {
            SwitchToScene(3);
        }
        [MenuItem("DevTools/Switch Scene/Slot 5 ^#&5")]
        public static void SwitchToScene_Slot5()
        {
            SwitchToScene(4);
        }
        [MenuItem("DevTools/Switch Scene/Slot 6 ^#&6")]
        public static void SwitchToScene_Slot6()
        {
            SwitchToScene(5);
        }
        [MenuItem("DevTools/Switch Scene/Slot 7 ^#&7")]
        public static void SwitchToScene_Slot7()
        {
            SwitchToScene(6);
        }
        [MenuItem("DevTools/Switch Scene/Slot 8 ^#&8")]
        public static void SwitchToScene_Slot8()
        {
            SwitchToScene(7);
        }
        [MenuItem("DevTools/Switch Scene/Slot 9 ^#&9")]
        public static void SwitchToScene_Slot9()
        {
            SwitchToScene(8);
        }
        [MenuItem("DevTools/Switch Scene/Multi Scene ^#&k")]
        public static void SwitchToMultiScene()
        {
            SceneAsset[] multiScenes = AssetDatabase.FindAssets($"t:{nameof(SceneSwitcherConfiguration)}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<SceneSwitcherConfiguration>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault(config => config != null && config.multiSceneSlots != null)
                ?.multiSceneSlots;
            if (multiScenes == null || multiScenes.Length == 0)
            {
                return;
            }
            // Open first scene
            SceneAsset firstScene = multiScenes.FirstOrDefault();
            if (firstScene == null)
            {
                return;
            }
            string firstScenePath = AssetDatabase.GetAssetPath(firstScene);
            if (string.IsNullOrEmpty(firstScenePath))
            {
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }
            EditorSceneManager.OpenScene(firstScenePath, OpenSceneMode.Single);
            // Open all scene to the editor
            foreach (var scene in multiScenes.Skip(1))
            {
                string scenePath = AssetDatabase.GetAssetPath(scene);
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                }
            }
        }
    }
}