using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Tables;

public class UIEquipPackRefresh : UIBase
{

    #region 

    public UIEquipInfoRefresh _EuipInfo;
    public UICurrencyItem _CostGold;
    public UICurrencyItem _CostDiamond;

    private ItemEquip _SelectedEquip;

    public void OnSelectedEquip(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null)
            return;

        _EuipInfo.ShowTips(equipItem, null);
        _SelectedEquip = equipItem;

        RefreshCost();
    }

    public void OnRefreshGold()
    {
        if (_SelectedEquip == null || !_SelectedEquip.IsVolid() || _SelectedEquip.EquipQuality == ITEM_QUALITY.WHITE)
        {
            UIMessageTip.ShowMessageTip(40005);
            return;
        }

        if (EquipRefresh.Instance.EquipRefreshGold(_SelectedEquip))
        {
            _EuipInfo.ShowTips(_SelectedEquip, null);
            RefreshCost();
        }
    }

    public void OnRefreshDiamond()
    {
        if (_SelectedEquip == null || !_SelectedEquip.IsVolid() || _SelectedEquip.EquipQuality == ITEM_QUALITY.WHITE)
        {
            UIMessageTip.ShowMessageTip(40005);
            return;
        }

        if (EquipRefresh.Instance.EquipRefreshDiamond(_SelectedEquip))
        {
            _EuipInfo.ShowTips(_SelectedEquip, null);
            RefreshCost();
        }
    }

    public void RefreshCost()
    {
        int costGold = GameDataValue.GetEquipRefreshGold(_SelectedEquip);

        _CostGold.ShowCostCurrency(MONEYTYPE.GOLD, costGold);
    }

    #endregion


}

