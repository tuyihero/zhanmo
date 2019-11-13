
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using Tables;

public class UIEquipSetInfo : UIBase
{

    #region 

    public Text _Name;
    public UIContainerBase _AttrContainer;

    #endregion

    #region 

    public void ShowTips(ItemEquip itemEquip)
    {
        if (itemEquip == null || !itemEquip.IsVolid())
        {
            return;
        }

        var setAttrInfo = EquipSet.Instance.GetSetInfo(itemEquip.SpSetRecord);
        if (setAttrInfo == null)
            return;

        _Name.text = CommonDefine.GetQualityColorStr(ITEM_QUALITY.GREEN) + StrDictionary.GetFormatStr(itemEquip.SpSetRecord.Name) + "</color>";

        var attrs = EquipSet.Instance.GetEquipAttr(itemEquip.SpSetRecord);
        List<EquipSetAttrItem> setAttrs = new List<EquipSetAttrItem>();
        for (int i = 0; i < attrs.Count; ++i)
        {
            EquipSetAttrItem attrItem = new EquipSetAttrItem();
            attrItem.SetAttr = attrs[i];
            attrItem.IsEnable = false;
            if (i < setAttrInfo.SetEquipCnt - 1)
            {
                attrItem.IsEnable = true;
            }
            setAttrs.Add(attrItem);
        }
        _AttrContainer.InitContentItem(setAttrs, null, null);
    }
    #endregion



}

