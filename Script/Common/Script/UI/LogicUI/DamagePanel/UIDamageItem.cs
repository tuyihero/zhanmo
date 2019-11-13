using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

 

 



public class UIDamageItem : UIItemBase
{
    public Text DamageValue1;
    public Text DamageValue2;
    public UIImgAnimText _AnimText;
    public RectTransform _RootTransform;
    public RectTransform _TextTransform;

    private const string _NormalColor = "<color=#ffff00dd>";
    private const string _CriticalColor = "<color=#FF5900dd>";
    private const string _HurtColor = "<color=#FF0000dd>";
    private const string _HealColor = "<color=#00FF00dd>";

    private Vector3 _InitPos = Vector3.zero;

    public void Show(Vector3 showWorldPos, int showValue1, int showValue2, RoleAttrManager.ShowDamageType showType, int baseSize)
    {
        gameObject.SetActive(true);
        switch (showType)
        {
            case RoleAttrManager.ShowDamageType.Normal:
                DamageValue1.text = _NormalColor + showValue1.ToString() + "</color>";
                break;
            case RoleAttrManager.ShowDamageType.Criticle:
                DamageValue1.text = _CriticalColor + showValue1.ToString() + "</color>";
                break;
            case RoleAttrManager.ShowDamageType.Hurt:
                DamageValue1.text = _HurtColor + showValue1.ToString() + "</color>";
                break;
            case RoleAttrManager.ShowDamageType.Heal:
                DamageValue1.text = _HealColor + showValue1.ToString() + "</color>";
                break;
        }
        _AnimText.text = showValue1.ToString();
        _AnimText.PlayAnim();

        if (showValue2 > 0)
        {
            DamageValue2.text = showValue2.ToString();
        }
        else
        {
            DamageValue2.text = "";
        }

        _InitPos = UIManager.Instance.WorldToScreenPoint(showWorldPos);
        _RootTransform.anchoredPosition = _InitPos;
        _RootTransform.localScale = Vector3.one;
        _TextTransform.localScale = new Vector3(baseSize, baseSize, baseSize);

        _PosDelta = Vector3.zero;
        _SizeDelta = Vector3.one;
        _StartAnimTime = Time.time;
    }

    #region animation

    public float _ShowTime = 0.9f;
    public float _LargeTime = 0.15f;
    public float _LargeSize = 0.2f;
    public float _SmallTime = 0.1f;
    public float _SmallSize = -0.1f;

    public Vector3 _PosDelta;
    public Vector3 _SizeDelta;

    private float _StartAnimTime;

    public void Update()
    {
        var itemPos = _InitPos + _PosDelta;
        _PosDelta += new Vector3(0, 40 * Time.deltaTime, 0);
        _RootTransform.anchoredPosition = itemPos;


        _RootTransform.localScale = _SizeDelta;
        if (Time.time - _StartAnimTime < _LargeTime)
        {
            _SizeDelta = Vector3.one + Vector3.one * _LargeSize * ((_LargeTime - (Time.time - _StartAnimTime)) / _LargeTime);
        }
        else if (Time.time - _StartAnimTime < _LargeTime + _SmallTime)
        {
            _SizeDelta = Vector3.one + Vector3.one * _SmallSize * ((_SmallTime - (Time.time - _StartAnimTime - _LargeTime)) / (_SmallTime));
        }
        else
        {
            UIDamagePanel.HideItem(this);
        }

    }

    #endregion


}

