using UnityEngine;
using UnityEditor;
using System.Collections;

public class ClearPrefs : MonoBehaviour
{

    [MenuItem("TyTools/ClearPref")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
