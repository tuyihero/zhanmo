
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NavMeshExtension
{
    /// <summary>
    /// Custom Editor for editing vertices and exporting the mesh.
    /// </summary>
    [CustomEditor(typeof(MapPosObj))]
    [CanEditMultipleObjects]
    public class MapPosObjEditor : Editor
    {
        //navmesh object reference
        private MapPosObj script;

        private bool placing;


        void OnEnable()
        {
            script = (MapPosObj)target;
        }


        /// <summary>
        /// Custom inspector override for buttons.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            if (GUILayout.Button("ShowModel"))
            {
                script.ShowMonsterByID();
            }

            if (GUILayout.Button("RemoveShow"))
            {
                script.RemoveShow();
            }

            //if (GUILayout.Button("FitEnemies"))
            //{
            //}

        }
        
    }
}