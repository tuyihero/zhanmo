using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonSkillPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonSkillPack, UILayer.PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillPack>(UIConfig.UISummonSkillPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region pack

    public UITagPanel _PackPage;
    public UIContainerBase _SummonItemContainer;

    private int _ShowingPage = 0;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowItemPack(_ShowingPage);
        ShowUsingItems();

        _ArrayMode = false;
        RefreshAttr();
        _BtnAbsort.SetActive(false);
    }

    public void OnShowPage(int page)
    {
        _ShowingPage = page;
        RefreshItems();

        if (page == 2)
        {
            _BtnAbsort.SetActive(true);
        }
        else
        {
            _BtnAbsort.SetActive(false);
        }
    }

    public void RefreshItems()
    {
        ShowItemPack(_ShowingPage);
        ShowUsingItems();
    }

    private void ShowItemPack(int pageIdx)
    {
        if (pageIdx == 2)
        {
            Hashtable hash = new Hashtable();
            hash.Add("IsMaterial", true);
            _SummonItemContainer.InitContentItem(SummonSkillData.Instance._SummonMatList._PackItems, null, hash, OnPackClick);
        }
        else
        {
            List<SummonMotionData> unusedMotions = new List<SummonMotionData>();
            for (int i = 0; i < SummonSkillData.Instance._SummonMotionList._PackItems.Count; ++i)
            {
                if (SummonSkillData.Instance._SummonMotionList._PackItems[i].SummonRecord.ActSkillIdx == pageIdx)
                {
                    unusedMotions.Add(SummonSkillData.Instance._SummonMotionList._PackItems[i]);
                }
            }

            SummonSkillData.Instance.SortSummonMotionsInPack(unusedMotions);

            _SummonItemContainer.InitContentItem(unusedMotions, null, null, OnPackClick);
        }
    }

    private void OnPackClick(UIItemBase itemObj)
    {
        UISummonSkillItem summonItem = itemObj as UISummonSkillItem;

        if (_ArrayMode)
        {
            var sameSummonData = SummonSkillData.Instance._UsingSummon.Find((summonData) =>
            {
                if (summonData!= null && summonData.SummonRecordID == summonItem.SummonMotionData.SummonRecordID)
                {
                    return true;
                }
                return false;
            });

            if (sameSummonData != null)
            {
                UIMessageTip.ShowMessageTip(20007);
                return;
            }

            if (summonItem.SummonMotionData.SummonRecord.Quality == Tables.ITEM_QUALITY.WHITE)
                return;

            SetSelectItem(summonItem.SummonMotionData);

            SelectNextEmpty();
        }
        else
        {
            if (_ShowingPage == 2)
            {
                UISummonSkillToolTips.ShowAsyn(summonItem.SummonMotionData, true);
            }
            else
            {
                UISummonSkillToolTips.ShowAsyn(summonItem.SummonMotionData, false);
            }
            
        }
    }

    #endregion

    #region using

    public List<UISummonSkillItem> _UsingItems;

    private UISummonSkillItem _SelectingItem;

    private void ShowUsingItems()
    {
        for (int i = 0; i < SummonSkillData.Instance._UsingSummon.Count; ++i)
        {
            _UsingItems[i].ShowSummonData(SummonSkillData.Instance._UsingSummon[i]);
            _UsingItems[i]._PanelClickEvent = OnArrayClick;
        }

        //if (!_ArrayMode)
        //{
        //    ClearSelect();
        //}
    }

    private void OnArrayClick(UIItemBase itemObj)
    {
        UISummonSkillItem summonItem = itemObj as UISummonSkillItem;
        if (summonItem == null)
            return;

        if (_ArrayMode)
        {
            if (_SelectingItem == summonItem)
            {
                //_SelectingItem.ShowSummonData(null);
                SetSelectItem(null);
            }
            SetSelectedArray(summonItem);
        }
        else
        {
            if (summonItem.SummonMotionData == null)
                return;

            UISummonSkillToolTips.ShowAsyn(summonItem.SummonMotionData, false);
        }
    }

    private void SetSelectedArray(UISummonSkillItem summonItem)
    {
        for (int i = 0; i < _UsingItems.Count; ++i)
        {
            if (_UsingItems[i] == summonItem)
            {
                _UsingItems[i].SetArraySelected(true);
            }
            else
            {
                _UsingItems[i].SetArraySelected(false);
            }
        }

        _SelectingItem = summonItem;
    }

    private void SetSelectItem(SummonMotionData summonData)
    {
        if (_SelectingItem == null)
            return;

        var idx = _UsingItems.IndexOf(_SelectingItem);
        SummonSkillData.Instance.SetUsingSummon(idx, summonData);
        RefreshItems();
    }

    private void SelectNextEmpty()
    {
        for (int i = 0; i < _UsingItems.Count; ++i)
        {
            if (_UsingItems[i].SummonMotionData == null)
            {
                SetSelectedArray(_UsingItems[i]);
                return;
            }
        }
    }

    private void ClearSelect()
    {
        for (int i = 0; i < _UsingItems.Count; ++i)
        {
            _UsingItems[i].SetArraySelected(false);
        }

        _SelectingItem = null;
    }

    #endregion

    #region interface

    public GameObject _BtnAbsort;


    private bool _ArrayMode = false;

    public void OnBtnLotteryGold()
    {
        UISummonSkillLottery.ShowGoldAsyn();
    }

    public void OnBtnLotteryDiamond()
    {
        UISummonSkillLottery.ShowDiamondAsyn();
    }

    public void OnBtnCollect()
    {
        UISummonCollections.ShowAsyn();
    }

    public void OnBtnSellAll()
    {

    }

    public void OnBtnAbsort()
    {
        Dictionary<SummonMotionData, int> absortMotions = new Dictionary<SummonMotionData, int>();
        for (int i = 0; i < SummonSkillData.Instance._SummonMatList._PackItems.Count; ++i)
        {
            if (!SummonSkillData.Instance.CanBeStage(SummonSkillData.Instance._SummonMatList._PackItems[i]))
            {
                absortMotions.Add(SummonSkillData.Instance._SummonMatList._PackItems[i], SummonSkillData.Instance._SummonMatList._PackItems[i].ItemStackNum);
            }
        }

        int exp = SummonSkillData.Instance.LevelUpSummonData(absortMotions);

        RefreshItems();
        RefreshAttr();
    }
    #endregion

    #region attr

    public Text _Level;
    public Text _Exp;
    public Slider _ExpSlider;

    public void RefreshAttr()
    {
        var lvUpExp = GameDataValue.GetSummonLevelExp(SummonSkillData.Instance.SummonLevel);
        float process = (float)SummonSkillData.Instance.SummonRemainExp / lvUpExp;
        _ExpSlider.value = process;

        _Exp.text = SummonSkillData.Instance.SummonRemainExp.ToString() + "/" + lvUpExp;
        _Level.text = Tables.StrDictionary.GetFormatStr(1350003, SummonSkillData.Instance.SummonLevel.ToString());
    }

    #endregion
}

