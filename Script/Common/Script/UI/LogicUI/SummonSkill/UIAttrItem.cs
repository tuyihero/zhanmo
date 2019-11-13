
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIAttrItem : UIItemBase
{
    public Text _AttrText;

    private EquipExAttr _ShowAttr;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipExAttr)hash["InitObj"];

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

        _AttrText.text = attrStr;
    }


}

