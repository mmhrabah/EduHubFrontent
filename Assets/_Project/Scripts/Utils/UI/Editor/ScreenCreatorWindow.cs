using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class ScreenCreatorWindow : EditorWindow
{
    private string screenName = "NewScreen";
    private string screenTitle = "New Screen Title";
    private string screenHandle = "new_screen_handle";
    private int nextScreenIndex = 0; // Dropdown index for NextScreen
    private int previousScreenIndex = 0; // Dropdown index for PreviousScreen
    private string prefabPath = "Assets/Prefabs/Screens"; // Default path for prefabs
    private string scriptPath = "Assets/Scripts/Scenes/Definitions/UI/Screens"; // Default path for scripts

    private const string ScreenHandlePath = "Assets/Scripts/Utils/UI/ScreenHandle.cs"; // Path to the ScreenHandle enum file
    private List<string> availableScreenHandles = new List<string> { "None" }; // List of available ScreenHandles

    [MenuItem("Tools/Screen Creator")]
    public static void ShowWindow()
    {
        GetWindow<ScreenCreatorWindow>("Screen Creator");
    }

    private void OnEnable()
    {
        // Populate the availableScreenHandles list
        availableScreenHandles = FindAllScreenHandles();
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Screen", EditorStyles.boldLabel);

        screenName = EditorGUILayout.TextField("Screen Name", screenName);
        screenTitle = EditorGUILayout.TextField("Screen Title", screenTitle);

        GUILayout.BeginHorizontal();
        screenHandle = EditorGUILayout.TextField("Screen Handle", screenHandle);
        if (GUILayout.Button("Add to Enum", GUILayout.Width(100)))
        {
            AddScreenHandleToEnum(screenHandle);
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Next Screen Handle", EditorStyles.label);
        nextScreenIndex = EditorGUILayout.Popup(nextScreenIndex, availableScreenHandles.ToArray());

        GUILayout.Label("Previous Screen Handle", EditorStyles.label);
        previousScreenIndex = EditorGUILayout.Popup(previousScreenIndex, availableScreenHandles.ToArray());

        prefabPath = EditorGUILayout.TextField("Prefab Path", prefabPath);
        scriptPath = EditorGUILayout.TextField("Script Path", scriptPath);

        if (GUILayout.Button("Create Screen"))
        {
            CreateScreen();
        }
    }

    private async void CreateScreen()
    {
        // Validate the paths
        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogError("Prefab path cannot be empty.");
            return;
        }

        if (string.IsNullOrEmpty(scriptPath))
        {
            Debug.LogError("Script path cannot be empty.");
            return;
        }

        // Get selected NextScreen and PreviousScreen handles
        string nextScreenHandle = nextScreenIndex > 0 ? $"ScreenHandle.{availableScreenHandles[nextScreenIndex]}" : "ScreenHandle.None";
        string previousScreenHandle = previousScreenIndex > 0 ? $"ScreenHandle.{availableScreenHandles[previousScreenIndex]}" : "ScreenHandle.None";

        // Create the folder structure for the script
        string scriptFolderPath = Path.Combine(scriptPath, screenName);
        if (!Directory.Exists(scriptFolderPath))
        {
            Directory.CreateDirectory(scriptFolderPath);
        }

        // Generate the namespace dynamically based on the script path
        string namespacePath = scriptPath.Replace("Assets/Scripts/", "").Replace("/", ".");
        string generatedNamespace = $"Namzga.{namespacePath}.{screenName}Scripts";

        // Create the screen script
        string scriptFilePath = Path.Combine(scriptFolderPath, $"{screenName}.cs");
        string scriptContent = $@"
using Namzga.Utils.UI;

namespace {generatedNamespace}
{{
    public class {screenName} : Utils.UI.Screen
    {{
        public override bool IsScreenDataValid()
        {{
            return true;
        }}

        private void Awake()
        {{
            Title = ""{screenTitle}"";
            Previous = {previousScreenHandle};
            Next = {nextScreenHandle};
        }}
    }}
}}
";
        File.WriteAllText(scriptFilePath, scriptContent);

        // Refresh the AssetDatabase to ensure the script is compiled
        AssetDatabase.Refresh();

        // Delay the prefab creation and script attachment until the script is compiled
        EditorApplication.delayCall += () => AttachScriptToPrefab(scriptFilePath);
    }

    private void AttachScriptToPrefab(string scriptFilePath)
    {
        // Wait for the script to compile and load the type
        MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptFilePath);
        System.Type scriptType = monoScript?.GetClass();

        if (scriptType == null)
        {
            Debug.LogError($"Failed to load script type for '{scriptFilePath}'. Ensure the script is compiled.");
            return;
        }

        // Create the folder structure for the prefab
        string prefabFolderPath = Path.Combine(prefabPath, screenName);
        if (!Directory.Exists(prefabFolderPath))
        {
            Directory.CreateDirectory(prefabFolderPath);
        }

        // Create the screen prefab
        GameObject screenPrefab = new GameObject(screenName);

        // Attach the generated script to the prefab
        screenPrefab.AddComponent(scriptType);

        string prefabFilePath = Path.Combine(prefabFolderPath, $"{screenName}.prefab");
        PrefabUtility.SaveAsPrefabAsset(screenPrefab, prefabFilePath);
        DestroyImmediate(screenPrefab);

        Debug.Log($"Screen prefab created at: {prefabFilePath}");
    }

    private void AddScreenHandleToEnum(string newHandle)
    {
        if (string.IsNullOrEmpty(newHandle))
        {
            Debug.LogError("Screen Handle cannot be empty.");
            return;
        }

        if (!File.Exists(ScreenHandlePath))
        {
            Debug.LogError($"ScreenHandle enum file not found at path: {ScreenHandlePath}");
            return;
        }

        var lines = File.ReadAllLines(ScreenHandlePath).ToList();
        int insertIndex = lines.FindLastIndex(line => line.Contains("}")) - 1; // Find the last line before the closing brace

        if (lines.Any(line => line.Contains(newHandle)))
        {
            Debug.LogWarning($"ScreenHandle '{newHandle}' already exists in the enum.");
            return;
        }

        lines.Insert(insertIndex, $"    {newHandle},");
        File.WriteAllLines(ScreenHandlePath, lines);

        AssetDatabase.Refresh();
        availableScreenHandles = FindAllScreenHandles(); // Refresh the dropdown list

        Debug.Log($"ScreenHandle '{newHandle}' added to the ScreenHandle enum.");
    }

    private List<string> FindAllScreenHandles()
    {
        // Find all values in the ScreenHandle enum
        if (!File.Exists(ScreenHandlePath))
        {
            Debug.LogError($"ScreenHandle enum file not found at path: {ScreenHandlePath}");
            return new List<string> { "None" };
        }

        var lines = File.ReadAllLines(ScreenHandlePath);
        return lines
            .Where(line => line.Trim().EndsWith(","))
            .Select(line => line.Trim().TrimEnd(','))
            .ToList();
    }
}