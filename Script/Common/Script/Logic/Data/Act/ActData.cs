using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class ActData : DataPackBase
{
    #region 单例

    private static ActData _Instance;
    public static ActData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ActData();
            }
            return _Instance;
        }
    }

    private ActData()
    {
        _SaveFileName = "ActData";
    }

    #endregion

    #region 

    public static int LEVEL_LIMIT = 10;

    public void InitActData()
    {
        if (_NormalStageIdx <= 0)
            _NormalStageIdx = 0;

        if (_BossStageIdx <= 0)
            _BossStageIdx = 0;
    }

    public int _ProcessStageIdx = 0;
    public STAGE_TYPE _StageMode;
    public int _DefaultStage = 0;

    public void StartDefaultStage()
    {
        if (_DefaultStage == 0)
        {
            _DefaultStage = _NormalStageIdx + 1;
        }

        //if (_DefaultStage >= RoleData.SelectRole.TotalLevel)
        //{
        //    _DefaultStage = RoleData.SelectRole.TotalLevel;
        //}
        //else 
        {
            _DefaultStage = _NormalStageIdx + 1;
        }

        StartStage(_DefaultStage, STAGE_TYPE.NORMAL);
    }

    public void StartCurrentStage()
    {
        if (_ProcessStageIdx > 0)
        {
            StartStage(_ProcessStageIdx, STAGE_TYPE.NORMAL);
        }
        else
        {
            StartDefaultStage();
        }
    }

    public void StartStage(int stageIdx, Tables.STAGE_TYPE stageMode, bool useTicket = false)
    {
        _ProcessStageIdx = stageIdx;
        _StageMode = stageMode;

        if (TestFight.TestMode)
        {
            if (stageMode == STAGE_TYPE.NORMAL)
            {
                TestPassStageOk();
            }
            else if (stageMode == STAGE_TYPE.ACT_GOLD)
            {
                TestPassAct(useTicket);
            }

            UIStageSelect.Refresh();
        }
        else
        {
            if (stageMode == STAGE_TYPE.NORMAL)
            {
                LogicManager.Instance.EnterFight(GetNormalStageRecord(_ProcessStageIdx));
            }
            else if (stageMode == STAGE_TYPE.ACT_GOLD)
            {
                var stageRecord = TableReader.StageInfo.GetRecord(_ActStageRecord[0]);
                if (useTicket)
                {
                    ActData.Instance._ActConsumeTickets = 1;
                }
                LogicManager.Instance.EnterFight(stageRecord);
            }
        }
        //if (stageMode == STAGE_TYPE.NORMAL)
        //{
        //    TestPassStageOk();
        //}
        //else if (stageMode == STAGE_TYPE.ACT_GOLD)
        //{
        //    TestPassAct(useTicket);
        //}

        int combatValue = 0;
        foreach (var equipItem in PlayerDataPack.Instance._SelectedRole.EquipList)
        {
            if (equipItem != null && equipItem.IsVolid())
            {
                combatValue += equipItem.CombatValue;
            }
        }
        Debug.Log("combatValue:" + combatValue);
        Debug.Log("playerLevel:" + RoleData.SelectRole.TotalLevel);
    }

    public int GetStageLevel()
    {
        return GameDataValue.GetStageLevel(_ProcessStageIdx, _StageMode);
    }

    public void PassStage(Tables.STAGE_TYPE stageMode)
    {
        switch (stageMode)
        {
            case STAGE_TYPE.NORMAL:
                SetPassNormalStage(_ProcessStageIdx);
                break;
            case STAGE_TYPE.BOSS:
                SetPassBossStage(_ProcessStageIdx);
                break;

        }

        _ActConsumeTickets = 0;

        Hashtable hash = new Hashtable();
        hash.Add("StageType", stageMode);
        hash.Add("StageIdx", _ProcessStageIdx);
        if (stageMode == STAGE_TYPE.NORMAL)
        {
            hash.Add("StageDiff", GetNormalDiff());
        }
        else
        {
            hash.Add("StageDiff", GetBossDiff());
        }
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, this, hash);
    }

    #endregion

    #region normal 

    [SaveField(1)]
    public int _NormalStageIdx = 0;

    public static int _MAX_NORMAL_DIFF = 9;
    public static int _CIRCLE_STAGE_COUNT = 20;
    public static int _MAX_NROMAL_PRE_STAGE = 20;

    public void SetPassNormalStage(int stageIdx)
    {
        if (FightManager.Instance != null)
        {
            int passStageTime = TableReader.AttrValueLevel.GetSpValue(stageIdx, 35);
            float deltaTime = passStageTime - (Time.time - FightManager.Instance._LogicStartTime);
            if (deltaTime < 0)
                return;
        }

        if (_NormalStageIdx >= stageIdx)
            return;

        _NormalStageIdx = stageIdx;
        _DefaultStage = _NormalStageIdx + 1;

        SaveClass(false);
    }

    public void GetNextNormalStateId()
    {

    }

    public int GetNormalDiff()
    {
        return GameDataValue.GetStageDiff(_ProcessStageIdx, STAGE_TYPE.NORMAL);
        //return (_NormalStageIdx - 1) / _CIRCLE_STAGE_COUNT;
        //return _NormalStageIdx;
    }

    public StageInfoRecord GetNormalStageRecord(int processStageIdx)
    {
        if (processStageIdx <= _CIRCLE_STAGE_COUNT)
        {
            return TableReader.StageInfo.GetRecord(processStageIdx.ToString());
        }
        else
        {
            int stageIdx = processStageIdx % _CIRCLE_STAGE_COUNT + _CIRCLE_STAGE_COUNT;
            if (stageIdx <= _CIRCLE_STAGE_COUNT)
            {
                stageIdx += _CIRCLE_STAGE_COUNT;
            }
            return TableReader.StageInfo.GetRecord(stageIdx.ToString());
        }
    }

    #endregion

    #region boss

    [SaveField(2)]
    public int _BossStageIdx = 0;

    public static int _MAX_BOSS_DIFF = 9;
    public static string _BOSS_TICKET = "1600000";

    public void SetPassBossStage(int stageIdx)
    {
        if (_BossStageIdx >= stageIdx)
            return;
        
        _BossStageIdx = stageIdx;

        SaveClass(false);
    }

    public bool IsCanStartBossStage()
    {
        return BackBagPack.Instance.PageItems.DecItem(ActData._BOSS_TICKET, 1);
    }

    public bool IsBossStageLock(BossStageRecord bossStageRecord)
    {
        int stageID = int.Parse(bossStageRecord.Id);
        if (stageID > _BossStageIdx + 2)
        {
            return false;
        }

        if (RoleData.SelectRole.RoleLevel < bossStageRecord.Level)
        {
            return false;
        }

        if (RoleData.SelectRole._CombatValue < bossStageRecord.Combat)
            return false;

        return true;
    }

    public int GetBossDiff()
    {
        return _BossStageIdx / _MAX_NORMAL_DIFF + 1;
    }

    #endregion

    #region act

    public static List<string> _ActStageRecord = new List<string>() { "200", "201" };
    public static string _ACT_TICKET = "1600001";
    public static int _ACT_TICKET_PRICE = 50;
    public static int _MAX_ACT_USING_TICKET = 1;
    public static int _MAX_START_ACT_LEVEL = 15;
    public static List<float> _DropGoldRateAdds = new List<float>() { 0.5f, 0.5f};
    public static List<float> _DropGoldNumAdds = new List<float>() { 0.0f, 2.0f };
    

    public int _ActConsumeTickets = 0;

    public bool IsCanStartAct(int consumeTickets)
    {
        if (BackBagPack.Instance.PageItems.DecItem(ActData._ACT_TICKET, consumeTickets))
        {
            _ActConsumeTickets = consumeTickets;
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetActDropGoldRateAdd()
    {
        return _DropGoldRateAdds[_ActConsumeTickets];
    }

    public float GetActDropGoldNumAdd()
    {
        return _DropGoldNumAdds[_ActConsumeTickets];
    }

    public void AddActTicket()
    {
        BackBagPack.Instance.PageItems.AddItem(ActData._ACT_TICKET, 1);
    }

    public void ActAsk()
    {

    }

    #endregion

    #region 

    public void TestPassStageOk()
    {
        int stageID = _ProcessStageIdx;
        int stageLevel = GetStageLevel();
        if (RoleData.SelectRole.TotalLevel + ActData.LEVEL_LIMIT < stageLevel)
        {
            UIMessageTip.ShowMessageTip(StrDictionary.GetFormatStr(71103, stageLevel));
            return;
        }

        //ActData.Instance.StartStage(stageID, STAGE_TYPE.NORMAL);
        ActData.Instance.PassStage(STAGE_TYPE.NORMAL);

        int normalMonstarCnt = 0;
        int eliteMonsterCnt = 0;
        int bossMonsterCnt = 1;
        if (stageLevel <= 20)
        {
            normalMonstarCnt = TableReader.AttrValueLevel.GetSpValue(stageLevel, 14);
            eliteMonsterCnt = 0;
        }
        else
        {
            normalMonstarCnt = 60;
            eliteMonsterCnt = 5;
        }

        for (int i = 0; i < normalMonstarCnt; ++i)
        {
            var monRecord = TableReader.MonsterBase.GetRecord("21");
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel);
            foreach (var dropItem in monsterDrops)
            {
                MonsterDrop.PickItem(dropItem);
            }
            int dropExp = GameDataValue.GetMonsterExp(MOTION_TYPE.Normal, stageLevel, RoleData.SelectRole.RoleLevel);
            RoleData.SelectRole.AddExp(dropExp);
            Hashtable hash = new Hashtable();
            MotionManager objMotion = new MotionManager();
            objMotion.RoleAttrManager = new RoleAttrManager();
            objMotion.RoleAttrManager.InitEnemyAttr(monRecord, stageLevel, MOTION_TYPE.Normal);
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);
        }

        for (int i = 0; i < eliteMonsterCnt; ++i)
        {
            var monRecord = TableReader.MonsterBase.GetRecord("22");
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel);
            foreach (var dropItem in monsterDrops)
            {
                MonsterDrop.PickItem(dropItem);
            }
            int dropExp = GameDataValue.GetMonsterExp(MOTION_TYPE.Elite, stageLevel, RoleData.SelectRole.RoleLevel);
            RoleData.SelectRole.AddExp(dropExp);

            Hashtable hash = new Hashtable();
            MotionManager objMotion = new MotionManager();
            objMotion.RoleAttrManager = new RoleAttrManager();
            objMotion.RoleAttrManager.InitEnemyAttr(monRecord, stageLevel, MOTION_TYPE.Elite);
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);
        }

        for (int i = 0; i < bossMonsterCnt; ++i)
        {
            int bossID = _ProcessStageIdx % 20 + 100;
            if (bossID == 100)
            {
                bossID += 20;
            }
            var monRecord = TableReader.MonsterBase.GetRecord(bossID.ToString());
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel);
            foreach (var dropItem in monsterDrops)
            {
                MonsterDrop.PickItem(dropItem);
            }
            int dropExp = GameDataValue.GetMonsterExp(MOTION_TYPE.Hero, stageLevel, RoleData.SelectRole.RoleLevel);
            RoleData.SelectRole.AddExp(dropExp);

            Hashtable hash = new Hashtable();
            MotionManager objMotion = new MotionManager();
            objMotion.RoleAttrManager = new RoleAttrManager();
            objMotion.RoleAttrManager.InitEnemyAttr(monRecord, stageLevel, MOTION_TYPE.Hero);
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);
        }

        
    }

    private void TestPassAct(bool isUsingTicket)
    {
        if (isUsingTicket)
        {
            //if (!ActData.Instance.IsCanStartAct(1))
            //{
            //    return;
            //}

            ActData.Instance._ActConsumeTickets = 1;
        }

        int stageID = 1;
        int stageLevel = RoleData.SelectRole.TotalLevel;

        var actRecord = TableReader.StageInfo.GetRecord(_ActStageRecord[0]);

        //ActData.Instance.StartStage(stageID, actRecord.StageType);

        int normalMonstarCnt = TableReader.AttrValueLevel.GetSpValue(stageLevel, 29);

        for (int i = 0; i < normalMonstarCnt; ++i)
        {
            var monRecord = TableReader.MonsterBase.GetRecord("21");
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel, actRecord.StageType);
            foreach (var dropItem in monsterDrops)
            {
                MonsterDrop.PickItem(dropItem);
            }
            int dropExp = GameDataValue.GetMonsterExp(MOTION_TYPE.Normal, stageLevel, RoleData.SelectRole.RoleLevel, actRecord.StageType);
            RoleData.SelectRole.AddExp(dropExp);

            Hashtable hash = new Hashtable();
            MotionManager objMotion = new MotionManager();
            objMotion.RoleAttrManager = new RoleAttrManager();
            objMotion.RoleAttrManager.InitEnemyAttr(monRecord, stageLevel, MOTION_TYPE.Normal);
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);
        }

        ActData.Instance.PassStage(actRecord.StageType);
        //InitDiffs();
    }

    #endregion

}
