using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIAchievementItem : UIItemSelect
{
    public Image _MissionIcon;
    public Text _MissionDesc;
    public Slider _MissionProcess;
    public Text _MissionProcessText;
    public UIAwardItem _Award;
    public GameObject _BtnGoto;
    public GameObject _BtnGetAward;
    public GameObject _AwardGettedGO;

    private AchievementGroup _AchieveGroup;

    public override void Show(Hashtable hash)
    {
        base.Show();

        _AchieveGroup = (AchievementGroup)hash["InitObj"];
        ShowAchieveItem(_AchieveGroup);
    }

    private void ShowAchieveItem(AchievementGroup showItem)
    {
        _MissionDesc.text = showItem._ActingAchieve.MissionRecord.SubClass + ": " + showItem._ActingAchieve.MissionRecord.ConditionNum;
        _MissionProcess.value = showItem.GetConditionProcess();
        _MissionProcessText.text = showItem.GetConditionProcessText();

        UpdateMissionState();

    }

    private void UpdateMissionState()
    {
        _BtnGetAward.SetActive(false);
        _AwardGettedGO.SetActive(false);
        _BtnGoto.SetActive(false);
        if (_AchieveGroup._ActingAchieve.MissionState == MissionState.Done)
        {
            _BtnGetAward.SetActive(true);
        }
        else if (_AchieveGroup._ActingAchieve.MissionState == MissionState.Finish)
        {
            _AwardGettedGO.SetActive(true);
        }
        else if (_AchieveGroup._ActingAchieve.MissionState == MissionState.Accepted)
        {
            _BtnGoto.SetActive(true);
        }
    }

    public void OnBtnGoto()
    {
        //_MissionItem._MissionCondition.ConditionGoto();
    }

    public void OnBtnGetAward()
    {
        _AchieveGroup.GetAward();
        //UpdateMissionState();
        ShowAchieveItem(_AchieveGroup);
    }

    


}

