using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonSkillToolTips : UIBase
{

    #region static funs

    public static void ShowAsyn(SummonMotionData summonDatas, bool isLvUp)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonData", summonDatas);
        hash.Add("IsLVUp", isLvUp);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonSkillToolTips, UILayer.SubPopUI, hash);
    }

    public static void ShowAddExp(SummonMotionData summonData, int exp)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillToolTips>(UIConfig.UISummonSkillToolTips);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.UpdateExp(summonData, exp);
    }

    public static void Refresh()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillToolTips>(UIConfig.UISummonSkillToolTips);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowItem();
    }

    #endregion

    #region info

    public UISummonSkillItem _SummonItem;
    public Text _SkillDesc;
    public UIContainerBase _AttrContainer;

    public Text _Exp;
    public Slider _ExpProcess;

    public Text _StarExp;

    public GameObject _BtnAct;
    public GameObject _BtnDisAct;
    public GameObject _BtnStage;

    private SummonMotionData _SummonData;
    private bool _IsLvUp;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SummonData = (SummonMotionData)hash["SummonData"];
        _IsLvUp = (bool)hash["IsLVUp"];
        ShowItem();
    }

    private void ShowItem()
    {
        _Exp.text = _SummonData.CurLvExp.ToString();

        var tabLevel = Tables.TableReader.SummonSkillAttr.GetRecord(_SummonData.Level.ToString());
        float process = (float)_SummonData.CurLvExp / tabLevel.Cost[0];
        _ExpProcess.value = process;

        _SummonItem.ShowSummonData(_SummonData);

        _StarExp.text = string.Format("({0}/{1})", _SummonData.CurStarExp, _SummonData.CurStarLevelExp());

        _SkillDesc.text = Tables.StrDictionary.GetFormatStr(_SummonData.SummonRecord.SkillDesc, _SummonData.SummonRecord.SkillRate[_SummonData.StarLevel] * 100);

        if (SummonSkillData.Instance.IsSummonAct(_SummonData))
        {
            _BtnAct.SetActive(false);
            _BtnDisAct.SetActive(true);
        }
        else
        {
            _BtnAct.SetActive(true);
            _BtnDisAct.SetActive(false);
        }

        if (!_IsLvUp)
        {
            _AttrContainer.gameObject.SetActive(true);
            Hashtable hash = new Hashtable();
            hash.Add("DisplayMode", UIEquipAttrItem.AttrItemDisplayMode.ZeroDisable);
            _AttrContainer.InitContentItem(_SummonData.SummonAttrs, null, hash);
        }
        else
        {
            _AttrContainer.gameObject.SetActive(false);
            _BtnAct.SetActive(false);
            _BtnDisAct.SetActive(false);
        }
    }

    public void UpdateExp(SummonMotionData summonMotion, int exp)
    {
        if (_SummonData != summonMotion)
            return;

        ShowItem();
    }

    #endregion

    #region 

    public void BtnSell()
    {
        SummonSkillData.Instance.SellSummonItem(_SummonData);
    }

    public void BtnStage()
    {
        //UISummonStageUp.ShowAsyn(_SummonData);
        if (!_IsLvUp)
        {
            SummonSkillData.Instance.StarUpSummonItem(_SummonData);
            ShowItem();
        }
        else
        {
            var targetSummonData = SummonSkillData.Instance._SummonMotionList.GetItem(_SummonData.ItemDataID);
            if (targetSummonData != null)
            {
                SummonSkillData.Instance.StarUpSummonItem(targetSummonData);
            }
            Hide();
        }

        UISummonSkillPack.RefreshPack();
    }

    public void BtnLevelUp()
    {
        UISummonLevelUpSelect.ShowAsyn(_SummonData);
    }

    public void BtnAct()
    {
        if (SummonSkillData.Instance.IsSummonAct(_SummonData))
        {
            UIMessageTip.ShowMessageTip(1260000);
            return;
        }
        else
        {
            var emptyPos = SummonSkillData.Instance.GetEmptyPos();
            SummonSkillData.Instance.SetUsingSummon(emptyPos, _SummonData);
            UISummonSkillPack.RefreshPack();

            Hide();
        }
    }

    public void BtnDisAct()
    {
        int idx = SummonSkillData.Instance._UsingSummon.IndexOf(_SummonData);
        SummonSkillData.Instance.SetUsingSummon(idx, null);
        UISummonSkillPack.RefreshPack();

        Hide();
    }

    #endregion


}

