using UnityEngine;
using System.Collections;
using Tables;
using System;

public class AchievementItem
{
    public AchievementItem()
    {

    }

    public AchievementItem(string missionDataID)
    {
        _MissionDataID = missionDataID;
    }

    public AchievementItem(MissionRecord missionData)
    {
        _MissionDataID = missionData.Id;
    }

    private string _MissionDataID;

    private MissionRecord _MissionRecord;
    public MissionRecord MissionRecord
    {
        get
        {
            if (_MissionRecord == null)
            {
                _MissionRecord = TableReader.Mission.GetRecord(_MissionDataID);
            }
            return _MissionRecord;
        }
    }

    public MissionState MissionState = MissionState.Accepted;

    public bool OnConditionEvent()
    {
        var conditionNum = (AchievementData.AchieveDataType)Enum.Parse(typeof(AchievementData.AchieveDataType), MissionRecord.ConditionScript);
        if (AchievementData.Instance.GetData(conditionNum) >= MissionRecord.ConditionNum)
        {
            MissionState = MissionState.Done;
            return true;
        }

        return false;
    }

    public void GetAward()
    {
        if (MissionRecord.AwardType == 0)
        {
            PlayerDataPack.Instance.AddGold(MissionRecord.AwardNum);
        }
        else if (MissionRecord.AwardType == 1)
        {
            PlayerDataPack.Instance.AddDiamond(MissionRecord.AwardNum);
        }
    }

}

