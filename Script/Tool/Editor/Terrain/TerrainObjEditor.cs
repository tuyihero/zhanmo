
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TerrainTool
{
    /// <summary>
    /// Custom Editor for editing vertices and exporting the mesh.
    /// </summary>
    [CustomEditor(typeof(TerrainObj))]
    [CanEditMultipleObjects]
    public class TerrainObjEditor : Editor
    {
        //navmesh object reference
        private TerrainObj script;

        void OnEnable()
        {
            script = (TerrainObj)target;
        }


        /// <summary>
        /// Custom inspector override for buttons.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            if (GUILayout.Button("SetHeight"))
            {
                SetHeight();
            }

            if (GUILayout.Button("SetAlpha"))
            {
                SetTexture();
            }

            if (GUILayout.Button("ResetHeight"))
            {
                ResetHeight();
            }
        }

        private void SetHeight()
        {
            Vector3 testRayOrigin = new Vector3(0, 100, 0);
            Vector3 testRayDest = new Vector3(0, -100, 0);

            float maxHeight = 0;
            float[,] heightMap = new float[script._TargetTerrain.terrainData.heightmapWidth, script._TargetTerrain.terrainData.heightmapHeight];
            float deltaX = script._TargetTerrain.terrainData.size.x / script._TargetTerrain.terrainData.heightmapWidth;
            float deltaY = script._TargetTerrain.terrainData.size.z / script._TargetTerrain.terrainData.heightmapWidth;
            for (int i = 0; i < script._TargetTerrain.terrainData.heightmapWidth; ++i)
            {
                for (int j = 0; j < script._TargetTerrain.terrainData.heightmapHeight; ++j)
                {
                    testRayOrigin.x = i * deltaX + script.transform.position.x;
                    testRayOrigin.z = j * deltaY + script.transform.position.z;
                    Ray ray = new Ray(testRayOrigin, testRayDest);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        heightMap[i, j] = hit.point.y;
                        if (hit.point.y > maxHeight)
                        {
                            maxHeight = hit.point.y;
                        }

                    }
                    else
                    {
                        heightMap[i, j] = 0;
                    }
                }
            }

            var terrainSize = script._TargetTerrain.terrainData.size;
            terrainSize.y = maxHeight;
            script._TargetTerrain.terrainData.size = terrainSize;
            Debug.Log("Terrian size:" + script._TargetTerrain.terrainData.size + "," + script._TargetTerrain.terrainData.detailHeight);
            for (int i = 0; i < script._TargetTerrain.terrainData.heightmapWidth; ++i)
            {
                for (int j = 0; j < script._TargetTerrain.terrainData.heightmapHeight; ++j)
                {
                    script._TargetTerrain.terrainData.SetHeights(i, j, new[,] { { heightMap[i, j] / maxHeight, heightMap[i, j] / maxHeight } });
                }
            }
        }

        private void SetTexture()
        {
            var mat = script.GetComponent<MeshRenderer>().sharedMaterial;
            var controlTex = mat.GetTexture("_Control") as Texture2D;
            float fixX = controlTex.width / script._TargetTerrain.terrainData.alphamapWidth;
            float fixY = controlTex.height / script._TargetTerrain.terrainData.alphamapHeight;
            Debug.Log("controlTex:" + controlTex.name);
            float[,,] maps = script._TargetTerrain.terrainData.GetAlphamaps(0, 0, script._TargetTerrain.terrainData.alphamapWidth, script._TargetTerrain.terrainData.alphamapHeight);

            for (int y = 0; y < script._TargetTerrain.terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < script._TargetTerrain.terrainData.alphamapWidth; x++)
                {
                    int controlX = Mathf.Clamp( Mathf.FloorToInt( fixX * x), 0, (int)controlTex.width);
                    int controlY = Mathf.Clamp(Mathf.FloorToInt(fixY * y), 0, (int)controlTex.height);
                    var pix = controlTex.GetPixel(controlY, controlX);
                    maps[x, y, 0] = pix.r;
                    maps[x, y, 1] = pix.g;
                    maps[x, y, 2] = pix.b;
                    maps[x, y, 3] = pix.a;
                }
            }

            script._TargetTerrain.terrainData.SetAlphamaps(0, 0, maps);
        }

        private void ResetHeight()
        {
            for (int i = 0; i < script._TargetTerrain.terrainData.heightmapWidth; ++i)
            {
                for (int j = 0; j < script._TargetTerrain.terrainData.heightmapHeight; ++j)
                {
                    script._TargetTerrain.terrainData.SetHeights(i, j, new[,] { { 0.0f, 0.0f } });
                }
            }
        }
    }
}