using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIGemPackPunch : UIBase, IDragablePack
{

    #region 

    public UIGemItem[] _GemPack;
    public Text _ActingSuit;

    private int _SelectGemSlot = -1;

    #endregion

    #region 

    public void OnEnable()
    {
        ShowPackItems();

        //UIGemPack.RefreshPack();
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        for (int i = 0; i < _GemPack.Length; ++i)
        {
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", GemData.Instance.EquipedGemDatas[i]);
            hash.Add("DragPack", this);
            hash.Add("RefreshType", 0);
            _GemPack[i].Show(hash);
            _GemPack[i]._InitInfo = GemData.Instance.EquipedGemDatas[i];
            _GemPack[i]._ClickEvent += ShowGemTooltipsLeft;
            _GemPack[i].RefreshGemEquip(i);
        }
        //_BackPack.Show(null);

        if (GemSuit.Instance.ActSet != null)
        {
            _ActingSuit.text = Tables.StrDictionary.GetFormatStr(GemSuit.Instance.ActSet.Name);
        }
        else
        {
            _ActingSuit.text = "";
        }
    }

    public void RefreshItems()
    {
        UIGemPack.RefreshPack();

        for (int i = 0; i < _GemPack.Length; ++i)
        {
            _GemPack[i].ShowGem(GemData.Instance.EquipedGemDatas[i], 0);
            _GemPack[i].RefreshGemEquip(i);
        }
        //_EquipContainer.RefreshItems();
        //_BackPack.RefreshItems();

        if (GemSuit.Instance.ActSet != null)
        {
            _ActingSuit.text = Tables.StrDictionary.GetFormatStr(GemSuit.Instance.ActSet.Name);
        }
        else
        {
            _ActingSuit.text = "";
        }
    }

    private void ShowGemTooltipsLeft(object equipObj)
    {
        //for (int i = 0; i < _GemPack.Length; ++i)
        //{
        //    if (_GemPack[i]._InitInfo == equipObj)
        //    {
        //        _GemPack[i].Selected();
        //        _SelectGemSlot = i;
        //    }
        //    else
        //    {
        //        _GemPack[i].UnSelected();
        //    }
        //}
        ItemGem gemItem = equipObj as ItemGem;
        if (gemItem == null || !gemItem.IsVolid())
            return;

        UIGemTooltips.ShowAsynInType(gemItem, TooltipType.GemSuitAttr, new ToolTipFunc[1] { new ToolTipFunc(10008, PunchOff)});
    }

    public void ShowGemTooltipsRight(ItemGem gemItem)
    {
        //if (gemItem.ItemStackNum == 0)
        //{
        //    UIGemTooltips.ShowAsynInType(gemItem, TooltipType.Single, new ToolTipFunc[1] { new ToolTipFunc(10012, LevelUp) });
        //}
        //else
        //{
        //    UIGemTooltips.ShowAsynInType(gemItem, TooltipType.Single, new ToolTipFunc[2] { new ToolTipFunc(10007, PunchOn), new ToolTipFunc(10009, LevelUp) });
        //}
        UIGemTooltips.ShowAsynInType(gemItem, TooltipType.Single, new ToolTipFunc[1] { new ToolTipFunc(10007, PunchOn) });
    }
    
    #endregion

    #region 

    private void ExchangeGems(ItemGem itemGem1, ItemGem itemGem2)
    {
        GemData.Instance.ExchangeGem(itemGem1, itemGem2);
        RefreshItems();
    }

    private void PunchOn(ItemGem itemGem, int idx)
    {
        GemData.Instance.PutOnGem(itemGem, idx);
        RefreshItems();
    }

    private void PunchOn(ItemBase itemBase)
    {
        ItemGem itemGem = itemBase as ItemGem;
        if (!itemGem.IsVolid())
            return;

        //if (_SelectGemSlot >= 0)
        //{
        //    PunchOn(itemGem, _SelectGemSlot);
        //}
        //else
        {
            var idx = GemData.Instance.GetPutOnIdx();
            PunchOn(itemGem, idx);
        }

        UIGemTooltips.HideAsyn();
    }

    private void PunchOff(ItemBase itemBase)
    {
        ItemGem itemGem = itemBase as ItemGem;
        GemData.Instance.PutOff(itemGem);
        RefreshItems();

        UIGemTooltips.HideAsyn();
    }

    private void LevelUp(ItemBase itemBase)
    {
        //ItemGem itemGem = itemBase as ItemGem;
        //if (GemData.Instance.GemLevelUp(itemGem))
        //{
        //    ShowGemTooltipsRight(itemGem);
        //    RefreshItems();
        //}
    }

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }

    public bool IsCanDragItem(UIDragableItemBase dragItem)
    {
        if (!dragItem.ShowedItem.IsVolid())
            return false;

        var gemRecord = Tables.TableReader.GemTable.GetRecord(dragItem.ShowedItem.ItemDataID);
        if (gemRecord == null)
            return false;

        return true;
    }

    public bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {

        /*if (!GemData.Instance._EquipedGems.Contains(dragItem.ShowedItem as ItemGem)
            && !GemData.Instance._EquipedGems.Contains(dropItem.ShowedItem as ItemGem))
            return false;

        if (dragItem.ShowedItem.ItemStackNum < 1)
            return false;
            */
        return true;
    }

    public void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        /*if (GemData.Instance._EquipedGems.Contains(dragItem.ShowedItem as ItemGem)
            && GemData.Instance._EquipedGems.Contains(dropItem.ShowedItem as ItemGem))
        {
            ExchangeGems(dragItem.ShowedItem as ItemGem, dropItem.ShowedItem as ItemGem);
        }
        else if (GemData.Instance._EquipedGems.Contains(dragItem.ShowedItem as ItemGem))
        {
            PunchOff(dragItem.ShowedItem as ItemGem);
        }
        else if (GemData.Instance._EquipedGems.Contains(dropItem.ShowedItem as ItemGem))
        {
            int idx = GemData.Instance._EquipedGems.IndexOf(dropItem.ShowedItem as ItemGem);
            PunchOn(dragItem.ShowedItem as ItemGem, idx);
        }*/
    }
    #endregion

    #region 

    public void OnBtnGemSuit()
    {
        UIGemSuitPack.ShowAsyn();
    }

    #endregion

}

