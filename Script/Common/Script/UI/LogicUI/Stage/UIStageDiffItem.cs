
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UIStageDiffItem : UIItemSelect
{
    public Text _StageName;

    private int _DiffID;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var diffId = (int)hash["InitObj"];
        ShowStage(diffId);
    }

    public void ShowStage(int diffID)
    {
        _DiffID = diffID;
        if (_DiffID < 4)
        {
            _StageName.text = StrDictionary.GetFormatStr(70000 + _DiffID);
        }
        else
        {
            _StageName.text = StrDictionary.GetFormatStr(70004, _DiffID);
        }
    }


}

