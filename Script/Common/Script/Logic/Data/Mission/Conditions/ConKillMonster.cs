using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class ConKillMonster : MissionConditionBase
{
    private string _MonsterID = "-1";
    private int _MonsterType = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _MonsterID = missionRecord.ConditionParams[0];
        _MonsterType = int.Parse(missionRecord.ConditionParams[1]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var monsterMotion = (MotionManager)eventArgs["MonsterInfo"];

        if (!string.Equals(_MonsterID, "-1") && monsterMotion.MonsterBase.Id != _MonsterID)
            return;

        if (_MonsterType != (int)MOTION_TYPE.Normal && (int)monsterMotion.RoleAttrManager.MotionType != _MonsterType)
            return;

        ++_MissionItem.MissionProcessData;
        _MissionItem.SaveClass(true);
        
    }

    public override float GetConditionProcess()
    {
        return _MissionItem.MissionProcessData / (float)_MissionRecord.ConditionNum;
    }

    public override string GetConditionProcessText()
    {
        return _MissionItem.MissionProcessData + "/" + _MissionRecord.ConditionNum;
    }

    public override bool IsConditionMet()
    {
        if (_MissionItem.MissionProcessData >= _MissionRecord.ConditionNum)
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
