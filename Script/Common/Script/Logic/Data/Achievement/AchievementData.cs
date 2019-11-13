using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class AchievementData : DataPackBase
{

    #region 唯一

    private static AchievementData _Instance = null;
    public static AchievementData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new AchievementData();
            }
            return _Instance;
        }
    }

    private AchievementData()
    {
        _SaveFileName = "AchievementData";
    }

    #endregion

    #region 

    public Dictionary<string, AchievementGroup> _AchieveGroup = new Dictionary<string, AchievementGroup>();

    public void InitMissionData()
    {
        InitAchieveConditions();

        foreach (var missionTab in TableReader.Mission.Records)
        {
            if (missionTab.Value.Achieve == 1)
            {
                if (!_AchieveGroup.ContainsKey(missionTab.Value.SubClass))
                {
                    AchievementGroup achieveGroup = new AchievementGroup();
                    achieveGroup._AchieveType = (AchieveDataType)System.Enum.Parse(typeof(AchieveDataType), missionTab.Value.SubClass);
                    _AchieveGroup.Add(missionTab.Value.SubClass, achieveGroup);
                }

                _AchieveGroup[missionTab.Value.SubClass].AddInitItem(missionTab.Value);
            }
        }

        foreach (var achieveGroup in _AchieveGroup)
        {
            achieveGroup.Value.InitAfterAdd();
        }
    }

    #endregion

    #region achieve data

    public enum AchieveDataType
    {
        ConPassNormalStage,
        ConKillMonster,
        ConKillBoss,
        ConPassBossStage,
        ConRoleLevel,
        ConWatchMovie,
        ConEquipGet,
        ConEquipDestory,
        ConEquipRefreshTimes,
        ConGemLvUp,
        ConGambling,
        ConEquipStore
    }

    [SaveField(1)]
    public List<int> _AchieveData = new List<int>();

    [SaveField(2)]
    public List<int> _AchieveAwardIdx = new List<int>();

    public void SetData(AchieveDataType dataType, int value)
    {
        _AchieveData[(int)dataType] = value;
    }

    public void AddData(AchieveDataType dataType, int value)
    {
        _AchieveData[(int)dataType] += value;
    }

    public int GetData(AchieveDataType dataType)
    {
        return _AchieveData[(int)dataType];
    }

    public void AddAwardIdx(AchieveDataType dataType)
    {
        ++_AchieveAwardIdx[(int)dataType];
    }

    public int GetAwardIdx(AchieveDataType dataType)
    {
        return _AchieveAwardIdx[(int)dataType];
    }

    public void InitAchieveConditions()
    {
        if (_AchieveData.Count == 0)
        {
            string[] values = System.Enum.GetNames(typeof(AchieveDataType));
            int dataCnt = values.Length;
            for (int i = 0; i < dataCnt; ++i)
            {
                _AchieveData.Add(0);
                _AchieveAwardIdx.Add(0);
            }
        }
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, EventPassNormalStage);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, EventKillMonster);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, EventLevelUp);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_WATCH_MOVIE, EventWatchMovie);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, EventEquipGet);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_DESTORY, EventEquipDestory);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventEquipRefresh);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_GAMBLING, EventGambling);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, EventEquipStore);
    }

    private void EventPassNormalStage(object go, Hashtable eventArgs)
    {
        var stageType = (Tables.STAGE_TYPE)eventArgs["StageType"];
        if (stageType == Tables.STAGE_TYPE.NORMAL)
        {
            AddData(AchieveDataType.ConPassNormalStage, 1);
            SaveClass(false);

            _AchieveGroup[AchieveDataType.ConPassNormalStage.ToString()].OnConditionEvent();
        }
        else
        {
            AddData(AchieveDataType.ConPassBossStage, 1);
            SaveClass(false);

            _AchieveGroup[AchieveDataType.ConPassBossStage.ToString()].OnConditionEvent();
        }

        
    }

    private void EventKillMonster(object go, Hashtable eventArgs)
    {
        var monsterMotion = (MotionManager)eventArgs["MonsterInfo"];

        if (monsterMotion.RoleAttrManager.MotionType == MOTION_TYPE.Normal
            || monsterMotion.RoleAttrManager.MotionType == MOTION_TYPE.Elite)
        {
            AddData(AchieveDataType.ConKillMonster, 1);
            SaveClass(false);

            _AchieveGroup[AchieveDataType.ConKillMonster.ToString()].OnConditionEvent();
        }
        else
        {
            AddData(AchieveDataType.ConKillBoss, 1);
            SaveClass(false);

            _AchieveGroup[AchieveDataType.ConKillBoss.ToString()].OnConditionEvent();
        }
    }

    private void EventLevelUp(object go, Hashtable eventArgs)
    {
        SetData(AchieveDataType.ConRoleLevel, RoleData.SelectRole.RoleLevel + RoleData.SelectRole.AttrLevel);
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConRoleLevel.ToString()].OnConditionEvent();
    }

    private void EventWatchMovie(object go, Hashtable eventArgs)
    {
        AddData(AchieveDataType.ConWatchMovie, 1);
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConWatchMovie.ToString()].OnConditionEvent();
    }

    private void EventEquipGet(object go, Hashtable eventArgs)
    {
        AddData(AchieveDataType.ConEquipGet, 1);
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConEquipGet.ToString()].OnConditionEvent();
    }

    private void EventEquipDestory(object go, Hashtable eventArgs)
    {
        AddData(AchieveDataType.ConEquipDestory, 1);
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConEquipDestory.ToString()].OnConditionEvent();
    }

    private void EventEquipRefresh(object go, Hashtable eventArgs)
    {
        AddData(AchieveDataType.ConEquipRefreshTimes, 1);
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConEquipRefreshTimes.ToString()].OnConditionEvent();
    }

    private void EventGemLvUp(object go, Hashtable eventArgs)
    {
        AddData(AchieveDataType.ConGemLvUp, 1);
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConGemLvUp.ToString()].OnConditionEvent();
    }

    private void EventGambling(object go, Hashtable eventArgs)
    {
        AddData(AchieveDataType.ConGambling, 1);
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConGambling.ToString()].OnConditionEvent();
    }

    private void EventEquipStore(object go, Hashtable eventArgs)
    {
        SetData(AchieveDataType.ConEquipStore, LegendaryData.Instance.GetLegendatyCnt());
        SaveClass(false);

        _AchieveGroup[AchieveDataType.ConEquipStore.ToString()].OnConditionEvent();
    }

    #endregion

}
