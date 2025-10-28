using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ImageConverter : EditorWindow
{
    private class ImageItem
    {
        public Texture2D texture;
        public bool isSelected;

        public ImageItem(Texture2D tex)
        {
            texture = tex;
            isSelected = true; // Default to selected
        }
    }

    private List<ImageItem> inputImages = new List<ImageItem>();
    private Vector2 scrollPos;
    private Vector2 mainScrollPos;
    private int targetWidth = 512;
    private int targetHeight = 512;
    private bool keepSquare = true;
    private ResizeMode resizeMode = ResizeMode.Expand;
    private bool overrideOriginal = false;
    private string outputFolder = "Assets/ConvertedImages";
    private static readonly string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".tga", ".bmp", ".psd", ".tiff", ".gif" };

    private enum ResizeMode
    {
        Stretch,    // Extrude/stretch the image
        Expand      // Expand with transparent borders
    }

    [MenuItem("Tools/Image Converter")]
    public static void ShowWindow()
    {
        GetWindow<ImageConverter>("Image Converter");
    }

    private void OnGUI()
    {
        mainScrollPos = EditorGUILayout.BeginScrollView(mainScrollPos);
        
        GUILayout.Label("Image Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Input Images Section
        GUILayout.Label("Input Images", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Select Images from Folder", GUILayout.Height(30)))
        {
            string folderPath = EditorUtility.OpenFolderPanel("Select Folder with Images", "Assets", "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                LoadImagesFromFolder(folderPath);
            }
        }
        
        if (GUILayout.Button("Add Single Image", GUILayout.Height(30)))
        {
            string path = EditorUtility.OpenFilePanelWithFilters(
                "Select Image", 
                "Assets", 
                new string[] { "Image files", "png,jpg,jpeg,tga,bmp,psd,tiff,gif", "All files", "*" }
            );
            
            if (!string.IsNullOrEmpty(path))
            {
                AddImageFromPath(path);
            }
        }
        EditorGUILayout.EndHorizontal();

        // Drag and drop area
        Rect dropArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag & Drop Multiple Images Here", EditorStyles.helpBox);
        
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is Texture2D texture)
                        {
                            AddImageFromTexture(texture);
                        }
                    }
                }
                Event.current.Use();
                break;
        }

        if (inputImages.Count > 0)
        {
            int selectedCount = inputImages.Count(img => img.isSelected);
            EditorGUILayout.HelpBox($"{inputImages.Count} image(s) loaded | {selectedCount} selected", MessageType.Info);
        }

        if (inputImages.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All"))
            {
                foreach (var img in inputImages)
                    img.isSelected = true;
            }
            if (GUILayout.Button("Deselect All"))
            {
                foreach (var img in inputImages)
                    img.isSelected = false;
            }
            EditorGUILayout.EndHorizontal();
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
        
        for (int i = 0; i < inputImages.Count; i++)
        {
            if (inputImages[i].texture != null)
            {
                EditorGUILayout.BeginHorizontal();
                inputImages[i].isSelected = EditorGUILayout.Toggle(inputImages[i].isSelected, GUILayout.Width(20));
                EditorGUILayout.LabelField(inputImages[i].texture.name, GUILayout.Width(180));
                EditorGUILayout.LabelField($"{inputImages[i].texture.width} x {inputImages[i].texture.height}", GUILayout.Width(100));
                EditorGUILayout.ObjectField(inputImages[i].texture, typeof(Texture2D), false);
                EditorGUILayout.EndHorizontal();
            }
        }
        
        EditorGUILayout.EndScrollView();

        if (inputImages.Count > 0 && GUILayout.Button("Clear Images"))
        {
            inputImages.Clear();
        }

        EditorGUILayout.Space();

        // Max Size Display
        if (inputImages.Any(img => img.texture != null && img.isSelected))
        {
            var maxWidth = inputImages.Where(img => img.texture != null && img.isSelected).Max(img => img.texture.width);
            var maxHeight = inputImages.Where(img => img.texture != null && img.isSelected).Max(img => img.texture.height);
            EditorGUILayout.HelpBox($"Max size from selected images: {maxWidth} x {maxHeight}", MessageType.Info);
        }

        EditorGUILayout.Space();

        // Target Resolution Section
        GUILayout.Label("Target Resolution", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Width (Power of 2)", GUILayout.Width(120));
        targetWidth = EditorGUILayout.IntPopup(targetWidth, 
            new string[] { "64", "128", "256", "512", "1024", "2048", "4096" },
            new int[] { 64, 128, 256, 512, 1024, 2048, 4096 });
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Height (Power of 2)", GUILayout.Width(120));
        
        if (keepSquare)
        {
            GUI.enabled = false;
            EditorGUILayout.IntPopup(targetWidth, 
                new string[] { "64", "128", "256", "512", "1024", "2048", "4096" },
                new int[] { 64, 128, 256, 512, 1024, 2048, 4096 });
            GUI.enabled = true;
        }
        else
        {
            targetHeight = EditorGUILayout.IntPopup(targetHeight, 
                new string[] { "64", "128", "256", "512", "1024", "2048", "4096" },
                new int[] { 64, 128, 256, 512, 1024, 2048, 4096 });
        }
        EditorGUILayout.EndHorizontal();

        keepSquare = EditorGUILayout.Toggle("Keep Square (Same Width/Height)", keepSquare);
        
        if (keepSquare)
        {
            targetHeight = targetWidth;
        }

        EditorGUILayout.Space();

        // Resize Mode Section
        GUILayout.Label("Resize Mode", EditorStyles.boldLabel);
        resizeMode = (ResizeMode)EditorGUILayout.EnumPopup("Mode", resizeMode);
        
        if (resizeMode == ResizeMode.Stretch)
        {
            EditorGUILayout.HelpBox("Stretch: Image will be stretched to fit the target resolution", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("Expand: Image will be centered with transparent borders to match target resolution", MessageType.Info);
        }

        EditorGUILayout.Space();

        // Output Options Section
        GUILayout.Label("Output Options", EditorStyles.boldLabel);
        overrideOriginal = EditorGUILayout.Toggle("Override Original Images", overrideOriginal);
        
        if (!overrideOriginal)
        {
            EditorGUILayout.BeginHorizontal();
            outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Output Folder", "Assets", "");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.StartsWith(Application.dataPath))
                    {
                        outputFolder = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);

        // Convert Button
        GUI.enabled = inputImages.Any(img => img.texture != null && img.isSelected);
        if (GUILayout.Button("Convert Images", GUILayout.Height(40)))
        {
            ConvertImages();
        }
        GUI.enabled = true;
        
        EditorGUILayout.EndScrollView();
    }

    private void AddImageFromPath(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            return;
        }

        // Convert absolute path to relative Unity asset path
        string assetPath = filePath;
        if (filePath.StartsWith(Application.dataPath))
        {
            assetPath = "Assets" + filePath.Substring(Application.dataPath.Length);
        }
        
        // Load the texture
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        if (texture != null)
        {
            AddImageFromTexture(texture);
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Failed to load image. Make sure it's in the Assets folder.", "OK");
        }
    }

    private void AddImageFromTexture(Texture2D texture)
    {
        if (texture == null)
            return;

        // Check if already added
        if (!inputImages.Any(img => img.texture == texture))
        {
            inputImages.Add(new ImageItem(texture));
        }
    }

    private void LoadImagesFromFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
        {
            return;
        }

        // Get all files in the folder
        string[] files = Directory.GetFiles(folderPath);
        int addedCount = 0;

        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).ToLower();

            // Check if file has an image extension
            if (imageExtensions.Contains(extension))
            {
                // Convert absolute path to relative Unity asset path
                string assetPath = file;
                if (file.StartsWith(Application.dataPath))
                {
                    assetPath = "Assets" + file.Substring(Application.dataPath.Length);
                }

                // Load the texture
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (texture != null)
                {
                    // Check if already added
                    if (!inputImages.Any(img => img.texture == texture))
                    {
                        inputImages.Add(new ImageItem(texture));
                        addedCount++;
                    }
                }
            }
        }

        if (addedCount == 0)
        {
            EditorUtility.DisplayDialog("No New Images", "No new image files found in the selected folder.", "OK");
        }
        else
        {
            Debug.Log($"Added {addedCount} image(s) from folder.");
        }
    }
    private void ConvertImages()
    {
        var validImages = inputImages.Where(img => img.texture != null && img.isSelected).Select(img => img.texture).ToList();
        
        if (validImages.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "No valid images selected!", "OK");
            return;
        }

        if (!overrideOriginal && !Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        int successCount = 0;
        
        foreach (var texture in validImages)
        {
            try
            {
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                
                // Make texture readable
                bool wasReadable = importer.isReadable;
                importer.isReadable = true;
                AssetDatabase.ImportAsset(path);

                Texture2D newTexture;
                
                if (resizeMode == ResizeMode.Stretch)
                {
                    newTexture = StretchImage(texture, targetWidth, targetHeight);
                }
                else
                {
                    newTexture = ExpandImage(texture, targetWidth, targetHeight);
                }

                // Save the texture
                byte[] bytes = newTexture.EncodeToPNG();
                string outputPath;
                
                if (overrideOriginal)
                {
                    outputPath = path;
                }
                else
                {
                    string fileName = Path.GetFileName(path);
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);
                    string extension = Path.GetExtension(path);
                    
                    // Check for duplicate and generate unique name
                    outputPath = Path.Combine(outputFolder, fileName);
                    int counter = 1;
                    while (File.Exists(outputPath))
                    {
                        string newFileName = $"{fileNameWithoutExt}_{counter}{extension}";
                        outputPath = Path.Combine(outputFolder, newFileName);
                        counter++;
                    }
                }

                File.WriteAllBytes(outputPath, bytes);
                
                // Restore original readable state
                if (!overrideOriginal)
                {
                    importer.isReadable = wasReadable;
                    AssetDatabase.ImportAsset(path);
                }

                DestroyImmediate(newTexture);
                successCount++;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to convert {texture.name}: {e.Message}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", $"Converted {successCount} image(s) successfully!", "OK");
    }

    private Texture2D StretchImage(Texture2D source, int newWidth, int newHeight)
    {
        Texture2D result = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
        
        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                float u = x / (float)newWidth;
                float v = y / (float)newHeight;
                result.SetPixel(x, y, source.GetPixelBilinear(u, v));
            }
        }
        
        result.Apply();
        return result;
    }

    private Texture2D ExpandImage(Texture2D source, int newWidth, int newHeight)
    {
        Texture2D result = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
        
        // Fill with transparent pixels
        Color[] clearPixels = new Color[newWidth * newHeight];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = new Color(0, 0, 0, 0);
        }
        result.SetPixels(clearPixels);
        
        // Calculate centered position
        int offsetX = (newWidth - source.width) / 2;
        int offsetY = (newHeight - source.height) / 2;
        
        // Copy original image to center
        for (int y = 0; y < source.height; y++)
        {
            for (int x = 0; x < source.width; x++)
            {
                if (offsetX + x >= 0 && offsetX + x < newWidth &&
                    offsetY + y >= 0 && offsetY + y < newHeight)
                {
                    result.SetPixel(offsetX + x, offsetY + y, source.GetPixel(x, y));
                }
            }
        }
        
        result.Apply();
        return result;
    }
}