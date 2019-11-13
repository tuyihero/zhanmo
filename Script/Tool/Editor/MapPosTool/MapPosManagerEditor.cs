
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Adds a new Portal Manager gameobject to the scene.
/// <summary>
[CustomEditor(typeof(MapPosManager))]
public class MapPosManagerEditor : Editor
{
    //manager reference
    private MapPosManager script;


    void OnEnable()
    {
        script = (MapPosManager)target;
    }


    /// <summary>
    /// Custom inspector override for buttons.
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUIUtility.LookLikeControls();
        EditorGUILayout.Space();

        if (GUILayout.Button("New PosGroup"))
        {
            CreatePosGroup();
        }

        if (GUILayout.Button("Fit Areas"))
        {
            var passArea = script.GetComponent<FightSceneLogicPassArea>();
            if (passArea == null)
                return;

            var areas = script.GetComponentsInChildren<FightSceneAreaBase>();
            passArea._FightArea = new List<FightSceneAreaBase>(areas);
        }
    }


    /// <summary>
    /// Creates a new gameobject to use it as NavMeshObject.
    /// </summary>
    public void CreatePosGroup()
    {
        //create gameobject
        GameObject navGO = new GameObject("FightArea");
        navGO.transform.parent = script.transform;
        navGO.AddComponent<MapPosGroup>();

        Selection.activeObject = navGO;
    }

    
}

