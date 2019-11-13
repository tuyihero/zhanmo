using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class EquipSetAttrItem
{
    public EquipExAttr SetAttr;
    public bool IsEnable;
}

public class UIEquipSetAttrItem : UIItemBase
{
    public Text _AttrText;

    private EquipSetAttrItem _ShowAttr;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipSetAttrItem)hash["InitObj"];

        ShowAttr(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowAttr(_ShowAttr);
    }

    public void ShowAttr(EquipSetAttrItem attr)
    {

        _ShowAttr = attr;

        string attrStr = _ShowAttr.SetAttr.GetAttrStr(false);

        if (_ShowAttr.IsEnable)
        {
            attrStr = CommonDefine.GetQualityColorStr(ITEM_QUALITY.ORIGIN) + attrStr + "</color>";
        }
        else
        {
            attrStr = CommonDefine.GetEnableGrayStr(0) + attrStr + "</color>";
        }
        _AttrText.text = attrStr;
    }


}

