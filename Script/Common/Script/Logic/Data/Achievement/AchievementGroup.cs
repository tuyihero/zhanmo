using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class AchievementGroup
{

    public AchievementData.AchieveDataType _AchieveType;
    public List<AchievementItem> _AchieveItems = new List<AchievementItem>();
    public AchievementItem _ActingAchieve;

    public void AddInitItem(MissionRecord missionRecord)
    {
        AchievementItem achieveItem = new AchievementItem(missionRecord);
        _AchieveItems.Add(achieveItem);
    }

    public void InitAfterAdd()
    {
        _AchieveItems.Sort((itemA, itemB) =>
        {
            if (itemA.MissionRecord.HardStar > itemB.MissionRecord.HardStar)
                return 1;
            else
                return -1;
        });

        CalActing();
    }

    public void GetAward()
    {
        if (_ActingAchieve.MissionState == MissionState.Done)
        {
            _ActingAchieve.MissionState = MissionState.Finish;
            _ActingAchieve.GetAward();
            AchievementData.Instance.AddAwardIdx(_AchieveType);
            Debug.Log("GetAward:" + _ActingAchieve.MissionRecord.Id);
            CalActing();
        }
    }

    private void CalActing()
    {
        int awardIdx = AchievementData.Instance.GetAwardIdx(_AchieveType);
        awardIdx = Mathf.Clamp(awardIdx, 0, _AchieveItems.Count - 1);
        _ActingAchieve = _AchieveItems[awardIdx];
        _ActingAchieve.OnConditionEvent();
    }

    public float GetConditionProcess()
    {
        float process = (float)AchievementData.Instance.GetData(_AchieveType) / _ActingAchieve.MissionRecord.ConditionNum;
        return Mathf.Clamp(process, 0, 1);
    }

    public string GetConditionProcessText()
    {
        return AchievementData.Instance.GetData(_AchieveType) + " / " + _ActingAchieve.MissionRecord.ConditionNum.ToString();
    }

    public void OnConditionEvent()
    {
        if (_ActingAchieve != null && _ActingAchieve.MissionState == MissionState.Accepted)
        {
            if (_ActingAchieve.OnConditionEvent())
            {
                CalActing();
                //AchievementData.Instance.AddAwardIdx(_AchieveType);
            }
        }
    }
}

