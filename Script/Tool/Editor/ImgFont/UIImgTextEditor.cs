
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;


/// <summary>
/// Custom Editor for editing vertices and exporting the mesh.
/// </summary>
[CustomEditor(typeof(UIImgText))]
public class UIImgTextEditor : Editor
{
    //navmesh object reference
    private UIImgText script;

    private bool placing;


    void OnEnable()
    {
        script = (UIImgText)target;
    }


    /// <summary>
    /// Custom inspector override for buttons.
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        string text = GUILayout.TextField(script.text);
        if (!string.Equals(text, script.text))
        {
            script.text = text;
        }
        

    }
    
}
