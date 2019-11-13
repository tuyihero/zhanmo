using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class UILegendaryPack : UIBase,IDragablePack
{

    #region 

    public UIContainerBase _LegencyContainer;
    public UIEquipPack _EquipPack;
    public GameObject _AttrPanel;
    public Text _CombatValue;
    public Text _Attr1;
    public Text _Attr2;
    public Text _Attr3;

    public void OnEnable()
    {
        _EquipPack._BackPack._OnItemSelectCallBack = ShowBackPackSelectItem;
        _EquipPack._BackPack._OnDragItemCallBack = OnDragItem;
        _EquipPack._BackPack._IsCanDropItemCallBack = IsCanDropItem;
        _AttrPanel.SetActive(false);

        ShowPackItems();
    }

    public void OnDisable()
    {
        _EquipPack._BackPack._OnItemSelectCallBack = _EquipPack.ShowBackPackSelectItem;
        _EquipPack._BackPack._OnDragItemCallBack = _EquipPack.OnDragItem;
        _EquipPack._BackPack._IsCanDropItemCallBack = _EquipPack.IsCanDropItem;
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        _LegencyContainer.InitContentItem(LegendaryData.Instance._LegendaryEquipDict.Keys, ShowLegendaryPackTooltips, exHash);
        _EquipPack._BackPack.Show(null);
        RefreshAttrs();
    }

    private void RefreshAttrs()
    {
        _CombatValue.text = StrDictionary.GetFormatStr(40006, LegendaryData.Instance.LegendaryValue);
    }

    public void RefreshEquipItems()
    {
        _LegencyContainer.RefreshItems();
        _EquipPack._BackPack.OnShowPage(UIBackPack.BackPackPage.PAGE_LEGENDARY);
    }

    public void ShowBackPackSelectItem(ItemBase itemObj)
    {
        ItemEquip equipItem = itemObj as ItemEquip;
        if (equipItem != null && equipItem.IsVolid() && LegendaryData.IsEquipLegendary(equipItem))
        {
            if (LegendaryData.IsEquipLegendary(equipItem))
            {
                UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10010, PutInEquip) });
            }
            else
            {
                UIEquipTooltips.ShowAsyn(equipItem);
            }
        }
        else if (itemObj.IsVolid())
        {
            UIItemTooltips.ShowAsyn(itemObj);
        }
    }

    private void ShowLegendaryPackTooltips(object equipObj)
    {
        EquipItemRecord equipRecord = equipObj as EquipItemRecord;
        if (equipRecord == null)
            return;

        var equipItem = LegendaryData.Instance._LegendaryEquipDict[equipRecord];
        if (equipItem == null || !equipItem.IsVolid())
        {
            UILegendaryItemTooltips.ShowAsyn(equipRecord, new ToolTipFunc[1] { new ToolTipFunc(10011, PutOffEquip) });   
        }
        else
        {
            UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10011, PutOffEquip) });
        }
    }

    public void OnBtnShowAttr()
    {
        _AttrPanel.SetActive(true);
        _Attr1.text = LegendaryData.Instance.ExAttrs[0].GetAttrStr();
        _Attr2.text = LegendaryData.Instance.ExAttrs[1].GetAttrStr();
        int impactLevel = 1;
        LegendaryData.LegendaryShadowAttrInfo nextShadowLv = null;
        if (LegendaryData.Instance.ExAttrs.Count == 3)
        {
            impactLevel = LegendaryData.Instance.ExAttrs[2].AttrParams[1];
            nextShadowLv = LegendaryData.Instance.GetNextShadowLv(impactLevel);
        }
        else
        {
            nextShadowLv = LegendaryData.Instance.GetNextShadowLv(0);
        }

        var attrRecord = TableReader.AttrValue.GetRecord(LegendaryData._SpecilImpact);
        string impactAttr = EquipExAttr.GetAttrStr(attrRecord.AttrImpact, new List<int>() { int.Parse(LegendaryData._SpecilImpact), impactLevel });
        impactAttr += "Lv." + impactLevel;
        if (nextShadowLv != null)
        {
            impactAttr += "\n" + StrDictionary.GetFormatStr(40007, LegendaryData.Instance.GetLegendaryCollectValue(), nextShadowLv._NeedValue, nextShadowLv._Level);
        }
        if (LegendaryData.Instance.ExAttrs.Count != 3)
        {
            impactAttr = CommonDefine.GetEnableGrayStr(0) + impactAttr + "</color>";
        }
        _Attr3.text = impactAttr;
    }

    public void OnHideAttr()
    {
        _AttrPanel.SetActive(false);
    }
    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshEquipItems();
    }

    public bool IsCanDragItem(UIDragableItemBase dragItem)
    {
        if (!dragItem.ShowedItem.IsVolid())
            return false;

        if (!LegendaryData.IsEquipLegendary(dragItem.ShowedItem as ItemEquip))
            return false;

        return true;
    }

    public bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == dropItem._DragPackBase)
            return false;

        if (dragItem._DragPackBase == this)
        {
            if (!dropItem.ShowedItem.IsVolid())
                return true;

            if (dropItem.ShowedItem is ItemEquip)
            {
                if (dropItem.ShowedItem.ItemDataID == dropItem.ShowedItem.ItemDataID)
                    return true;
                else
                    return false;
            }
        }
        else if (dropItem._DragPackBase == this)
        {
            if (dragItem.ShowedItem is ItemEquip)
            {
                UILegendaryEquipItem legendaryItem = dropItem.transform.parent.GetComponent<UILegendaryEquipItem>();
                if (legendaryItem == null)
                    return false;

                if (legendaryItem._LegendaryRecord.Id == dragItem.ShowedItem.ItemDataID)
                    return true;
            }
        }

        return false;
    }

    public void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == dropItem._DragPackBase)
            return;

        if (dragItem._DragPackBase == this)
        {
            PutOffEquip(dragItem.ShowedItem);
        }
        else if (dropItem._DragPackBase == this)
        {
            PutInEquip(dragItem.ShowedItem);
        }
    }

    private void PutInEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            if (LegendaryData.Instance.PutInEquip(itemEquip))
            {
                RefreshAttrs();
                RefreshEquipItems();
            }
        }
    }

    private void PutOffEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            if (LegendaryData.Instance.PutOffEquip(itemEquip))
            {
                RefreshAttrs();
                RefreshEquipItems();
            }
        }
    }

    #endregion


}

