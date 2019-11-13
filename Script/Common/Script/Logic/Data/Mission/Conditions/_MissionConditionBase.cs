using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class MissionConditionBase
{
    protected MissionItem _MissionItem;
    protected MissionRecord _MissionRecord;

    public virtual void InitCondition(MissionItem missionData, MissionRecord missionRecord)
    {
        _MissionItem = missionData;
        _MissionRecord = missionRecord;
    }

    public virtual bool IsConditionMet()
    {
        return true;
    }

    public virtual float GetConditionProcess()
    {
        return 0;
    }

    public virtual string GetConditionProcessText()
    {
        return "";
    }

    public virtual void ConditionGoto()
    {

    }
}
