using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UISummonSkillItemExp : UISummonSkillItem
{
    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _CurNum = SummonMotionData.ItemStackNum;

        for (int i = 0; i < _Stars.Length; ++i)
        {
            _Stars[i].gameObject.SetActive(false);
        }

        RefreshBtns();
        RefreshNumText();
    }

    public override void Refresh()
    {
        base.Refresh();

    }

    public override void OnItemClick()
    {
        base.OnItemClick();

        OnBtnAdd();
    }

    #region

    public GameObject _SelectPanel;
    public Button _BtnAdd;
    public Button _BtnDec;

    private int _CurNum;

    public void OnBtnAdd()
    {
        --_CurNum;
        RefreshBtns();
        RefreshNumText();
        UISummonLevelUpSelect.OnSelectMat(SummonMotionData, 1);
    }

    public void OnBtnDec()
    {
        ++_CurNum;
        RefreshBtns();
        RefreshNumText();
        UISummonLevelUpSelect.OnSelectMat(SummonMotionData, -1);
    }

    public void RefreshBtns()
    {
        if (_CurNum == 0)
        {
            _SelectPanel.gameObject.SetActive(true);
            _BtnAdd.gameObject.SetActive(false);
        }
        else if (_CurNum == SummonMotionData.ItemStackNum)
        {
            _SelectPanel.gameObject.SetActive(false);
        }
        else
        {
            _SelectPanel.gameObject.SetActive(true);
            _BtnAdd.gameObject.SetActive(true);
        }
    }

    public void RefreshNumText()
    {
        if (_CurNum == SummonMotionData.ItemStackNum)
        {
            _Level.text = _CurNum.ToString();
        }
        else
        {
            _Level.text = CommonDefine.GetEnableRedStr(0) + _CurNum.ToString() + "</color>";
        }
    }
    #endregion
}

