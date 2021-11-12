using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SettingsWindow : EditorWindow
{
    [MenuItem("Window/ObjectEditor/Settings")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SettingsWindow window = (SettingsWindow)EditorWindow.GetWindow(typeof(SettingsWindow));
        window.Show();
        window.title = "Settings";
    }

    void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);

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

        GUILayout.EndVertical();
    }

}
