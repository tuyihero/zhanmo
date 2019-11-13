
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIEquipInfoRefresh : UIBase
{

    #region 

    public UIBackPackItem _ItemIcon;
    public Text _Name;
    public Text _Level;
    public Text _Value;
    public Text _BaseAttr;
    public UIContainerBase _AttrContainer;

    #endregion

    #region 

    private ItemEquip _ShowItem;

    public void ShowTips(ItemEquip itemEquip, ItemEquip orgEquip)
    {
        if (itemEquip == null || !itemEquip.IsVolid())
        {
            _ShowItem = null;
            return;
        }

        _ItemIcon.ShowItem(_ShowItem);
        _ShowItem = itemEquip;
        _Name.text = _ShowItem.GetEquipNameWithColor();
        if (_ShowItem.RequireLevel > RoleData.SelectRole.RoleLevel)
        {
            _Level.text = StrDictionary.GetFormatStr(10000) + " " + CommonDefine.GetEnableRedStr(0) + _ShowItem.RequireLevel + "</color>";
        }
        else
        {
            _Level.text = StrDictionary.GetFormatStr(10000) + " " + _ShowItem.RequireLevel;
        }
        _Value.text = StrDictionary.GetFormatStr(10001) + " " + _ShowItem.EquipValue;
        string attrStr = _ShowItem.GetBaseAttrStr();
        if (string.IsNullOrEmpty(attrStr))
        {
            _BaseAttr.gameObject.SetActive(false);
        }
        else
        {
            _BaseAttr.gameObject.SetActive(true);
            _BaseAttr.text = attrStr;
        }
        Hashtable hash = new Hashtable();
        hash.Add("ItemEquip", _ShowItem);

        List<RefreshAttr> refreshAttrs = new List<RefreshAttr>();
        for (int i = 0; i < itemEquip.EquipExAttrs.Count; ++i)
        {
            RefreshAttr refreshAttr = new RefreshAttr();
            refreshAttr._ShowAttr = itemEquip.EquipExAttrs[i];
            if (orgEquip != null)
            {
                refreshAttr._OrgValue = orgEquip.EquipExAttrs[i].Value;
            }
            else
            {
                refreshAttr._OrgValue = 0;
            }
            refreshAttrs.Add(refreshAttr);
        }
        if (itemEquip.SpSetRecord != null)
        {
            RefreshAttr refreshAttr = new RefreshAttr();
            refreshAttr._SetAttr = true;
            refreshAttrs.Add(refreshAttr);
        }
        _AttrContainer.InitContentItem(refreshAttrs, null, hash);
    }
    #endregion



}

