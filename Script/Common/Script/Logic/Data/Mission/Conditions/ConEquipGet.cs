using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConEquipGet : MissionConditionBase
{
    private string _EquipID = "-1";
    private int _EquipQuality = 0;

    public override void InitCondition(MissionItem missionData, Tables.MissionRecord missionRecord)
    {
        base.InitCondition(missionData, missionRecord);
        _EquipID = missionRecord.ConditionParams[0];
        _EquipQuality = int.Parse(missionRecord.ConditionParams[1]);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, EventDelegate);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        var equipInfo = (ItemEquip)eventArgs["EquipInfo"];

        if (!string.Equals(_EquipID, "-1") && equipInfo.EquipItemRecord.Id != _EquipID)
            return;

        if (_EquipQuality != -1 && (int)equipInfo.EquipQuality != _EquipQuality)
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
