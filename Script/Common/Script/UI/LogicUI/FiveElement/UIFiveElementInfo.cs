
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UIFiveElementInfo : UIItemInfo
{

    #region 

    public Text _Value;

    #endregion

    #region 

    private ItemFiveElement _ShowElementItem;

    public void ShowTips(ItemFiveElement itemElement)
    {
        if (itemElement == null || !itemElement.IsVolid())
        {
            _ShowElementItem = null;
            return;
        }
        //itemEquip.CalculateCombatValue();
        _ShowElementItem = itemElement;

        if (_ShopPrice != null)
        {
            _ShopPrice.gameObject.SetActive(false);
        }

        _Name.text = _ShowElementItem.GetElementNameWithColor();

        _Value.text = StrDictionary.GetFormatStr(10001) + " " + _ShowElementItem.CombatValue;
        
        Hashtable hash = new Hashtable();
        _AttrContainer.InitContentItem(_ShowElementItem.EquipExAttrs, null, hash);
    }
    #endregion



}

