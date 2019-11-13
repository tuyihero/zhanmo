using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIGemPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGemPack, UILayer.PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.Refresh();
    }

    public static void RefreshPunchPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance._PunchPanel.RefreshItems();
    }

    public static UIContainerBase GetGemPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return null;

        if (!instance.isActiveAndEnabled)
            return null;

        return instance._GemPack;
    }

    public static void SetGemCombine(Tables.GemTableRecord resultRecord)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance._CombinePanel.AutoFitCombine(resultRecord);
    }

    public static UIGemPackPunch GetUIGemPunch()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return null;

        if (!instance.isActiveAndEnabled)
            return null;

        return instance._PunchPanel;
    }

    public static UIGemPackCombine GetUIGemCombine()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>(UIConfig.UIGemPack);
        if (instance == null)
            return null;

        if (!instance.isActiveAndEnabled)
            return null;

        return instance._CombinePanel;
    }

    #endregion

    public void OnTagSelect(int page)
    {
        _GemPackPanel.SetActive(true);
        if (page == 2)
        {
            _GemPackPanel.SetActive(false);
        }
        else if (page == 0)
        {
            RefreshForPunch();
        }
        else if (page == 1)
        {
            RefreshForCombine();
        }
    }

    #region 

    public UIContainerSelect _GemPack;

    public UITagPanel _TagPanel;
    public UIGemPackPunch _PunchPanel;
    public UIGemPackCombine _CombinePanel;
    public GameObject _GemPackPanel;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        //_GemPack.InitContentItem(GemData.Instance.PackGemDatas._PackItems, OnPackItemClick, null, OnPackPanelItemClick);
        _TagPanel.ShowPage(0);
        RefreshForPunch();

        RefreshTips();
    }

    public void Refresh()
    {
        if (_PunchPanel.isActiveAndEnabled)
        {
            RefreshForPunch();
        }
        else if (_CombinePanel.isActiveAndEnabled)
        {
            RefreshForCombine();
        }
        RefreshTips();
    }

    public void RefreshForPunch()
    {
        //_GemPack.RefreshItems();
        Hashtable hash = new Hashtable();
        hash.Add("RefreshType", UIGemItem.GemRefreshType.PUNCH);
        List<ItemGem> combineItems = new List<ItemGem>();
        foreach (var gemItem in GemData.Instance.PackExtraGemDatas._PackItems)
        {
            if (gemItem != null && gemItem.IsVolid())
            {
                combineItems.Add(gemItem);
            }
        }
        foreach (var gemItem in GemData.Instance.PackGemDatas._PackItems)
        {
            if (gemItem != null && gemItem.IsVolid())
            {
                combineItems.Add(gemItem);
            }
        }
        
        _GemPack.InitContentItem(combineItems, OnPackItemClick, hash, OnPackPanelItemClick);
    }

    public void RefreshForCombine()
    {
        //_GemPack.RefreshItems();
        Hashtable hash = new Hashtable();
        hash.Add("RefreshType", UIGemItem.GemRefreshType.COMBINE);
        List<ItemGem> combineItems = new List<ItemGem>();
        foreach (var gemItem in GemData.Instance.PackExtraGemDatas._PackItems)
        {
            if (gemItem != null && gemItem.IsVolid())
            {
                combineItems.Add(gemItem);
            }
        }
        foreach (var gemItem in GemData.Instance.PackGemDatas._PackItems)
        {
            if (gemItem != null && gemItem.IsVolid())
            {
                combineItems.Add(gemItem);
            }
        }
        _GemPack.InitContentItem(combineItems, OnPackItemClick, hash, OnPackPanelItemClick);
    }

    private void OnPackItemClick(object gemItemObj)
    {
        ItemGem gemItem = gemItemObj as ItemGem;
        if (gemItem == null)
            return;

        int showingPage = _TagPanel.GetShowingPage();
        if (showingPage == 0)
        {
            _PunchPanel.ShowGemTooltipsRight(gemItem);
        }
    }

    private void OnPackPanelItemClick(UIItemBase uiItemBase)
    {
        UIGemItem gemItem = uiItemBase as UIGemItem;
        if (gemItem == null)
            return;

        int showingPage = _TagPanel.GetShowingPage();
        if (showingPage == 1)
        {
            _CombinePanel.ShowGemTooltipsRight(gemItem);
        }
    }

    #endregion

    #region 

    public void OnBtnSort()
    {
        GemData.Instance.PacketSort();
        Refresh();
    }

    public void OnBtnLvUpAll()
    {
        GemData.Instance.AutoLevelAll();
    }

    #endregion

    #region redtip

    public GameObject _EquipRedTips;
    public GameObject _LvupRedTips;

    public void RefreshTips()
    {
        _EquipRedTips.SetActive(GemData.Instance.IsAnyGemGanEquip());
        _LvupRedTips.SetActive(GemData.Instance.IsAnyGemGanLvUp());
    }

    #endregion

}

