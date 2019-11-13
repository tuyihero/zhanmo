using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFiveElementCorePack : UIBase
{
    public void OnEnable()
    {
        ShowPackItems();
        ShowItemByIndex(UIFiveElement.GetSelectedIdx());
    }

    #region 

    public UIContainerBase _CoreAttrContainer;
    public UIContainerSelect _CorePackContainer;

    private ItemFiveElementCore _ElementItem;

    private int _ShowingType;

    public void RefreshInfo()
    {
        ShowItemInfo(_ElementItem);
        ShowPackItems();
        UIFiveElement.RefreshPack();
    }

    public void ShowItemByIndex(int idx)
    {
        _ShowingType = idx;
        ShowPackItems();
        ShowItemInfo(FiveElementData.Instance._UsingCores[idx]);
    }

    private void ShowItemInfo(ItemFiveElementCore extraItem)
    {
        _ElementItem = extraItem;

        bool isConditionComplate = true;
        List<EleCoreConditionInfo> conditionDesc = new List<EleCoreConditionInfo>();
        if (_ElementItem != null && _ElementItem.IsVolid())
        {
            for (int i = 0; i < _ElementItem.FiveElementCoreRecord.PosCondition.Count; ++i)
            {
                var conState = _ElementItem.ConditionState(i);
                if (conState < 0)
                    break;

                if (conState == 0)
                {
                    isConditionComplate = false;
                }

                EleCoreConditionInfo conditionInfo = new EleCoreConditionInfo();
                var conDesc = _ElementItem.GetConditionDesc(i);
                conditionInfo._Desc = conDesc;
                conditionInfo._IsAct = conState > 0 && isConditionComplate;
                conditionInfo._Attr = _ElementItem.EquipExAttrs[i];
                conditionDesc.Add(conditionInfo);
            }
        }
        Hashtable hash = new Hashtable();
        _CoreAttrContainer.InitContentItem(conditionDesc, null, hash);
    }

    private void ShowPackItems()
    {
        List<ItemFiveElementCore> showCores = new List<ItemFiveElementCore>();
        foreach (var itemCore in FiveElementData.Instance._PackCores._PackItems)
        {
            if (!itemCore.IsVolid())
                continue;

            if ((int)itemCore.FiveElementCoreRecord.ElementType == _ShowingType)
            {
                showCores.Add(itemCore);
            }
        }
        _CorePackContainer.InitSelectContent(showCores, null, OnCoreItemClick);
    }

    private void OnCoreItemClick(object itemObj)
    {
        ItemFiveElementCore itemElement = itemObj as ItemFiveElementCore;
        if (itemElement == null || !itemElement.IsVolid())
            return;

        UIFiveElementCoreTooltip.ShowAsyn(itemElement, new ToolTipFunc[2] { new ToolTipFunc(10003, PutOnCore), new ToolTipFunc(10005, SellCore) });
    }

    private void PutOnCore(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemCore = itemBase as ItemFiveElementCore;
        if (itemCore != null)
        {
            int elePos = (int)itemCore.FiveElementCoreRecord.ElementType;
            FiveElementData.Instance.PutOnElementCore(itemCore);
            RefreshInfo();

            UIFiveElement.SetSelectedIdx(elePos);
        }
    }

    private void SellCore(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemCore = itemBase as ItemFiveElementCore;
        if (itemCore != null)
        {
            RefreshInfo();
        }
    }

    public void OnBtnSell()
    {
        List<Tables.ITEM_QUALITY> equipQualities = new List<Tables.ITEM_QUALITY>()
        {
            Tables.ITEM_QUALITY.WHITE,
            Tables.ITEM_QUALITY.GREEN,
            Tables.ITEM_QUALITY.BLUE,
            Tables.ITEM_QUALITY.PURPER,
            Tables.ITEM_QUALITY.ORIGIN,
        };
        UISellShopPack.ShowSellQualitySync(FiveElementData.Instance._PackCores.ToItemBases(), equipQualities, GetSellPrice, SellEquips);
    }

    public int GetSellPrice(List<ItemBase> items)
    {
        int totalMoney = 0;
        for (int i = 0; i < items.Count; ++i)
        {
            ItemFiveElementCore itemElement = items[i] as ItemFiveElementCore;
            if (itemElement != null)
            {
                totalMoney += FiveElementData.Instance.GetCoreSellMoney(itemElement);
            }
        }

        return totalMoney;
    }

    public void SellEquips(List<ItemBase> items)
    {
        FiveElementData.Instance.SellCores(items);
    }
    #endregion

}

