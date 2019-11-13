using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConEquipRefreshTimes : MissionConditionBase
{

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
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
        UIGemPack.ShowAsyn();
    }
}
