#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
using System;
using System.Reflection;

[ExecuteInEditMode]
public class ObjectEditor : MonoBehaviour
{
    private void Update()
    {
        ObjEditStruct.obj = gameObject;
        ObjEditStruct.layers = getlay();
        ObjEditStruct.groupEnabled1 = EditorPrefs.GetBool("ObjectEditorGroup1Enabled");
        ObjEditStruct.groupEnabled2 = EditorPrefs.GetBool("ObjectEditorGroup2Enabled");
        if (ObjEditStruct.obj)
        {
            if (ObjEditStruct.obj.GetComponent<MeshRenderer>())
            {
                ObjEditStruct.renderer1 = ObjEditStruct.obj.GetComponent<MeshRenderer>();
                ObjEditStruct.id = ObjEditStruct.renderer1.sortingLayerID;
                ObjEditStruct.order = ObjEditStruct.renderer1.sortingOrder;
            }
            if (ObjEditStruct.obj.GetComponent<SkinnedMeshRenderer>())
            {
                ObjEditStruct.renderer2 = ObjEditStruct.obj.GetComponent<SkinnedMeshRenderer>();
                ObjEditStruct.id = ObjEditStruct.renderer2.sortingLayerID;
                ObjEditStruct.order = ObjEditStruct.renderer2.sortingOrder;
            }
            ObjEditStruct.obvec1.x = ObjEditStruct.obj.gameObject.transform.localScale.x;
            ObjEditStruct.obvec1.y = ObjEditStruct.obj.gameObject.transform.localScale.y;
            ObjEditStruct.obvec1.z = ObjEditStruct.obj.gameObject.transform.localScale.z;
            ObjEditStruct.obvec2.x = ObjEditStruct.obj.gameObject.transform.position.x;
            ObjEditStruct.obvec2.y = ObjEditStruct.obj.gameObject.transform.position.y;
            ObjEditStruct.obvec2.z = ObjEditStruct.obj.gameObject.transform.position.z;
        }

    }

    public static SortingLayer[] getlay()
    {
        var layers = SortingLayer.layers;
        var names = layers.Select(l => l.name).ToArray();
        return layers;
    }
}

[CustomEditor(typeof(ObjectEditor))]
public class ObjectEditorEditor : Editor
{
    private ObjectEditor myClass;
    public override void OnInspectorGUI()
    {
        myClass = target as ObjectEditor;
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Space(5);
        EditorGUI.BeginChangeCheck();
        ObjEditStruct.obj = myClass.gameObject;
        if (EditorGUI.EndChangeCheck())
        {
            if (ObjEditStruct.obj)
            {
                if (ObjEditStruct.obj.GetComponent<MeshRenderer>())
                {
                    ObjEditStruct.renderer1 = ObjEditStruct.obj.GetComponent<MeshRenderer>();
                    ObjEditStruct.id = ObjEditStruct.renderer1.sortingLayerID;
                    ObjEditStruct.order = ObjEditStruct.renderer1.sortingOrder;
                }
                if (ObjEditStruct.obj.GetComponent<SkinnedMeshRenderer>())
                {
                    ObjEditStruct.renderer2 = ObjEditStruct.obj.GetComponent<SkinnedMeshRenderer>();
                    ObjEditStruct.id = ObjEditStruct.renderer2.sortingLayerID;
                    ObjEditStruct.order = ObjEditStruct.renderer2.sortingOrder;
                }
                ObjEditStruct.obvec1.x = ObjEditStruct.obj.gameObject.transform.localScale.x;
                ObjEditStruct.obvec1.y = ObjEditStruct.obj.gameObject.transform.localScale.y;
                ObjEditStruct.obvec1.z = ObjEditStruct.obj.gameObject.transform.localScale.z;
                ObjEditStruct.obvec2.x = ObjEditStruct.obj.gameObject.transform.position.x;
                ObjEditStruct.obvec2.y = ObjEditStruct.obj.gameObject.transform.position.y;
                ObjEditStruct.obvec2.z = ObjEditStruct.obj.gameObject.transform.position.z;
            }
        }
        if (ObjEditStruct.obj)
        {
            if (ObjEditStruct.obj.GetComponent<MeshRenderer>())
            {
                ObjEditStruct.renderer1 = ObjEditStruct.obj.GetComponent<MeshRenderer>();
                ObjEditStruct.id = EditorGUILayout.IntPopup("\"" + ObjEditStruct.obj.name + "\" SortingLayer:", ObjEditStruct.id, GetSortingLayerNames(), GetSortingLayerUniqueIDs());
                ObjEditStruct.order = EditorGUILayout.IntSlider("\"" + ObjEditStruct.obj.name + "\" SortingOrder:", ObjEditStruct.order, -32768, 32767);
                GUILayout.Space(5);
                ObjEditStruct.groupEnabled2 = EditorGUILayout.BeginToggleGroup("\"" + ObjEditStruct.obj.name + "\" Transform", ObjEditStruct.groupEnabled2);
                EditorPrefs.SetBool("ObjectEditorGroup2Enabled", ObjEditStruct.groupEnabled2);
                GUILayout.BeginHorizontal();
                GUILayout.Label("\"" + ObjEditStruct.obj.name + "\" size:");
                GUILayout.Label("X");
                ObjEditStruct.obvec1.x = EditorGUILayout.FloatField(ObjEditStruct.obvec1.x);
                GUILayout.Label("Y");
                ObjEditStruct.obvec1.y = EditorGUILayout.FloatField(ObjEditStruct.obvec1.y);
                GUILayout.Label("Z");
                ObjEditStruct.obvec1.z = EditorGUILayout.FloatField(ObjEditStruct.obvec1.z);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("\"" + ObjEditStruct.obj.name + "\" pos.:");
                GUILayout.Label("X");
                ObjEditStruct.obvec2.x = EditorGUILayout.FloatField(ObjEditStruct.obvec2.x);
                GUILayout.Label("Y");
                ObjEditStruct.obvec2.y = EditorGUILayout.FloatField(ObjEditStruct.obvec2.y);
                GUILayout.Label("Z");
                ObjEditStruct.obvec2.z = EditorGUILayout.FloatField(ObjEditStruct.obvec2.z);
                GUILayout.EndHorizontal();
                EditorGUILayout.EndToggleGroup();
                if (GUILayout.Button("Change"))
                {
                    ObjEditStruct.renderer1.sortingOrder = ObjEditStruct.order;
                    ObjEditStruct.renderer1.sortingLayerID = ObjEditStruct.id;
                    ObjEditStruct.obj.gameObject.transform.localScale = new Vector3(ObjEditStruct.obvec1.x, ObjEditStruct.obvec1.y, ObjEditStruct.obvec1.z);
                    ObjEditStruct.obj.gameObject.transform.position = new Vector3(ObjEditStruct.obvec2.x, ObjEditStruct.obvec2.y, ObjEditStruct.obvec2.z);
                }
                GUILayout.Space(5);
                GUILayout.EndVertical();
            }
            if (ObjEditStruct.obj.GetComponent<SkinnedMeshRenderer>())
            {
                ObjEditStruct.renderer2 = ObjEditStruct.obj.GetComponent<SkinnedMeshRenderer>();
                ObjEditStruct.id = EditorGUILayout.IntPopup("\"" + ObjEditStruct.obj.name + "\" SortingLayer:", ObjEditStruct.id, GetSortingLayerNames(), GetSortingLayerUniqueIDs());
                ObjEditStruct.order = EditorGUILayout.IntSlider("\"" + ObjEditStruct.obj.name + "\" SortingOrder:", ObjEditStruct.order, -32768, 32767);
                GUILayout.Space(5);
                ObjEditStruct.groupEnabled2 = EditorGUILayout.BeginToggleGroup("\"" + ObjEditStruct.obj.name + "\" Transform", ObjEditStruct.groupEnabled2);
                EditorPrefs.SetBool("ObjectEditorGroup2Enabled", ObjEditStruct.groupEnabled2);
                GUILayout.BeginHorizontal();
                GUILayout.Label("\"" + ObjEditStruct.obj.name + "\" size:");
                GUILayout.Label("X");
                ObjEditStruct.obvec1.x = EditorGUILayout.FloatField(ObjEditStruct.obvec1.x);
                GUILayout.Label("Y");
                ObjEditStruct.obvec1.y = EditorGUILayout.FloatField(ObjEditStruct.obvec1.y);
                GUILayout.Label("Z");
                ObjEditStruct.obvec1.z = EditorGUILayout.FloatField(ObjEditStruct.obvec1.z);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("\"" + ObjEditStruct.obj.name + "\" pos.:");
                GUILayout.Label("X");
                ObjEditStruct.obvec2.x = EditorGUILayout.FloatField(ObjEditStruct.obvec2.x);
                GUILayout.Label("Y");
                ObjEditStruct.obvec2.y = EditorGUILayout.FloatField(ObjEditStruct.obvec2.y);
                GUILayout.Label("Z");
                ObjEditStruct.obvec2.z = EditorGUILayout.FloatField(ObjEditStruct.obvec2.z);
                GUILayout.EndHorizontal();
                EditorGUILayout.EndToggleGroup();
                if (GUILayout.Button("Change"))
                {
                    ObjEditStruct.renderer2.sortingOrder = ObjEditStruct.order;
                    ObjEditStruct.renderer2.sortingLayerID = ObjEditStruct.id;
                    ObjEditStruct.obj.gameObject.transform.localScale = new Vector3(ObjEditStruct.obvec1.x, ObjEditStruct.obvec1.y, ObjEditStruct.obvec1.z);
                    ObjEditStruct.obj.gameObject.transform.position = new Vector3(ObjEditStruct.obvec2.x, ObjEditStruct.obvec2.y, ObjEditStruct.obvec2.z);
                }
                GUILayout.Space(5);
                GUILayout.EndVertical();
            }
            if (!ObjEditStruct.obj.GetComponent<MeshRenderer>() && !ObjEditStruct.obj.GetComponent<SkinnedMeshRenderer>())
            {
                GUILayout.Space(5);
                GUILayout.Space(5);
                ObjEditStruct.groupEnabled2 = EditorGUILayout.BeginToggleGroup("\"" + ObjEditStruct.obj.name + "\" Transform", ObjEditStruct.groupEnabled2);
                EditorPrefs.SetBool("ObjectEditorGroup2Enabled", ObjEditStruct.groupEnabled2);
                GUILayout.BeginHorizontal();
                GUILayout.Label("\"" + ObjEditStruct.obj.name + "\" size:");
                GUILayout.Label("X");
                ObjEditStruct.obvec1.x = EditorGUILayout.FloatField(ObjEditStruct.obvec1.x);
                GUILayout.Label("Y");
                ObjEditStruct.obvec1.y = EditorGUILayout.FloatField(ObjEditStruct.obvec1.y);
                GUILayout.Label("Z");
                ObjEditStruct.obvec1.z = EditorGUILayout.FloatField(ObjEditStruct.obvec1.z);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("\"" + ObjEditStruct.obj.name + "\" pos.:");
                GUILayout.Label("X");
                ObjEditStruct.obvec2.x = EditorGUILayout.FloatField(ObjEditStruct.obvec2.x);
                GUILayout.Label("Y");
                ObjEditStruct.obvec2.y = EditorGUILayout.FloatField(ObjEditStruct.obvec2.y);
                GUILayout.Label("Z");
                ObjEditStruct.obvec2.z = EditorGUILayout.FloatField(ObjEditStruct.obvec2.z);
                GUILayout.EndHorizontal();
                EditorGUILayout.EndToggleGroup();
                if (GUILayout.Button("Change"))
                {
                    ObjEditStruct.obj.gameObject.transform.localScale = new Vector3(ObjEditStruct.obvec1.x, ObjEditStruct.obvec1.y, ObjEditStruct.obvec1.z);
                    ObjEditStruct.obj.gameObject.transform.position = new Vector3(ObjEditStruct.obvec2.x, ObjEditStruct.obvec2.y, ObjEditStruct.obvec2.z);
                }
                GUILayout.Space(5);
                GUILayout.EndVertical();
            }
        }
        if (!ObjEditStruct.obj)
        {
            GUILayout.Space(5);
            GUILayout.EndVertical();
        }
    }

    public static SortingLayer[] getlay()
    {
        var layers = SortingLayer.layers;
        var names = layers.Select(l => l.name).ToArray();
        return layers;
    }

    public static string[] getnames(SortingLayer[] layers)
    {
        var names = layers.Select(l => l.name).ToArray();
        return names;
    }

    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public int[] GetSortingLayerUniqueIDs()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
    }
}
#endif
