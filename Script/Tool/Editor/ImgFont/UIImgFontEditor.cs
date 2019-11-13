
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;


/// <summary>
/// Custom Editor for editing vertices and exporting the mesh.
/// </summary>
[CustomEditor(typeof(UIImgFont))]
public class UIImgFontEditor : Editor
{
    //navmesh object reference
    private UIImgFont script;

    private bool placing;


    void OnEnable()
    {
        script = (UIImgFont)target;
    }


    /// <summary>
    /// Custom inspector override for buttons.
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (GUILayout.Button("FitChars"))
        {
            var children = script.GetComponentsInChildren<Transform>();
            foreach (var trans in children)
            {
                if (trans != script.transform)
                {
                    GameObject.DestroyImmediate(trans);
                }
            }
            var textures = Selection.GetFiltered(typeof(UnityEngine.Texture2D), SelectionMode.DeepAssets);
            foreach (var texture in textures)
            {
                if (texture.name.Length > 1)
                    continue;

                string dependObjPath = AssetDatabase.GetAssetPath(texture);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(dependObjPath);
                var charGO = new GameObject(texture.name);
                charGO.transform.SetParent(script.transform);
                var imgChar = charGO.AddComponent<UIImgChar>();
                imgChar._Image = sprite;
                imgChar._Char = texture.name[0];
                imgChar._CharHeight = imgChar._Image.rect.height;
                imgChar._CharWidth = imgChar._Image.rect.width;
            }
            var chars = script.GetComponentsInChildren<UIImgChar>();
            foreach (var _char in chars)
            {
                _char._CharHeight = _char._Image.rect.height;
                _char._CharWidth = _char._Image.rect.width;
            }
        }


    }

}
