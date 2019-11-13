using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

using System;
using UnityEngine.Events;

public class UINumBoardPage : UINumBoardInput
{

    #region param

    public override int Value
    {
        get
        {
            return _Value;
        }
        set
        {
            _Value = value;
            SetNumBtnState();
            _InputNum.text = _Value.ToString() + "/" + _MaxValue.ToString();
        }
    }

    public void SetMaxPage(int maxPage)
    {
        _MaxValue = maxPage;
        SetNumBtnState();
        _InputNum.text = _Value.ToString() + "/" + _MaxValue.ToString();
    }
    #endregion

}

