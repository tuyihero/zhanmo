
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIItemInfo : UIBase
{

    #region 

    public Text _Name;
    public Text _Level;
    public Text _Desc;
    public Text _ShopOpt;
    public UICurrencyItem _ShopPrice;
    public UIContainerBase _AttrContainer;

    #endregion

    #region 

    protected ItemBase _ShowItem;

    public virtual void ShowTips(ItemBase itemBase)
    {
        if (itemBase == null || !itemBase.IsVolid())
        {
            _ShowItem = null;
            return;
        }

        _ShowItem = itemBase;
        _Name.text = StrDictionary.GetFormatStr(_ShowItem.CommonItemRecord.NameStrDict);

        if (_ShopPrice != null)
        {
            _ShopPrice.gameObject.SetActive(false);
        }

        if (_Desc != null)
        {
            _Desc.text = _ShowItem.GetDesc();
        }
    }

    public virtual void ShowPrice(bool isBuy, MONEYTYPE priceType, int priceValue)
    {
        string buyStr = "";
        if (isBuy)
        {
            buyStr = StrDictionary.GetFormatStr(10006) + ":";
        }
        else
        {
            buyStr = StrDictionary.GetFormatStr(10005) + ":";
        }

        _ShopOpt.text = buyStr;
        _ShopPrice.gameObject.SetActive(true);
        _ShopPrice.ShowCurrency(priceType, priceValue);
    }

    #endregion



}

