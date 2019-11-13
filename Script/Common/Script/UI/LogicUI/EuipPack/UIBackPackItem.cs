
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public enum RedTipType
{
    None,
    EquipPack,
    LegedaryPack,
}

public class UIBackPackItem : /*UIDragableItemBase*/ UIPackItemBase
{

    public Toggle _SellToggle;

    public ItemEquip _BackpackEquip;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (ItemBase)hash["InitObj"];
        if (hash.ContainsKey("RedTipType"))
        {
            _RedTipType = (RedTipType)hash["RedTipType"];
        }
        ShowItem(showItem);

        RefreshRedTips();
    }

    public override void ShowItem(ItemBase showItem)
    {
        base.ShowItem(showItem);
        _BackpackEquip = null;
        if (showItem == null || !showItem.IsVolid())
        {
            _Icon.gameObject.SetActive(false);
            _Quality.gameObject.SetActive(false);
            return;
        }

        if (_Num != null)
        {
            if (showItem is ItemEquip)
            {
                _BackpackEquip = showItem as ItemEquip;
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
        _Quality.gameObject.SetActive(true);
        ResourceManager.Instance.SetImage(_Icon, showItem.CommonItemRecord.Icon);
        ResourceManager.Instance.SetImage(_Quality, CommonDefine.GetQualityIcon(showItem.GetQuality()));

        if (_SellToggle != null)
        {
            _SellToggle.gameObject.SetActive(false);
        }
    }

    protected override void ClearItem()
    {
        base.ClearItem();
        if (_Num != null)
        {
            _Num.text = "";
        }
    }

    public override void OnItemClick()
    {
        base.OnItemClick();

        if (_SellToggle != null && _SellToggle.gameObject.activeSelf)
        {
            _SellToggle.isOn = !_SellToggle.isOn;
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        RefreshRedTips();
    }

    #region interaction

    public void SetSellMode(bool isSellMode, Tables.ITEM_QUALITY sellQuality = Tables.ITEM_QUALITY.WHITE)
    {
        if (_SellToggle == null)
            return;

        if (!isSellMode)
        {
            _SellToggle.gameObject.SetActive(false);
        }
        else
        {
            if (_BackpackEquip == null)
            {
                _SellToggle.gameObject.SetActive(false);
            }
            else
            {
                _SellToggle.gameObject.SetActive(true);
                if (_BackpackEquip.EquipQuality > sellQuality)
                {
                    _SellToggle.isOn = false;
                }
                else
                {
                    _SellToggle.isOn = true;
                }
            }

        }
    }

    #endregion

    #region redtip

    public GameObject _RedTips;

    private RedTipType _RedTipType = RedTipType.None;

    public void RefreshRedTips()
    {
        if (_RedTips == null)
            return;

        if (_RedTipType == RedTipType.None)
        {
            _RedTips.gameObject.SetActive(false);
            return;
        }

        if (_RedTipType == RedTipType.EquipPack)
        {
            if (_BackpackEquip == null || !_BackpackEquip.IsVolid())
            {
                _RedTips.gameObject.SetActive(false);
                return;
            }

            if (BackBagPack.Instance.IsEquipBetter(_BackpackEquip))
            {
                _RedTips.gameObject.SetActive(true);
            }
            else
            {
                _RedTips.gameObject.SetActive(false);
            }
        }
        else if (_RedTipType == RedTipType.LegedaryPack)
        {
            if (_BackpackEquip == null || !_BackpackEquip.IsVolid())
            {
                _RedTips.gameObject.SetActive(false);
                return;
            }

            if (!BackBagPack.Instance.IsEquipBetter(_BackpackEquip) && LegendaryData.Instance.IsCollectBetter(_BackpackEquip))
            {
                _RedTips.gameObject.SetActive(true);
            }
            else
            {
                _RedTips.gameObject.SetActive(false);
            }
        }
    }


    #endregion
}

