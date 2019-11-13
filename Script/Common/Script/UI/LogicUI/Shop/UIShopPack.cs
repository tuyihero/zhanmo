using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIShopPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIShopPack, UILayer.PopUI, hash);
    }

    public static void RefreshShopItems()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIShopPack>(UIConfig.UIShopPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public UITagPanel _TagPanel;
    public UIContainerSelect _ShopItemContainer;
    public Text _Desc;
    public Text _Name;

    private ItemBase _SelectedItem;
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _TagPanel.ShowPage(0);
        OnPage(0);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnPage(int page)
    {
        if (page == 0)
        {
            if (ShopData.Instance._RefreshShopItemsFlag)
            {
                ShopData.Instance.RefreshShopItem();
            }
            _ShopItemContainer.InitSelectContent(ShopData.Instance._ShopItems, new List<ItemShop>() { ShopData.Instance._ShopItems[0]}, OnSelectItem);
        }
        else if(page == 1)
        {
            //ShopData.Instance.RefreshGambling();
            List<ItemBase> gamblingItems = new List<ItemBase>();
            List<ItemBase> selectItems = new List<ItemBase>();

            for (int i = 0; i < ShopData.Instance._GamblingEquips.Count; ++i)
            {
                gamblingItems.Add(ShopData.Instance._GamblingEquips[i]);

                if (selectItems.Count == 0 && ShopData.Instance._GamblingEquips[i].IsVolid())
                {
                    selectItems.Add(ShopData.Instance._GamblingEquips[i]);
                }
            }
            for (int i = 0; i < ShopData.Instance._GamblingCores.Count; ++i)
            {
                gamblingItems.Add(ShopData.Instance._GamblingCores[i]);
            }

            if (gamblingItems.Count > 0)
            {
                _ShopItemContainer.InitSelectContent(gamblingItems, selectItems, OnSelectItem);
            }
            else
            {
                _ShopItemContainer.InitSelectContent(gamblingItems, null, OnSelectItem);
            }
        }
    }

    public void OnSelectItem(object shopItemGO)
    {
        _SelectedItem = shopItemGO as ItemBase;
        ShowShopItem();
    }
    

    public void RefreshItems()
    {
        if (_TagPanel.GetShowingPage() == 0)
        {
            _ShopItemContainer.RefreshItems();
        }
        else
        {
            OnPage(1);
        }
    }

    public void ShowShopItem()
    {
        if (_SelectedItem is ItemShop)
        {
            _Desc.text = _SelectedItem.GetDesc();
            _Name.text = _SelectedItem.GetNameWithColor();
        }
        else if (_SelectedItem is ItemEquip)
        {
            _Desc.text = Tables.StrDictionary.GetFormatStr(2300078);
            _Name.text = (_SelectedItem as ItemEquip).GetName();
        }
        else if (_SelectedItem is ItemFiveElementCore)
        {
            _Desc.text = _SelectedItem.GetDesc();
            _Name.text = (_SelectedItem as ItemFiveElementCore).GetName();
        }
        else
        {
            _Desc.text = "";
        }
    }

    #endregion

    #region buy item

    public void OnBtnBuy()
    {
        BuyItem();

        RefreshItems();
    }

    public void OnBtnRefreshGambling()
    {
        ShopData.Instance.RefreshGambling();
        RefreshItems();
    }

    public void BuyItem()
    {
        if (_SelectedItem is ItemShop)
        {
            ItemShop shopItem = _SelectedItem as ItemShop;
            if (!shopItem.ShopRecord.MutiBuy)
            {
                ShopData.Instance.BuyItem(shopItem);
                RefreshItems();
            }
            else
            {
                UIShopNum.ShowAsyn(shopItem, MutiBuyCallBack);
            }
        }
        else if (_SelectedItem is ItemEquip)
        {
            ShopData.Instance.BuyGambling(_SelectedItem as ItemEquip);
        }
        else if (_SelectedItem is ItemFiveElementCore)
        {
            ShopData.Instance.BuyGambling(_SelectedItem as ItemFiveElementCore);
        }

        
        
    }

    public void MutiBuyCallBack(ItemShop shopItem, int num)
    {
        //for (int i = 0; i < num; ++i)
        {
            ShopData.Instance.BuyItem(shopItem, num);
        }
        RefreshItems();
    }

    public void SellItem(ItemBase itemBase)
    {
        ShopData.Instance.SellItem(itemBase);
        RefreshItems();
    }

    #endregion
    
}

