#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class SettingsWindow : EditorWindow
{
    static bool scan = false;

    [MenuItem("Window/ObjectEditor/Settings")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SettingsWindow window = (SettingsWindow)EditorWindow.GetWindow(typeof(SettingsWindow));
        window.Show();
        window.title = "Settings";
        ObjEditStruct.groupEnabled1 = EditorPrefs.GetBool("ObjectEditorGroup1Enabled");
        ObjEditStruct.groupEnabled3 = EditorPrefs.GetBool("ObjectEditorGroup3Enabled");
    }

    void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Space(5);
        ObjEditStruct.groupEnabled1 = EditorGUILayout.BeginToggleGroup("Layers", ObjEditStruct.groupEnabled1);
        EditorPrefs.SetBool("ObjectEditorGroup1Enabled", ObjEditStruct.groupEnabled1);
        string tags = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Assets\ObjectEditor\Manager\TagManager.asset");
        if (GUILayout.Button("Generate Layers"))
        {
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\ProjectSettings\TagManager.asset", tags);
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndToggleGroup();
        if (tags == File.ReadAllText(Directory.GetCurrentDirectory() + @"\ProjectSettings\TagManager.asset"))
        {
            GUILayout.Label("Already generated all layers!");
        }

        GUILayout.Space(5);

        ObjEditStruct.groupEnabled3 = EditorGUILayout.BeginToggleGroup("Missing Scripts", ObjEditStruct.groupEnabled3);
        EditorPrefs.SetBool("ObjectEditorGroup3Enabled", ObjEditStruct.groupEnabled3);
        if (GUILayout.Button("Remove"))
        {
            #if UNITY_2018
                Run2018();
            #endif
            #if UNITY_2019_1_OR_NEWER
                Run2019();
            #endif
        }
        EditorGUILayout.EndToggleGroup();
        GUILayout.Space(5);
        GUILayout.EndVertical();
    }

    //Missingscripts Remover
    void Run2019()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        List<UnityEngine.Object> objectsWithDeadLinks = new List<UnityEngine.Object>();
        foreach (GameObject g in rootObjects)
        {
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                Component currentComponent = components[i];
                if (currentComponent == null)
                {
                    objectsWithDeadLinks.Add(g);
                    Selection.activeGameObject = g;
                    try
                    {
                        PrefabUtility.UnpackPrefabInstance(g, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                    }
                    catch (ArgumentException)
                    {
                    }
                    RemoveMissingScript_2019(g);
                    break;
                }
            }
        }
        if (objectsWithDeadLinks.Count > 0)
        {
            Selection.objects = objectsWithDeadLinks.ToArray();
        }
        else
        {
            Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts! Yay!");
        }
    }

    void RemoveMissingScript_2019(GameObject gameObject)
    {
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
        if (gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                RemoveMissingScript_2019(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }

    void Run2018()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        List<UnityEngine.Object> objectsWithDeadLinks = new List<UnityEngine.Object>();
        foreach (GameObject g in rootObjects)
        {
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                Component currentComponent = components[i];
                if (currentComponent == null)
                {
                    objectsWithDeadLinks.Add(g);
                    Selection.activeGameObject = g;
                    try
                    {
                        PrefabUtility.UnpackPrefabInstance(ObjEditStruct.obj, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                    }
                    catch (ArgumentException)
                    {
                    }
                    RemoveMissingScript_2018(g);
                    break;
                }
            }
        }
        if (objectsWithDeadLinks.Count > 0)
        {
            Selection.objects = objectsWithDeadLinks.ToArray();
        }
        else
        {
            Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts! Yay!");
        }
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    void RemoveMissingScript_2018(GameObject gameObject)
    {
        Component[] components = gameObject.GetComponents<Component>();
        int count = 0;
        for (int i = 0; i < components.Length; i++)
        {
            Component component = components[i];
            if (component == null)
            {
                SerializedObject sObject = new SerializedObject(gameObject);
                SerializedProperty property = sObject.FindProperty("m_Component");
                property.DeleteArrayElementAtIndex(i - count);
                count++;
                sObject.ApplyModifiedProperties();
            }
        }

        if (gameObject.transform.childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                RemoveMissingScript_2018(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}
#endif