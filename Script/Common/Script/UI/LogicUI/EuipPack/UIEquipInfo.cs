
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UIEquipInfo : UIItemInfo
{

    #region 

    public Text _LengendaryName;
    public Text _Value;
    public Text _ProfessionLimit;
    public Text _BaseAttr;
    public Text _ModelAttr;

    #endregion

    #region 

    private ItemEquip _ShowEquip;

    public void ShowTips(ItemEquip itemEquip)
    {
        if (itemEquip == null || !itemEquip.IsVolid())
        {
            _ShowEquip = null;
            return;
        }
        //itemEquip.CalculateCombatValue();
        _ShowEquip = itemEquip;

        if (_ShopPrice != null)
        {
            _ShopPrice.gameObject.SetActive(false);
        }

        if (_ShowEquip.IsLegandaryEquip())
        {
            _LengendaryName.gameObject.SetActive(true);
            _LengendaryName.text = _ShowEquip.GetEquipLegandaryName();
        }
        else
        {
            _LengendaryName.gameObject.SetActive(false);
        }

        _Name.text = _ShowEquip.GetEquipNameWithColor();
        if (_ShowEquip.RequireLevel > RoleData.SelectRole.RoleLevel)
        {
            _Level.text = StrDictionary.GetFormatStr(10000) + " " + CommonDefine.GetEnableRedStr(0) + _ShowEquip.RequireLevel + "</color>";
        }
        else
        {
            _Level.text = StrDictionary.GetFormatStr(10000) + " " + _ShowEquip.RequireLevel;
        }
        _Value.text = StrDictionary.GetFormatStr(2300077) + " " + _ShowEquip.CombatValue;

        if (_ProfessionLimit != null)
        {
            if(!_ShowEquip.IsMatchRole(RoleData.SelectRole.Profession))
            {
                _ProfessionLimit.gameObject.SetActive(true);
            }
            else
            {
                _ProfessionLimit.gameObject.SetActive(false);
            }
        }

        string attrStr = _ShowEquip.GetBaseAttrStr();
        if (string.IsNullOrEmpty(attrStr))
        {
            _BaseAttr.gameObject.SetActive(false);
        }
        else
        {
            _BaseAttr.gameObject.SetActive(true);
            _BaseAttr.text = attrStr;
        }

        if (_ShowEquip.BaseModelAttrs.Count > 0)
        {
            _ModelAttr.gameObject.SetActive(true);
            _ModelAttr.text = _ShowEquip.BaseModelAttrs[0].GetAttrStr();
        }
        else
        {
            _ModelAttr.gameObject.SetActive(false);
        }
        Hashtable hash = new Hashtable();
        hash.Add("ItemEquip", _ShowEquip);
        _AttrContainer.InitContentItem(itemEquip.EquipExAttrs, null, hash);
    }
    #endregion



}

