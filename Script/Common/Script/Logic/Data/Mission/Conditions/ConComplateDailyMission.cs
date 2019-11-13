using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConComplateDailyMission : MissionConditionBase
{
    private bool _IsChallengeMission = false;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        _IsChallengeMission = int.Parse(_MissionRecord.ConditionParams[0]) > 0;
    }

    public override float GetConditionProcess()
    {
        if (_IsChallengeMission)
        {
            int complateMissionCnt = 0;
            foreach (var missionItem in MissionData.Instance._ChallengeItems)
            {
                if (missionItem._MissionState == MissionState.Done
                    || missionItem._MissionState == MissionState.Finish)
                {
                    ++complateMissionCnt;
                }
            }
            return complateMissionCnt / (float)MissionData.Instance._ChallengeItems.Count;
        }
        else
        {
            int complateMissionCnt = 0;
            foreach (var missionItem in MissionData.Instance._MissionItems)
            {
                if (missionItem._MissionState == MissionState.Done
                    || missionItem._MissionState == MissionState.Finish)
                {
                    ++complateMissionCnt;
                }
            }
            return complateMissionCnt / (float)MissionData.Instance._MissionItems.Count;
        }
    }

    public override string GetConditionProcessText()
    {
        if (_IsChallengeMission)
        {
            int complateMissionCnt = 0;
            foreach (var missionItem in MissionData.Instance._ChallengeItems)
            {
                if (missionItem._MissionState == MissionState.Done
                    || missionItem._MissionState == MissionState.Finish)
                {
                    ++complateMissionCnt;
                }
            }
            return complateMissionCnt + "/" + MissionData.Instance._ChallengeItems.Count;
        }
        else
        {
            int complateMissionCnt = 0;
            foreach (var missionItem in MissionData.Instance._MissionItems)
            {
                if (missionItem._MissionState == MissionState.Done
                    || missionItem._MissionState == MissionState.Finish)
                {
                    ++complateMissionCnt;
                }
            }
            return complateMissionCnt + "/" + MissionData.Instance._MissionItems.Count;
        }
    }

    public override bool IsConditionMet()
    {
        if (_IsChallengeMission)
        {
            int complateMissionCnt = 0;
            foreach (var missionItem in MissionData.Instance._ChallengeItems)
            {
                if (missionItem._MissionState == MissionState.Done
                    || missionItem._MissionState == MissionState.Finish)
                {
                    ++complateMissionCnt;
                }
            }
            if (complateMissionCnt == MissionData.Instance._ChallengeItems.Count)
                return true;
        }
        else
        {
            int complateMissionCnt = 0;
            foreach (var missionItem in MissionData.Instance._MissionItems)
            {
                if (missionItem._MissionState == MissionState.Done
                    || missionItem._MissionState == MissionState.Finish)
                {
                    ++complateMissionCnt;
                }
            }
            if (complateMissionCnt == MissionData.Instance._MissionItems.Count)
                return true;
        }
        return false;
    }

    public override void ConditionGoto()
    {
        UIGemPack.ShowAsyn();
    }
}
