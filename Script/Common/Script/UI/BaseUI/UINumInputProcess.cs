using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System;
using UnityEngine.Events;

public class UINumInputProcess : UIItemBase
{

    #region param

    public InputField _InputField;
    public Slider _NumProcess;

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

    public void OnSlide()
    {
        int num = (int)(_NumProcess.value * (_MaxValue - _MinValue) + _MinValue);
        _InputField.text = num.ToString();

        if (_NumModifyEvent != null)
        {
            _NumModifyEvent.Invoke();
        }
    }

    public void OnTextInput()
    {
        var process = (float)(Value - _MinValue) / (_MaxValue - _MinValue);
        _NumProcess.value = process;

        if (_NumModifyEvent != null)
        {
            _NumModifyEvent.Invoke();
        }
    }

    #endregion
    
}

