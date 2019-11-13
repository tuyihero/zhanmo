
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;


public class UIEquipRefreshPackItem : UIPackItemBase
{
  
    public override void ShowItem(ItemBase showItem)
    {
        base.ShowItem(showItem);

        if (showItem == null || !showItem.IsVolid())
            return;

        if (_Num != null)
        {
            if (showItem is ItemEquip)
            {
                _Num.text = "";
            }
            else
            {
                if (_ShowedItem.ItemStackNum > 1)
                    _Num.text = _ShowedItem.ItemStackNum.ToString();
                else
                    _Num.text = "";
            }
        }
        _Icon.gameObject.SetActive(true);
    }

    protected override void ClearItem()
    {
        base.ClearItem();
        if (_Num != null)
        {
            _Num.text = "";
        }
    }
}

