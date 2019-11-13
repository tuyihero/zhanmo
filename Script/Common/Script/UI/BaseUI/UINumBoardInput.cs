using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

using System;
using UnityEngine.Events;

public class UINumBoardInput : UIBase
{

    #region param
    
    public Text _InputNum;
    public GameObject _NumBoard;
    public Button _BtnAdd;
    public Button _BtnDec;
    public bool _IsShowInputBoard = true;

    private static Material _ImageGrayMaterial;
    protected int _Value;
    public virtual int Value
    {
        get
        {
            _Value = int.Parse(_InputNum.text);
            return _Value;
        }
        set
        {
            _Value = value;
            SetNumBtnState();
            _InputNum.text = _Value.ToString();
        }
    }

    protected int _MaxValue = -1;
    protected int _MinValue = 0;
    protected int _StepValue = 1;

    [Serializable]
    public class NumModifyEvent : UnityEvent
    {
        public NumModifyEvent()
        {

        }
    }

    [SerializeField]
    private NumModifyEvent _NumModifyEvent;

    #endregion

    #region 

    public void Init(int initValue, int minValue, int maxValue)
    {
        if (_ImageGrayMaterial == null)
        {
            _ImageGrayMaterial = Resources.Load<Material>("Material/ImageEffectGray");
        }

        _MaxValue = maxValue;
        _MinValue = minValue;
        Value = initValue;
    }

    public void SetStepValue(int stepValue)
    {
        _StepValue = stepValue;
    }

    #endregion

    #region add/dec

    public void BtnAdd(int stepValue)
    {
        int resValue = Value + _StepValue;
        SetValue(resValue);

        _NumModifyEvent.Invoke();

        if (resValue == _MaxValue)
        {
            UIMessageTip.ShowMessageTip(23300);
        }
    }

    public void BtnDec(int stepValue)
    {
        int resValue = Value - _StepValue;
        SetValue(resValue);

        _NumModifyEvent.Invoke();

        //SetNumBtnState();
        if (resValue == _MinValue)
        {
            UIMessageTip.ShowMessageTip(23300);
        }
    }

    protected void SetNumBtnState()
    {
        if (_Value == _MaxValue)
        {
            _BtnAdd.interactable = (false);

            _BtnAdd.image.material = _ImageGrayMaterial;
        }
        else
        {
            _BtnAdd.interactable = (true);

            _BtnAdd.image.material = null;
        }

        if (_Value == _MinValue)
        {
            _BtnDec.interactable = (false);

            _BtnDec.image.material = _ImageGrayMaterial;
        }
        else
        {
            _BtnDec.interactable = (true);

            _BtnDec.image.material = null;
        }

    }

    #endregion

    #region num board

    private int _PreNum = 0;
    private int _InputBoardNum = 0;

    public void OnBtnNumBoardOpen()
    {
        if (!_IsShowInputBoard)
            return;

        if (_NumBoard.activeSelf)
        {
            OnBtnOk();
        }

        _NumBoard.SetActive(!_NumBoard.activeSelf);
        _InputBoardNum = 0;
        _PreNum = _Value;
    }

    public void OnBtnNumInput(int num)
    {
        int value = _InputBoardNum * 10 + num;

        if (value <= 0)
            return;

        _InputBoardNum = value;
        _InputNum.text = value.ToString();
    }

    public void OnBtnNumDelete()
    {
        int value = (int)(_InputBoardNum * 0.1f);
        //if (value >= _MaxValue)
        //{
        //    return;
        //}

        _InputBoardNum = value;
        _InputNum.text = value.ToString();

    }

    public void OnBtnMax()
    {
        _InputBoardNum = _MaxValue;

        _InputNum.text = _InputBoardNum.ToString();
    }

    public void OnBtnOk()
    {
        if (_InputBoardNum >= _MaxValue)
        {
            UIMessageTip.ShowMessageTip(23300);
            _InputBoardNum = _MaxValue;
        }
        if (_InputBoardNum <= _MinValue)
        {
            UIMessageTip.ShowMessageTip(23300);
            _InputBoardNum = _MinValue;
        }

        SetValue(_InputBoardNum);

        _NumModifyEvent.Invoke();
    }

    public void OnBtnCancel()
    {
        SetValue(_Value);
    }

    #endregion

    private void SetValue(int resValue)
    {

        Value = Mathf.Clamp(resValue, _MinValue, _MaxValue);

    }

}

