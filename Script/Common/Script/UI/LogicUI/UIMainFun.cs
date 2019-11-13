using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMainFun : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMainFun, UILayer.BaseUI, hash);
    }

    public static void ShowAsynInFight()
    {
        Hashtable hash = new Hashtable();
        hash.Add("IsInFight", true);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMainFun, UILayer.BaseUI, hash);
    }

    public static void UpdateMoney()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.UpdateMoneyInner();
    }

    public static void RefreshGift()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIMainFun>(UIConfig.UIMainFun);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshGiftBtns();
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        UpdateMoneyInner();
        RefreshGiftBtns();

        bool isInFight = false;
        if (hash.ContainsKey("IsInFight"))
        {
            isInFight = (bool)hash["IsInFight"];
        }
        InitFightBtn(isInFight);
        RefreshBtn();
    }

    #endregion

    #region info

    public UICurrencyItem _GoldItem;
    public UICurrencyItem _DiamondItem;

    private void UpdateMoneyInner()
    {
        _GoldItem.ShowOwnCurrency(MONEYTYPE.GOLD);
        _DiamondItem.ShowOwnCurrency(MONEYTYPE.DIAMOND);
    }

    #endregion

    #region event

    //fight
    public void BtnFight()
    {
        //UIStageSelect.ShowAsyn();
        //LogicManager.Instance.EnterFight("Stage_01_01");
        ActData.Instance.StartDefaultStage();
    }

    public void BtnBagPack()
    {
        UIEquipPack.ShowAsyn();
    }

    public void BtnShop()
    {
        UIShopPack.ShowAsyn();
    }

    public void BtnGem()
    {
        UIGemPack.ShowAsyn();
    }

    public void BtnSoul()
    {
        //UISoulPack.ShowAsyn();
        UISummonSkillPack.ShowAsyn();
    }

    public void BtnTestPanel()
    {
        UITestEquip.ShowAsyn();
    }

    public void BtnSkill()
    {
        UISkillLevelUp.ShowAsyn();
    }

    public void BtnAttr()
    {
        UIRoleAttr.ShowAsyn();
    }

    public void BtnElement()
    {
        UIFiveElement.ShowAsyn();
    }

    public void BtnBossStage()
    {
        UIBossStageSelect.ShowAsyn();
    }

    public void BtnMission()
    {
        UIDailyMission.ShowAsyn();
    }

    public void BtnAchieve()
    {
        UIAchievement.ShowAsyn();
    }

    public void BtnSetting()
    {
        UISystemSetting.ShowAsyn();
    }

    public void BtnBuffT()
    {
        UIGlobalBuff.ShowTelantAsyn();
    }

    public void BtnBuffA()
    {
        UIGlobalBuff.ShowAttrAsyn();
    }

    public void BtnStage()
    {
        if (ActData.Instance._NormalStageIdx == 0)
        {
            ActData.Instance.StartDefaultStage();
            return;
        }

        UIStageSelect.ShowAsyn();
    }

    public void BtnAct()
    {
        UIActPanel.ShowAsyn();
    }
    #endregion

    #region gift 

    public GameObject _AdGift;
    public GameObject _SpGiftEffect;
    public GameObject _PurchGift;

    public void RefreshGiftBtns()
    {
        if (GiftData.Instance._GiftItems != null)
        {
            _AdGift.SetActive(true);
            if (GiftData.Instance._IsShowDefaultGift)
            {
                _SpGiftEffect.SetActive(false);
            }
            else
            {
                _SpGiftEffect.SetActive(true);
            }
            //_PurchGift.SetActive(true);
        }
        else
        {
            _AdGift.SetActive(false);
            //_PurchGift.SetActive(false);
        }
    }

    public void OnBtnAdGift()
    {
        UIGiftPack.ShowAsyn();
    }

    public void OnBtnPurchGift()
    {
        UIGiftPack.ShowAsyn();
    }

    #endregion

    #region Fight

    public GameObject _BG;
    public GameObject _LargeFightBtn;
    public GameObject _SmallFightBtn;
    public GameObject _RetryBtn;
    public GameObject _NextBtn;
    public GameObject _DisableNextBtn;

    public void InitFightBtn(bool isInFight)
    {
        _BG.SetActive(!isInFight);
        _LargeFightBtn.SetActive(!isInFight);
        _SmallFightBtn.SetActive(isInFight);

        if (ActData.Instance._StageMode == Tables.STAGE_TYPE.NORMAL)
        {
            _RetryBtn.SetActive(true);
            _NextBtn.SetActive(true);
            if (ActData.Instance._ProcessStageIdx > ActData.Instance._NormalStageIdx)
            {
                _DisableNextBtn.SetActive(true);

            }
            else
            {
                _DisableNextBtn.SetActive(false);
            }
        }
        else
        {
            _RetryBtn.SetActive(false);
            _NextBtn.SetActive(false);
        }
    }

    public void OnBtnRetry()
    {
        ActData.Instance.StartCurrentStage();
    }

    public void OnBtnDisableNext()
    {
        UIMessageTip.ShowMessageTip(2300087);
    }

    #endregion

    #region func open

    public GameObject _LargeActLock;
    public GameObject _SmallAct;
    public GameObject _BtnGem;
    public GameObject _BtnSoul;

    public void RefreshBtn()
    {
        if (GemData.Instance.PackGemDatas._PackItems.Count > 0 || GemData.Instance.PackExtraGemDatas._PackItems.Count > 0)
        {
            _BtnGem.SetActive(true);
        }
        else
        {
            _BtnGem.SetActive(false);
        }

        if (RoleData.SelectRole.TotalLevel >= GameDataValue._SOUL_START_LEVEL)
        {
            _BtnSoul.SetActive(true);
        }
        else
        {
            _BtnSoul.SetActive(false);
        }

        if (RoleData.SelectRole.TotalLevel >= GameDataValue.ACT_GOLD_START)
        {
            _SmallAct.SetActive(true);
            _LargeActLock.SetActive(false);
        }
        else
        {
            _LargeActLock.SetActive(true);
            _SmallAct.SetActive(false);
        }
    }

    #endregion
}

