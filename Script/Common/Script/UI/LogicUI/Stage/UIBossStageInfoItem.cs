using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class BossStageInfoItem
{
    public BossStageRecord _StageRecord;
    public int _Level;
    public int _StageIdx;
}

public class UIBossStageInfoItem : UIStageInfoItem
{
    protected BossStageInfoItem _ShowBossItem;
    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (BossStageInfoItem)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(BossStageInfoItem showItem)
    {
        _ShowBossItem = showItem;

        _StageName.text = StrDictionary.GetFormatStr(showItem._StageRecord.Name);

        int stageID = showItem._StageIdx;
        int combatLimit = GameDataValue.GetBossStageLimitCombat(showItem._Level);
        if (RoleData.SelectRole._CombatValue < combatLimit)
        {
            _ConditionTips = CommonDefine.GetEnableRedStr(0) + StrDictionary.GetFormatStr(71104, combatLimit) + "</color>";
        }
        else if (ActData.Instance._BossStageIdx + 1 < stageID)
        {
            _ConditionTips = StrDictionary.GetFormatStr(71104, combatLimit);
        }
        else
        {
            _ConditionTips = "";
        }
        _StageCondition.text = _ConditionTips;

        if (string.IsNullOrEmpty(_ConditionTips))
        {
            _LockedGO.SetActive(false);
        }
        else
        {
            _LockedGO.SetActive(true);
        }
    }

    public override void OnItemClick()
    {
        if (_LockedGO.activeSelf)
        {
            int stageId = _ShowBossItem._StageIdx;
            if (ActData.Instance._BossStageIdx < stageId)
            {
                UIMessageTip.ShowMessageTip(71102);
            }
            else
            {
                UIMessageTip.ShowMessageTip(71101);
            }
            return;
        }

        base.OnItemClick();
    }
}

