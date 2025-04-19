#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class AddInputFieldUIElements
{
    [MenuItem("GameObject/UI/Custom Components/Input Fields/Standard Input Field", false, 10)]
    public static void AddStandardInputField(MenuCommand menuCommand)
    {
        CreatePrefabFromResources("Input Field UIElement", menuCommand);
    }

    [MenuItem("GameObject/UI/Custom Components/Input Fields/Float Input Field", false, 11)]
    public static void AddFloatInputField(MenuCommand menuCommand)
    {
        CreatePrefabFromResources("Float Input Field UIElement", menuCommand);
    }

    [MenuItem("GameObject/UI/Custom Components/Input Fields/Integer Input Field", false, 12)]
    public static void AddIntInputField(MenuCommand menuCommand)
    {
        CreatePrefabFromResources("Integer Input Field UIElement", menuCommand);
    }

    [MenuItem("GameObject/UI/Custom Components/Input Fields/Password Input Field", false, 13)]
    public static void AddPasswordInputField(MenuCommand menuCommand)
    {
        CreatePrefabFromResources("Password Input Field UIElement", menuCommand);
    }

    private static void CreatePrefabFromResources(string prefabName, MenuCommand menuCommand)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null)
        {
            Debug.LogError($"Prefab '{prefabName}' not found in Resources folder.");
            return;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
        Selection.activeObject = instance;
    }
}
#endif
