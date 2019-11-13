using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIDailyMissionItem : UIItemSelect
{
    public Image _MissionIcon;
    public Text _MissionDesc;
    public Slider _MissionProcess;
    public Text _MissionProcessText;
    public UIAwardItem _Award;
    public GameObject _BtnGoto;
    public GameObject _BtnGetAward;
    public GameObject _AwardGettedGO;

    private MissionItem _MissionItem;

    public override void Show(Hashtable hash)
    {
        base.Show();

        _MissionItem = (MissionItem)hash["InitObj"];
        ShowMissionItem(_MissionItem);
    }

    public void ShowMissionItem(MissionItem showItem)
    {
        _MissionItem = showItem;
        //_MissionDesc.text = StrDictionary.GetFormatStr(showItem.MissionRecord.Desc, showItem.MissionRecord.ConditionNum);
        _MissionDesc.text = showItem.MissionRecord.ConditionScript + showItem.MissionRecord.ConditionNum;
        _MissionProcess.value = showItem._MissionCondition.GetConditionProcess();
        _MissionProcessText.text = showItem._MissionCondition.GetConditionProcessText();

        UpdateMissionState();

    }

    private void UpdateMissionState()
    {
        _BtnGetAward.SetActive(false);
        _AwardGettedGO.SetActive(false);
        _BtnGoto.SetActive(false);
        if (_MissionItem._MissionState == MissionState.Done)
        {
            _BtnGetAward.SetActive(true);
        }
        else if (_MissionItem._MissionState == MissionState.Finish)
        {
            _AwardGettedGO.SetActive(true);
        }
        else if (_MissionItem._MissionState == MissionState.Accepted)
        {
            _BtnGoto.SetActive(true);
        }
    }

    public void OnBtnGoto()
    {
        _MissionItem._MissionCondition.ConditionGoto();
    }

    public void OnBtnGetAward()
    {
        _MissionItem.MissionGetAward();
        UpdateMissionState();
    }

    


}

