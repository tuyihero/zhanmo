using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Tables;

public class UIGemPackRefresh : UIBase
{

    #region 

    public UIContainerSelect _EquipContainer;
    public Text _EquipTag;
    public UIEquipInfoRefresh _EuipInfo;
    public UIGemItem _GemCost;

    private ItemGem _CostGem;
    private ItemEquip _SelectedEquip;

    public void OnEnable()
    {
        List<ItemEquip> equipList = new List<ItemEquip>();
        foreach (var equipItem in PlayerDataPack.Instance._SelectedRole.EquipList)
        {
            if (equipItem.IsVolid() && equipItem.EquipQuality > ITEM_QUALITY.WHITE)
            {
                equipList.Add(equipItem);
            }
        }

        List<ItemEquip> equipInBackPack = new List<ItemEquip>();
        foreach (var equipItem in BackBagPack.Instance.PageEquips._PackItems)
        {
            if (equipItem.IsVolid() && equipItem.EquipQuality > ITEM_QUALITY.WHITE)
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

        if (equipList.Count > 0)
        {
            _EquipContainer.InitSelectContent(equipList, new List<ItemEquip>() { equipList[0]}, OnSelectedEquip);
        }
        else
        {
            _EquipContainer.InitSelectContent(equipList, null, OnSelectedEquip);
        }

        _GemCost.ShowGem(null, 0);
        _CostGem = null;
    }

    private void OnSelectedEquip(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null)
            return;

        //ShowEquipInfo(equipItem, null);

        if (PlayerDataPack.Instance._SelectedRole.EquipList.Contains(equipItem))
        {
            _EquipTag.text = StrDictionary.GetFormatStr(10013);
        }
        else
        {
            _EquipTag.text = "";
        }

        _EuipInfo.ShowTips(equipItem, null);
        _SelectedEquip = equipItem;
    }

    public void OnGemClick()
    {
        UICostGemPanel.ShowAsyn(OnSelectedGem);
    }

    private void OnSelectedGem(ItemGem itemGem)
    {
        _GemCost.ShowGem(itemGem, 0);
        _CostGem = itemGem;
    }

    public void OnRefresh()
    {
        if (_CostGem == null || _CostGem.ItemStackNum == 0)
        {
            UIMessageTip.ShowMessageTip(40004);
            return;
        }

        if (_SelectedEquip == null || !_SelectedEquip.IsVolid() || _SelectedEquip.EquipQuality == ITEM_QUALITY.WHITE)
        {
            UIMessageTip.ShowMessageTip(40005);
            return;
        }

        if (EquipRefresh.Instance.EquipRefreshMat(_SelectedEquip, _CostGem))
        {
            _EuipInfo.ShowTips(_SelectedEquip, null);
            _GemCost.ShowGem(_CostGem, 0);
        }
    }

    #endregion


}

