using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using Tables;

public class UISellShopPack : UIBase
{
    #region static

    public delegate int GetItemPrice(List<ItemBase> items);
    public delegate void SellItems(List<ItemBase> items);

    public static void ShowSellQualitySync(List<ItemBase> toSellItems, List<ITEM_QUALITY> selectQualities, GetItemPrice getItemPrice, SellItems sellItems)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ToSellItems", toSellItems);
        hash.Add("SelectQualities", selectQualities);
        hash.Add("GetItemPrice", getItemPrice);
        hash.Add("SellItems", sellItems);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISellShopPack, UILayer.SubPopUI, hash);
    }

    public static void ShowSellLevelSync(List<ItemBase> toSellItems, List<Vector2> selectLevels, GetItemPrice getItemPrice, SellItems sellItems)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ToSellItems", toSellItems);
        hash.Add("SelectLevels", selectLevels);
        hash.Add("GetItemPrice", getItemPrice);
        hash.Add("SellItems", sellItems);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISellShopPack, UILayer.SubPopUI, hash);
    }

    #endregion

    #region 

    public UIContainerSelect _ConditionContainer;
    public UIContainerSelect _ItemsContainer;
    public UICurrencyItem _SellPrice;

    private GetItemPrice _GetItemPrice;
    private SellItems _SellItems;
    private List<ItemBase> _ShowItems = new List<ItemBase>();
    private List<ItemBase> _SelectedItems = new List<ItemBase>();
    #endregion

    #region 

    public override void Init()
    {
        base.Init();
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SelectedItems.Clear();
        _ShowItems = (List<ItemBase>)hash["ToSellItems"];
        _GetItemPrice = (GetItemPrice)hash["GetItemPrice"];
        _SellItems = (SellItems)hash["SellItems"];

        if (hash.ContainsKey("SelectQualities"))
        {
            List<ITEM_QUALITY> selectQualities = (List<ITEM_QUALITY>)hash["SelectQualities"];
            _SelectedItems.Clear();
            for (int i = 0; i < _ShowItems.Count; ++i)
            {
                if (ShopData.Instance._SellQualityTemp.Contains(_ShowItems[i].GetQuality()))
                {
                    _SelectedItems.Add(_ShowItems[i]);
                }
            }
            _ConditionContainer.InitSelectContent(selectQualities, ShopData.Instance._SellQualityTemp, OnQualitySelect, OnQualitySelect);
        }
        if (hash.ContainsKey("SelectLevels"))
        {
            List<Vector2> selectLevel = (List<Vector2>)hash["SelectLevels"];
            _SelectedItems.Clear();
            for (int i = 0; i < _ShowItems.Count; ++i)
            {
                for (int j = 0; j < ShopData.Instance._SellLevelTemp.Count; ++j)
                {
                    if (selectLevel[j].x <= _ShowItems[i].GetLevel()
                        && selectLevel[j].y >= _ShowItems[i].GetLevel())
                    {
                        _SelectedItems.Add(_ShowItems[i]);
                        break;
                    }
                }
            }
            _ConditionContainer.InitSelectContent(selectLevel, ShopData.Instance._SellLevelTemp, OnLevelSelect, OnLevelSelect);
            
        }

        _ItemsContainer.InitSelectContent(_ShowItems, _SelectedItems, OnItemSelect, OnItemUnSelect);
        _ItemsContainer.SetShowItemFinishCallFun(AfterInit);

        _SellPrice.ShowCurrency(MONEYTYPE.GOLD, 0);
    }

    public void AfterInit()
    {
        OnQualitySelect(null);
    }

    public void OnQualitySelect(object quality)
    {
        var selectQualities = _ConditionContainer.GetSelecteds<ITEM_QUALITY>();

        ShopData.Instance._SellQualityTemp = selectQualities;
        _SelectedItems.Clear();
        for (int i = 0; i < _ShowItems.Count; ++i)
        {
            if (selectQualities.Contains(_ShowItems[i].GetQuality()))
            {
                _SelectedItems.Add(_ShowItems[i]);
            }
        }
        _ItemsContainer.SetSelect(_SelectedItems);

        if (_GetItemPrice != null)
        {
            int sellGold = _GetItemPrice.Invoke(_SelectedItems);
            _SellPrice.ShowCurrency(MONEYTYPE.GOLD, sellGold);
        }
    }

    public void OnLevelSelect(object level)
    {
        var selectLevel = _ConditionContainer.GetSelecteds<Vector2>();

        ShopData.Instance._SellLevelTemp = selectLevel;
        _SelectedItems.Clear();
        for (int i = 0; i < _ShowItems.Count; ++i)
        {
            for (int j = 0; j < selectLevel.Count; ++j)
            {
                if (selectLevel[j].x <= _ShowItems[i].GetLevel()
                    && selectLevel[j].y >= _ShowItems[i].GetLevel())
                {
                    _SelectedItems.Add(_ShowItems[i]);
                    break;
                }
            }
        }
        _ItemsContainer.SetSelect(_SelectedItems);

        if (_GetItemPrice != null)
        {
            int sellGold = _GetItemPrice.Invoke(_SelectedItems);
            _SellPrice.ShowCurrency(MONEYTYPE.GOLD, sellGold);
        }
    }

    public void OnItemSelect(object itemObj)
    {
        ItemBase itemBase = itemObj as ItemBase;
        if (!_SelectedItems.Contains(itemBase))
        {
            _SelectedItems.Add(itemBase);
        }
    }

    public void OnItemUnSelect(object itemObj)
    {
        ItemBase itemBase = itemObj as ItemBase;
        if (_SelectedItems.Contains(itemBase))
        {
            _SelectedItems.Remove(itemBase);
        }
    }

    public override void Hide()
    {
        base.Hide();

        UIEquipPack.RefreshBagItems();
    }

    private void ShowSellBackTooltips(object equipObj)
    {
        ItemBase equipItem = equipObj as ItemBase;
        //UIEquipTooltips.ShowShopAsyn(equipItem, true, MONEYTYPE.GOLD,0,  new ToolTipFunc[1] {  });
    }

    public void OnBtnSell()
    {

        if (_SellItems != null)
        {
            _SellItems.Invoke(_SelectedItems);
        }

        Hide();

        //UIEquipPack.RefreshBagItems();
    }

    #endregion


}

