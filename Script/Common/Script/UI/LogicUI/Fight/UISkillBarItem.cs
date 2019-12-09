using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;

public class UISkillBarItem : UIItemSelect
{
    public UIPressBtn _BtnPress;
    public Image _SkillIcon;
    public Image _CDImage;
    public Image _UseTipImage;
    public string _SkillInput;
    public Text _StoreTimes;
    public Image _Icon;

    public bool IsSkillAct
    {
        get; set;
    }

    //void Update()
    //{
    //    if (_UseTipImage.gameObject.activeInHierarchy)
    //    {
    //        _UseTipImage.fillAmount -= _Step * Time.deltaTime;
    //        if (_UseTipImage.fillAmount <= 0)
    //        {
    //            //InputManager.Instance.ResetReuseSkill();
    //        }
    //    }

    //    UpdateCD();
    //}

    public void InitSkill()
    {
        _CDImage.fillAmount = 0;
        _UseTipImage.fillAmount = 0;
    }

    public void SetSkillIcon(string heroIcon)
    {
        ResourceManager.Instance.SetImage(_Icon, heroIcon);
    }

    public void BtnSkill()
    {

    }

    #region use tips

    private float _Step;

    public void SetUseTips(float second)
    {
        if (second > 0)
        {
            _UseTipImage.gameObject.SetActive(true);
            _Step = 1 / second;
            _UseTipImage.fillAmount = 1;
        }
        else
        {
            _UseTipImage.gameObject.SetActive(false);
        }
    }

    #endregion

    #region cd

    private float _CDOrigin;
    private float _CDNow;
    public float CDNow
    {
        get
        {
            return _CDNow;
        }
    }

    public void SetCDPro(float cdProcess)
    {
        if (cdProcess > 0 && cdProcess < 1)
        {
            _CDImage.gameObject.SetActive(true);
            _CDImage.fillAmount = cdProcess;
        }
        else
        {
            _CDImage.gameObject.SetActive(false);
        }
    }

    public void SetStoreTimes(int times)
    {
        if (_StoreTimes == null)
            return;

        _StoreTimes.text = times.ToString();
    }

    public void SetCDTime(float cdTime)
    {
        _CDOrigin = cdTime;
        _CDNow = cdTime;
    }

    private void UpdateCD()
    {
        _CDNow -= Time.deltaTime;
        if (_CDNow <= 0)
        {
            _CDNow = 0;
            _CDOrigin = 0;
            SetCDPro(0);
        }
        else
        {
            float cdPro = _CDNow / _CDOrigin;
            SetCDPro(cdPro);
        }
    }

    #endregion

    #region double click

    public void OnDoubleClick()
    {

    }

    #endregion
}

