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

        public static void AddSceneToCurrentScene(int index)
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

            // Check if scene is already loaded
            UnityEngine.SceneManagement.Scene existingScene = EditorSceneManager.GetSceneByPath(scenePath);
            if (existingScene.isLoaded)
            {
                // Scene is loaded, remove it
                if (EditorSceneManager.sceneCount > 1)
                {
                    EditorSceneManager.CloseScene(existingScene, true);
                }
                return;
            }

            // Scene not loaded, add it
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
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

        // Add Scene shortcuts (Ctrl+Alt+Shift + Q/W/E/R/T/Y/U/I/O)
        [MenuItem("DevTools/Add Scene/Add Slot 1 ^#&q")]
        public static void AddScene_Slot1()
        {
            AddSceneToCurrentScene(0);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 2 ^#&w")]
        public static void AddScene_Slot2()
        {
            AddSceneToCurrentScene(1);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 3 ^#&e")]
        public static void AddScene_Slot3()
        {
            AddSceneToCurrentScene(2);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 4 ^#&r")]
        public static void AddScene_Slot4()
        {
            AddSceneToCurrentScene(3);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 5 ^#&t")]
        public static void AddScene_Slot5()
        {
            AddSceneToCurrentScene(4);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 6 ^#&y")]
        public static void AddScene_Slot6()
        {
            AddSceneToCurrentScene(5);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 7 ^#&u")]
        public static void AddScene_Slot7()
        {
            AddSceneToCurrentScene(6);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 8 ^#&i")]
        public static void AddScene_Slot8()
        {
            AddSceneToCurrentScene(7);
        }
        [MenuItem("DevTools/Add Scene/Add Slot 9 ^#&o")]
        public static void AddScene_Slot9()
        {
            AddSceneToCurrentScene(8);
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