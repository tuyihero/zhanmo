using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;

public class UIFightSkillInfo : UIItemBase
{
    #region 

    public void Update()
    {
        UpdateCD();
        UpdateReuse();
    }

    #endregion

    public Image _SkillIcon;
    public Image _CDImage;
    public Image _UseTipImage;
    public Text _StoreTimes;
    public Text _SkillInput;

    private FightSkillInfo _FightSkillInfo;

    public void InitSkillInfo(FightSkillInfo fightSkillInfo)
    {
        _FightSkillInfo = fightSkillInfo;
        if (fightSkillInfo._StoreCnt > 0)
        {
            _StoreTimes.text = fightSkillInfo._StoreCnt.ToString();
        }
        else
        {
            _StoreTimes.text = "";
        }
        _SkillInput.text = "";
        ResourceManager.Instance.SetImage(_SkillIcon, fightSkillInfo._SkillIcon);
    }

    public void UpdateCD()
    {
        if (_FightSkillInfo._LastActCD == 0)
            return;

        float cdRate = (_FightSkillInfo._CDTime -( Time.time - _FightSkillInfo._LastActCD)) / _FightSkillInfo._CDTime;
        _CDImage.fillAmount = cdRate;

        if (cdRate <= 0)
        {
            FightSkillManager.Instance.UpdateSkillInfo();
        }
    }

    public void UpdateReuse()
    {
        if (_FightSkillInfo._LastReuse == 0)
            return;

        float reuseRate = (_FightSkillInfo._ReuseTime - (Time.time - _FightSkillInfo._LastReuse)) / _FightSkillInfo._ReuseTime;
        _CDImage.fillAmount = reuseRate;

        if (reuseRate <= 0)
        {
            FightSkillManager.Instance.UpdateSkillInfo();
        }
    }

}

