
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class EleCoreConditionInfo
{
    public string _Desc;
    public EquipExAttr _Attr;
    public bool _IsAct;
}

public class UIElementCoreCondition : UIItemSelect
{
    public Text _Desc;
    public Text _Attr;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EleCoreConditionInfo)hash["InitObj"];

        ShowAttr(showItem);
    }

    public void ShowAttr(EleCoreConditionInfo coreConditionInfo)
    {
        if (coreConditionInfo._IsAct)
        {
            _Desc.text = CommonDefine.GetEnableGrayStr(1) + coreConditionInfo._Desc + "</color>";
            _Attr.text = CommonDefine.GetQualityColorStr(ITEM_QUALITY.BLUE) + coreConditionInfo._Attr.GetAttrStr() + "</color>";
        }
        else
        {
            _Desc.text = CommonDefine.GetEnableGrayStr(0) + coreConditionInfo._Desc + "</color>";
            _Attr.text = CommonDefine.GetQualityColorStr(ITEM_QUALITY.BLUE) + coreConditionInfo._Attr.GetAttrStr() + "</color>";
        }
    }


}

