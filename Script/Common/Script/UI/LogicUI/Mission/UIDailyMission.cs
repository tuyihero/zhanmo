
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIDailyMission : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIDailyMission, UILayer.PopUI, hash);
    }

    #endregion

    public override void Init()
    {
        base.Init();

        InitMissionType();
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _MissionType.ShowDefaultFirst();
    }

    #region 

    public UIContainerBase _MissionContainer;
    public UISubScollMenu _MissionType;
    public UIDailyMissionItem _TotalMissionItem;

    private void InitMissionType()
    {
        _MissionType.PushMenu(StrDictionary.GetFormatStr(50000));
        _MissionType.PushMenu(StrDictionary.GetFormatStr(50001));
    }

    private void InitContainer()
    {
        _MissionContainer.InitContentItem(MissionData.Instance._MissionItems);
    }

    public void ShowMissionType(object missionTypeObj)
    {
        string missionType = missionTypeObj.ToString();
        if (missionType == StrDictionary.GetFormatStr(50000))
        {
            foreach (var missionItem in MissionData.Instance._MissionItems)
            {
                missionItem.RefreshMissionState();
            }
            _MissionContainer.InitContentItem(MissionData.Instance._MissionItems);
            MissionItem totalMission = new MissionItem("998");
            totalMission.InitMissionItem();
            totalMission.RefreshMissionState();
            _TotalMissionItem.ShowMissionItem(totalMission);
        }
        else if (missionType == StrDictionary.GetFormatStr(50001))
        {
            foreach (var missionItem in MissionData.Instance._ChallengeItems)
            {
                missionItem.RefreshMissionState();
            }
            _MissionContainer.InitContentItem(MissionData.Instance._ChallengeItems);
            MissionItem totalMission = new MissionItem("999");
            totalMission.InitMissionItem();
            totalMission.RefreshMissionState();
            _TotalMissionItem.ShowMissionItem(totalMission);
        }
    }

    #endregion

}

