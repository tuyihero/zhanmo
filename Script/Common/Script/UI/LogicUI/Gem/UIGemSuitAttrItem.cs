
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIGemSuitAttrItem : UIItemBase
{
    public Text _AttrText;

    private GemSetRecord _GemSetRecord;
    private EquipExAttr _ShowAttr;
    private int _Idx;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipExAttr)hash["InitObj"];
        _GemSetRecord = (GemSetRecord)hash["GetSetRecord"];
        _Idx = (int)hash["InitIdx"];

        ShowAttr(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowAttr(_ShowAttr);
    }

    public void ShowAttr(EquipExAttr attr)
    {

        _ShowAttr = attr;

        string attrStr = _ShowAttr.GetAttrStr();


        if (_ShowAttr.AttrParams[0] == 0 || (_ShowAttr.AttrParams.Count > 1 && _ShowAttr.AttrParams[1] == 0))
        {
            attrStr = CommonDefine.GetEnableGrayStr(0) + attrStr + "</color>";
            attrStr += CommonDefine.GetEnableRedStr(0) + "</color>";
        }
        else
        {
            attrStr = CommonDefine.GetEnableGrayStr(1) + attrStr + "</color>";
            attrStr += CommonDefine.GetEnableRedStr(1) + "</color>";
        }

        _AttrText.text = attrStr;
    }


}

