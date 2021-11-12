using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjEditStruct
{

    //Editor Button bools
    public static bool groupEnabled1 = false;
    public static bool groupEnabled2 = false;
    public static bool groupEnabled3 = false;

    //Object
    public static GameObject obj;

    //Object transform
    public static Vector3 obvec1 = new Vector3();
    //Object position
    public static Vector3 obvec2 = new Vector3();

    //Object SortingOrder
    public static int order = 0;

    //Object SortingLayer ID
    public static int id = 0;

    //SortingLayers
    public static SortingLayer[] layers = default;

    //Object Renderer Types
    public static MeshRenderer renderer1;
    public static SkinnedMeshRenderer renderer2;

    //Missing Scripts remover Vars
    public static int _goCount;
    public static int _componentsCount;
    public static int _missingCount;
    public static bool _bHaveRun;

    //This script is made by PoH#4626
}
