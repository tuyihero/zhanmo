using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConStagePassed : MissionConditionBase
{
    private int _StageType = 0;
    private int _TargetStageIdx = 0;
    private int _TargetStageDiff = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);

        _StageType = int.Parse(_MissionRecord.ConditionParams[0]);
        _TargetStageIdx = int.Parse(_MissionRecord.ConditionParams[1]);
        _TargetStageDiff = int.Parse(_MissionRecord.ConditionParams[2]);

        //GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {

        
    }

    public override float GetConditionProcess()
    {
        var roleCnt = GetRoleCnt();
        return roleCnt / (float)_TargetStageIdx;
    }

    private int GetRoleCnt()
    {
        if (_StageType == (int)Tables.STAGE_TYPE.NORMAL)
        {
            if (ActData.Instance.GetNormalDiff() > _TargetStageDiff)
            {
                return _TargetStageIdx;
            }
            else if (ActData.Instance.GetNormalDiff() < _TargetStageDiff)
            {
                return 0;
            }
            else
            {
                return ActData.Instance._NormalStageIdx;
            }


        }
        else if (_StageType == (int)Tables.STAGE_TYPE.BOSS)
        {
            if (ActData.Instance.GetBossDiff() > _TargetStageDiff)
            {
                return _TargetStageIdx;
            }
            else if (ActData.Instance.GetBossDiff() < _TargetStageDiff)
            {
                return 0;
            }
            else
            {
                return ActData.Instance._BossStageIdx;
            }
        }

        return 0;
    }

    public override string GetConditionProcessText()
    {
        var roleCnt = GetRoleCnt();
        return roleCnt + "/" + _TargetStageIdx;
    }

    public override bool IsConditionMet()
    {
        var roleCnt = GetRoleCnt();
        if (roleCnt >= _TargetStageIdx)
        {
            return true;
        }
        return false;
    }

    public override void ConditionGoto()
    {
        UIStageSelect.ShowAsyn();
    }
}
