using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIBackPack : UIBase, IDragablePack
{
    public enum BackPackPage
    {
        PAGE_EQUIP,
        PAGE_ITEM,
        PAGE_LEGENDARY,
    }

    public delegate void OnSelectItem(ItemBase itemBase);
    public OnSelectItem _OnItemSelectCallBack;

    public delegate void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem);
    public OnDragItem _OnDragItemCallBack;

    public delegate bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem);
    public IsCanDropItem _IsCanDropItemCallBack;

    #region 

    public UIContainerSelect _ItemsContainer;
    public GameObject _BtnPanel;

    private BackPackPage _ShowingPage = 0;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);


        OnShowPage(BackPackPage.PAGE_EQUIP);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnShowPage(BackPackPage page)
    {
        _ShowingPage = page;
        Hashtable hash = new Hashtable();
        hash.Add("DragPack", this);
        if (page == BackPackPage.PAGE_EQUIP)
        {
            hash.Add("RedTipType", RedTipType.EquipPack);
            _ItemsContainer.InitContentItem(BackBagPack.Instance.PageEquips._PackItems, ShowBackPackTooltips, hash);
        }
        else if (page == BackPackPage.PAGE_ITEM)
        {
            _ItemsContainer.InitContentItem(BackBagPack.Instance.PageItems._PackItems, ShowBackPackTooltips, hash);
        }
        else if (page == BackPackPage.PAGE_LEGENDARY)
        {
            hash.Add("RedTipType", RedTipType.LegedaryPack);
            List<ItemEquip> equipList = new List<ItemEquip>();
            foreach (var equipItem in BackBagPack.Instance.PageEquips._PackItems)
            {
                if (LegendaryData.IsEquipLegendary(equipItem))
                {
                    equipList.Add(equipItem);
                }
            }
            if (equipList.Count < BackBagPack._BAG_PAGE_SLOT_CNT)
            {
                for (int i = equipList.Count; i < BackBagPack._BAG_PAGE_SLOT_CNT; ++i)
                {
                    equipList.Add(null);
                }
            }
            _ItemsContainer.InitContentItem(equipList, ShowBackPackTooltips, hash);
        }

        _BtnPanel.SetActive(true);
    }

    public void OnShowAllEquip()
    {
        List<ItemEquip> equipList = new List<ItemEquip>();
        foreach (var equipItem in PlayerDataPack.Instance._SelectedRole.EquipList)
        {
            if (equipItem.IsVolid() && equipItem.EquipQuality > Tables.ITEM_QUALITY.BLUE)
            {
                equipList.Add(equipItem);
            }
        }

        List<ItemEquip> equipInBackPack = new List<ItemEquip>();
        foreach (var equipItem in BackBagPack.Instance.PageEquips._PackItems)
        {
            if (equipItem.IsVolid() && equipItem.EquipQuality > Tables.ITEM_QUALITY.BLUE)
            {
                equipInBackPack.Add(equipItem);
            }
        }
        equipInBackPack.Sort((equipA, equipB) =>
        {
            if (equipA.EquipQuality > equipB.EquipQuality)
                return 1;
            return -1;
        });
        equipList.AddRange(equipInBackPack);
        if (equipList.Count < BackBagPack._BAG_PAGE_SLOT_CNT)
        {
            for (int i = equipList.Count; i < BackBagPack._BAG_PAGE_SLOT_CNT; ++i)
            {
                equipList.Add(new ItemEquip());
            }
        }

        Hashtable hash = new Hashtable();
        hash.Add("DragPack", this);
        if (equipList.Count > 0)
        {
            _ItemsContainer.InitContentItem(equipList, ShowBackPackTooltips, hash);
        }
        else
        {
            _ItemsContainer.InitContentItem(equipList, ShowBackPackTooltips, hash);
        }
        _BtnPanel.SetActive(false);
    }

    public void RefreshItems()
    {
        _ItemsContainer.RefreshItems();
    }

    private void ShowBackPackTooltips(object equipObj)
    {
        if (_OnItemSelectCallBack == null)
            return;

        ItemBase equipItem = equipObj as ItemBase;
        _OnItemSelectCallBack.Invoke(equipItem);
    }

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }

    #endregion

    #region interaction

    public void OnBtnSell()
    {
        //_ItemsContainer.ForeachActiveItem<UIBackPackItem>((backPackItem) =>
        //{
        //    backPackItem.SetSellMode(true);
        //});

        //_BuyBackPanel.SetActive(true);
        //_BuyBackContainer.InitContentItem(ShopData.Instance._BuyBackList);

        List<Tables.ITEM_QUALITY> equipQualities = new List<Tables.ITEM_QUALITY>()
        {
            Tables.ITEM_QUALITY.WHITE,
            Tables.ITEM_QUALITY.BLUE,
            Tables.ITEM_QUALITY.PURPER,
            Tables.ITEM_QUALITY.ORIGIN,
        };
        UISellShopPack.ShowSellQualitySync(BackBagPack.Instance.PageEquips.ToItemBases(), equipQualities, GetSellPrice, SellEquips);
    }

    public int GetSellPrice(List<ItemBase> items)
    {
        int totalMoney = 0;
        for (int i = 0; i < items.Count; ++i)
        {
            ItemEquip itemEquip = items[i] as ItemEquip;
            if (itemEquip != null)
            {
                totalMoney += GameDataValue.GetEquipSellGold(itemEquip);
            }
        }

        return totalMoney;
    }

    public void SellEquips(List<ItemBase> items)
    {
        BackBagPack.Instance.SellEquips(items);

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfos", items);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_SELL, this, hash);
    }

    public List<ItemBase> GetSellList()
    {
        List<ItemBase> sellList = new List<ItemBase>();
        _ItemsContainer.ForeachActiveItem<UIBackPackItem>((backPackItem) =>
        {
            if (backPackItem._SellToggle.isOn)
            {
                //ShopData.Instance.SellItem(backPackItem.ShowedItem, false);
                sellList.Add(backPackItem.ShowedItem);
            }
        });

        return sellList;
    }

    public void OnBtnRefresh()
    {
        if (_ShowingPage == BackPackPage.PAGE_EQUIP)
        {
            BackBagPack.Instance.SortEquip();
        }
        else if (_ShowingPage == BackPackPage.PAGE_ITEM)
        {
            BackBagPack.Instance.SortItem();
        }
        else if (_ShowingPage == BackPackPage.PAGE_LEGENDARY)
        {
            BackBagPack.Instance.SortItem();
        }
        OnShowPage(_ShowingPage);
    }

    public void SetBackPackSellMode(Tables.ITEM_QUALITY sellQuality)
    {
        _ItemsContainer.ForeachActiveItem<UIBackPackItem>((backPackItem) =>
        {
            backPackItem.SetSellMode(true, sellQuality);
        });
    }
    #endregion

    #region drag

    public bool IsCanDragItem(UIDragableItemBase dragItem)
    {
        if (!dragItem.ShowedItem.IsVolid())
            return false;

        return true;
    }

    void IDragablePack.OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if ((dragItem._DragPackBase == this && dropItem._DragPackBase == this))
        {
            dragItem.ShowedItem.ExchangeInfo(dropItem.ShowedItem);
            dragItem.Refresh();
            dropItem.Refresh();
            return;
        }

        if (_OnDragItemCallBack != null)
        {
            _OnDragItemCallBack(dragItem, dropItem);
        }
    }

    bool IDragablePack.IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if ((dragItem._DragPackBase == this && dropItem._DragPackBase == this))
        {
            return true;
        }

        if (_IsCanDropItemCallBack != null)
        {
            return _IsCanDropItemCallBack(dragItem, dropItem);
        }
        return true;
    }

    #endregion

}

