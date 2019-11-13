using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonSkillLottery : UIBase
{

    #region static funs

    public static void ShowGoldAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("GoldPanel", 1);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonSkillLottery, UILayer.SubPopUI, hash);
    }

    public static void ShowDiamondAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("DiamondPanel", 1);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonSkillLottery, UILayer.SubPopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillLottery>(UIConfig.UISummonSkillLottery);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public UICurrencyItem _GoldOne;
    public UICurrencyItem _GoldTen;
    public UICurrencyItem _DiamondOne;
    public UICurrencyItem _DiamondTen;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowLotteryPanel();
    }

    private void ShowLotteryPanel()
    {
        int backPackItem = BackBagPack.Instance.PageItems.GetItemCnt(SummonSkillData._GoldCostItem);
        if (backPackItem > 0)
        {
            _GoldOne.ShowOwnCurrency(SummonSkillData._GoldCostItem);
        }
        else
        {
            _GoldOne.ShowCurrency(MONEYTYPE.GOLD, GameDataValue.GetSummonCostGold(SummonSkillData.Instance.SummonLevel));
        }

        if (backPackItem > 10)
        {
            _GoldTen.ShowCostCurrency(SummonSkillData._GoldCostItem, 10, -1);
        }
        else
        {
            _GoldTen.ShowCurrency(MONEYTYPE.GOLD, GameDataValue.GetSummonCostGold(SummonSkillData.Instance.SummonLevel) * 10);
        }

        int backPackDiamondItem = BackBagPack.Instance.PageItems.GetItemCnt(SummonSkillData._DiamondCostItem);
        if (backPackDiamondItem > 0)
        {
            _DiamondOne.ShowOwnCurrency(SummonSkillData._DiamondCostItem);
        }
        else
        {
            _DiamondOne.ShowCurrency(MONEYTYPE.DIAMOND, GameDataValue.GetSummonCostDiamond(SummonSkillData.Instance.SummonLevel));
        }

        if (backPackDiamondItem > 10)
        {
            _DiamondTen.ShowCostCurrency(SummonSkillData._DiamondCostItem, 10, -1);
        }
        else
        {
            _DiamondTen.ShowCurrency(MONEYTYPE.DIAMOND, GameDataValue.GetSummonCostDiamond(SummonSkillData.Instance.SummonLevel) * 10);
        }
    }


    public void RefreshItems()
    {
        ShowLotteryPanel();
    }

    #endregion

    #region interface

    private SummonSkillData.LotteryResult _LotteryResult;

    public void OnBtnBuyOne(bool isGold)
    {
        if (_LotteryResult != null)
            return;

        if (isGold)
        {
            _LotteryResult = SummonSkillData.Instance.LotteryGold(1);
        }
        else
        {
            _LotteryResult = SummonSkillData.Instance.LotteryDiamond(1);
        }

        if (_LotteryResult != null)
        {
            PlayerSummonAnim(_LotteryResult._SummonData);
        }
    }

    public void OnBtnBuyTen(bool isGold)
    {
        if (_LotteryResult != null)
            return;

        if (isGold)
        {
            _LotteryResult = SummonSkillData.Instance.LotteryGold(10);
        }
        else
        {
            _LotteryResult = SummonSkillData.Instance.LotteryDiamond(10);
        }

        if (_LotteryResult != null)
        {
            PlayerSummonAnim(_LotteryResult._SummonData);
        }
    }

    public void PlayerSummonAnim(List<SummonMotionData> summonDatas)
    {
        UISummonGotAnim.ShowAsyn(summonDatas, AnimFinish);
        //RefreshItems();
        UISummonSkillPack.RefreshPack();
        RefreshItems();
    }

    public void AnimFinish()
    {
        if (_LotteryResult._ReturnItemNum > 0)
        {
            UISummonLotteryReturn.ShowAsyn(_LotteryResult._ReturnItem, _LotteryResult._ReturnItemNum);
        }
        _LotteryResult = null;
    }

    #endregion

}

