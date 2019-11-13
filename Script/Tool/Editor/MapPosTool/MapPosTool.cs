
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class MapPosTool : Editor
{

    [MenuItem("TyTools/Map/MapPosRoot")]
    public static void InitMapPosRoot()
    {
        var obj = GameObject.Find("MapPosManager");
        if (obj == null)
        {
            GameObject navGO = new GameObject("MapPosManager");
            navGO.AddComponent<MapPosManager>();
        }
    }

    
}
