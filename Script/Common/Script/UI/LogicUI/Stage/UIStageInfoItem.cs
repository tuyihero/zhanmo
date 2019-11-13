
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class StageInfoItem
{
    public StageInfoRecord _StageRecord;
    public int _Level;
    public int _StageIdx;
}

public class UIStageInfoItem : UIItemSelect
{
    public Text _StageName;
    public Image _StageIcon;
    public GameObject _LockedGO;
    public Text _StageCondition;

    protected string _ConditionTips;
    protected StageInfoItem _ShowItem;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (StageInfoItem)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(StageInfoItem showItem)
    {
        _ShowItem = showItem;
        _StageName.text = StrDictionary.GetFormatStr(_ShowItem._StageRecord.Name);
        ResourceManager.Instance.SetImage(_StageIcon, showItem._StageRecord.Icon);
        int stageId = _ShowItem._StageIdx;

        _ConditionTips = "";

        int stageLevel = _ShowItem._Level;
        if (showItem._StageRecord.StageType == STAGE_TYPE.ACT_GEM
            || showItem._StageRecord.StageType == STAGE_TYPE.ACT_GOLD)
        {
            stageLevel = showItem._Level;
        }
        else
        {
            
            /*if (RoleData.SelectRole.TotalLevel + ActData.LEVEL_LIMIT < stageLevel)
            {
                _ConditionTips = CommonDefine.GetEnableRedStr(0) + StrDictionary.GetFormatStr(71103, stageLevel) + "</color>";
            }
            else */if (ActData.Instance._NormalStageIdx + 1 < stageId)
            {
                _ConditionTips = StrDictionary.GetFormatStr(71103, stageLevel);
            }
            _StageCondition.text = _ConditionTips;
        }

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
        //if (_LockedGO.activeSelf)
        //{
        //    if (ActData.Instance._NormalStageIdx < _ShowItem._StageIdx)
        //    {
        //        UIMessageTip.ShowMessageTip(71102);
        //    }
        //    else
        //    {
        //        UIMessageTip.ShowMessageTip(71100);
        //    }
        //    return;
        //}

        base.OnItemClick();
    }
}

