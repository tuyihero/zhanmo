using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonLevelUpSelect : UIBase
{

    #region static funs

    public static void ShowAsyn(SummonMotionData summonMotion)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonMotion", summonMotion);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonLevelUpSelect, UILayer.Sub2PopUI, hash);
    }

    public static void OnSelectMat(SummonMotionData matMotion, int addNum)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonLevelUpSelect>(UIConfig.UISummonLevelUpSelect);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.AddExpMotion(matMotion, addNum);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonLevelUpSelect>(UIConfig.UISummonLevelUpSelect);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region pack

    public UIContainerSelect _SummonItemContainer;

    public Text _Level;
    public Text _Exp;
    public Slider _CurExpProcess;
    public Slider _SelectExpProcess;
    public List<Image> _Stars;
    public Text _StarPro;

    private SummonMotionData _LevelUpMotion;

    private Dictionary<SummonMotionData, int> _SelectedExp = new Dictionary<SummonMotionData, int>();

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SelectedExp.Clear();
        _LevelUpMotion = (SummonMotionData)hash["SummonMotion"];

        ShowItemPack();

        RefreshExp();
    }

    public void RefreshItems()
    {
        ShowItemPack();
    }

    private void ShowItemPack()
    {
        List<SummonMotionData> unusedMotions = new List<SummonMotionData>(SummonSkillData.Instance._SummonMatList._PackItems);
        SummonSkillData.Instance.SortSummonMotionsInExp(unusedMotions, _LevelUpMotion);

        Hashtable hash = new Hashtable();
        hash.Add("ShowSelect", true);
        _SummonItemContainer.InitSelectContent(unusedMotions, null, null, null, hash);
    }

    private void AddExpMotion(SummonMotionData expMotion, int addNum)
    {
        if (!_SelectedExp.ContainsKey(expMotion))
        {
            _SelectedExp.Add(expMotion, 0);
        }

        _SelectedExp[expMotion] += addNum;

        RefreshExp();
    }

    private void RefreshExp()
    {
        int exp = SummonSkillData.Instance.GetItemsExp(_SelectedExp) + _LevelUpMotion.Exp;
        int starCnt = SummonSkillData.Instance.GetItemsStarExp(_LevelUpMotion, _SelectedExp) + _LevelUpMotion.StarExp;

        int lastExp = 0;
        int lastStarExp = 0;
        int destStarLevel = SummonMotionData.GetStarLevelByExp(_LevelUpMotion, starCnt, out lastStarExp);
        int destLevel = SummonMotionData.GetLevelByExp(exp, destStarLevel, out lastExp);

        if (destLevel == destStarLevel * 10)
        {
            _Level.text = Tables.StrDictionary.GetFormatStr(1200006, destLevel);
        }
        else if (destLevel > _LevelUpMotion.Level)
        {
            _Level.text = CommonDefine.GetEnableRedStr(1) + destLevel + "</color>";
        }
        else
        {
            _Level.text = destLevel.ToString();
        }

        if (destLevel > _LevelUpMotion.Level || exp > _LevelUpMotion.Exp)
        {
            _Exp.text = CommonDefine.GetEnableRedStr(1) + lastExp + "</color>";
        }
        else
        {
            _Exp.text = lastExp.ToString();
        }

        var tabLevel = Tables.TableReader.SummonSkillAttr.GetRecord(destLevel.ToString());
        if (destLevel > _LevelUpMotion.Level)
        {
            _CurExpProcess.value = 0;
        }
        else
        {
            _CurExpProcess.value = _LevelUpMotion.Exp / tabLevel.Cost[0];
        }
        _SelectExpProcess.value = lastExp / tabLevel.Cost[0];

        for (int i = 0; i < _Stars.Count; ++i)
        {
            if (i < _LevelUpMotion.StarLevel)
            {
                _Stars[i].gameObject.SetActive(true);
                _Stars[i].color = new Color(1, 1, 1, 1);
            }
            else if (i < destStarLevel)
            {
                _Stars[i].gameObject.SetActive(true);
                _Stars[i].color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                _Stars[i].gameObject.SetActive(false);
            }
        }

        if (destStarLevel > _LevelUpMotion.StarLevel || starCnt > _LevelUpMotion.StarExp)
        {
            _StarPro.text = CommonDefine.GetEnableRedStr(1) + string.Format("({0}/{1})", lastStarExp, _LevelUpMotion.SummonRecord.StarExp[destStarLevel]) + "</color>";
        }
        else
        {
            _StarPro.text = string.Format("({0}/{1})", lastStarExp, _LevelUpMotion.SummonRecord.StarExp[destStarLevel]);
        }

    }

    #endregion

    #region 

    public void OnBtnOk()
    {
        int exp = SummonSkillData.Instance.LevelUpSummonItem(_LevelUpMotion, _SelectedExp);
        UISummonSkillToolTips.ShowAddExp(_LevelUpMotion, exp);

        Hide();
    }

    public void OnBtnAutoSelect()
    {
        //List<SummonMotionData> unusedMotions = new List<SummonMotionData>();
        //for (int i = 0; i < SummonSkillData.Instance._SummonMotionList.Count; ++i)
        //{
        //    if (SummonSkillData.Instance._SummonMotionList[i].SummonRecord.Quality == Tables.ITEM_QUALITY.WHITE)
        //    {
        //        unusedMotions.Add(SummonSkillData.Instance._SummonMotionList[i]);
        //    }
        //}
        //_SummonItemContainer.SetSelect(unusedMotions);
    }

    #endregion
}

