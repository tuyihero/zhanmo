using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

public class UITools : MonoBehaviour
{

    [MenuItem("TyTools/ChangeFont")]
    public static void ChangeFont()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        Debug.Log("ChangeFont:" + selection.Length);
        foreach(var selectGO in selection)
        {
            if (selectGO is GameObject)
            {
                var texts = (selectGO as GameObject).GetComponentsInChildren<Text>();

                var font = AssetDatabase.LoadAssetAtPath<Font>("Assets\\FightCraft\\Res\\Font\\ChatFont.TTF");
                foreach (var tex in texts)
                {
                    tex.font = font;
                }
            }
        }
    }
}
