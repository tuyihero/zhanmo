
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIShopItem : UIItemSelect
{
    public Image _Icon;
    public Image _Quality;
    public Text _Num;
    public Text _Name;
    public GameObject _SellOutFlag;
    public UICurrencyItem _BuyPrice;

    private ItemShop _ShowItem;
    private ItemEquip _ShowEquip;
    private ItemFiveElementCore _ShowCore;

    public override void Show(Hashtable hash)
    {
        base.Show();

        _ShowItem = null;
        _ShowEquip = null;
        _ShowCore = null;
        if (hash["InitObj"] is ItemShop)
        {
            var showItem = (ItemShop)hash["InitObj"];
            ShowItem(showItem);
        }
        else if (hash["InitObj"] is ItemEquip)
        {
            var showItem = (ItemEquip)hash["InitObj"];
            ShowEquip(showItem);
        }
        else if (hash["InitObj"] is ItemFiveElementCore)
        {
            var showItem = (ItemFiveElementCore)hash["InitObj"];
            ShowCore(showItem);
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        if (_ShowItem != null)
        {
            ShowItem(_ShowItem);
        }
        else if (_ShowEquip != null)
        {
            ShowEquip(_ShowEquip);
        }
        else if (_ShowCore != null)
        {
            ShowCore(_ShowCore);
        }
    }

    public void ShowItem(ItemShop showItem)
    {
        if (showItem == null)
        {
            ClearItem();
            return;
        }

        _ShowItem = showItem;
        if (!showItem.IsVolid())
        {
            _SellOutFlag.SetActive(true);
            ClearItem();
            return;
        }

        int lastNumCnt = showItem.ShopRecord.DailyLimit - showItem.BuyTimes;
        if (lastNumCnt > 0)
        {
            _Num.text = lastNumCnt.ToString();
            _SellOutFlag.SetActive(false);
        }
        else if (lastNumCnt == 0)
        {
            _Num.text = "";
            _SellOutFlag.SetActive(true);
        }
        else if (lastNumCnt < 0)
        {
            _Num.text = "";
            _SellOutFlag.SetActive(false);
        }
        _Icon.gameObject.SetActive(true);
        _Quality.gameObject.SetActive(true);
        ResourceManager.Instance.SetImage(_Icon, showItem.CommonItemRecord.Icon);
        ResourceManager.Instance.SetImage(_Quality, CommonDefine.GetQualityIcon(showItem.CommonItemRecord.Quality));
        _BuyPrice.ShowCurrency((MONEYTYPE)showItem.ShopRecord.MoneyType, showItem.BuyPrice);


        _Name.text = showItem.GetNameWithColor();
    }

    public void ShowEquip(ItemEquip showItem)
    {
        if (showItem == null)
        {
            ClearItem();
            return;
        }

        _ShowEquip = showItem;
        if (!showItem.IsVolid())
        {
            _SellOutFlag.SetActive(true);
            ClearItem();
            return;
        }

        _SellOutFlag.SetActive(false);

        _Icon.gameObject.SetActive(true);
        _Quality.gameObject.SetActive(true);
        ResourceManager.Instance.SetImage(_Icon, showItem.CommonItemRecord.Icon);
        ResourceManager.Instance.SetImage(_Quality, CommonDefine.GetQualityIcon(showItem.CommonItemRecord.Quality));

        int itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(ShopData._GAMBLING_COST_ITEM_ID);
        if (itemCnt > 0)
        {
            _BuyPrice.ShowCurrency(ShopData._GAMBLING_COST_ITEM_ID, 1);
        }
        else
        {
            _BuyPrice.ShowCurrency((MONEYTYPE.GOLD), ShopData.Instance.GetGamblingEquipCost(_ShowEquip));
        }

        _Num.text = "";
        _Name.text = showItem.GetName();
    }

    public void ShowCore(ItemFiveElementCore showItem)
    {
        if (showItem == null)
        {
            ClearItem();
            return;
        }

        _ShowCore = showItem;
        if (!showItem.IsVolid())
        {
            ClearItem();
            return;
        }

        _SellOutFlag.SetActive(false);

        _Icon.gameObject.SetActive(true);

        _BuyPrice.ShowCurrency((MONEYTYPE.GOLD), ShopData.Instance.GetGamblingCoreCost(_ShowCore));


        _Name.text = showItem.GetName();
    }

    private void ClearItem()
    {
        _Icon.gameObject.SetActive(false);
        _Quality.gameObject.SetActive(false);
        if (_Num != null)
        {
            _Num.text = "";
        }
    }
}

