using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System;
using UnityEngine.Events;

public class UINumInput : UIItemBase
{

    #region param

    public InputField _InputField;

    public Button _BtnAdd;
    public Button _BtnDec;

    private int _Value;
    public int Value
    {
        get
        {
            _Value = int.Parse(_InputField.text);
            return _Value;
        }
        set
        {
            _Value = value;
            SetNumBtnState();
            _InputField.text = _Value.ToString();
        }
    }

    private int _MaxValue = -1;
    private int _MinValue = 0;

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
        _MaxValue = maxValue;
        _MinValue = minValue;
        Value = initValue;
    }

    #endregion

    #region 

    public void BtnAdd(int stepValue)
    {
        int resValue = Value + stepValue;
        if (_MaxValue > _MinValue && _MaxValue > 0 && _MinValue >= 0)
        {
            Value = Mathf.Clamp(resValue, _MinValue, _MaxValue);
        }
        else if (_MinValue >= 0)
        {
            Value = Mathf.Min(resValue, _MinValue);
        }

        _NumModifyEvent.Invoke();

        
    }

    public void BtnDec(int stepValue)
    {
        int resValue = Value - stepValue;
        if (_MaxValue > _MinValue && _MaxValue > 0 && _MinValue >= 0)
        {
            Value = Mathf.Clamp(resValue, _MinValue, _MaxValue);
        }
        else if (_MinValue >= 0)
        {
            Value = Mathf.Min(resValue, _MinValue);
        }

        _NumModifyEvent.Invoke();

        //SetNumBtnState();
    }

    public void OnInputValue(string input)
    {
        int value;
        if (int.TryParse(input, out value))
        {
            _Value = value;
        }

        _NumModifyEvent.Invoke();
    }

    private void SetNumBtnState()
    {
        if (_Value == _MaxValue)
        {
            _BtnAdd.interactable = (false);
            _BtnDec.interactable = (true);

            _BtnAdd.image.material.SetInt("IsGray", 1);
            _BtnDec.image.material.SetInt("IsGray", 0);
        }
        else if (_Value == _MinValue)
        {
            _BtnAdd.interactable = (true);
            _BtnDec.interactable = (false);

            _BtnAdd.image.material.SetInt("IsGray", 0);
            _BtnDec.image.material.SetInt("IsGray", 1);
        }
        else
        {
            _BtnAdd.interactable = (true);
            _BtnDec.interactable = (true);

            _BtnAdd.image.material.SetInt("IsGray", 0);
            _BtnDec.image.material.SetInt("IsGray", 0);
        }
    }

    #endregion
}

