
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIStageSelect : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageSelect, UILayer.PopUI, hash);
    }

    public static void Refresh()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIStageSelect>(UIConfig.UIStageSelect);
        if (instance == null)
            return;

        instance.InitPassedStages();
    }

    public static int GetSelectedDiff()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIStageSelect>(UIConfig.UIStageSelect);
        if (instance == null)
            return -1;

        return instance._SelectedDiff;
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        //InitDiffs();
        InitPassedStages();
    }

    #region difficult panel

    public UIContainerSelect _DiffContainer;

    private int _SelectedDiff;

    public void InitDiffs()
    {
        int maxStage = ActData.Instance._NormalStageIdx;
        int maxDiff = maxStage / ActData._CIRCLE_STAGE_COUNT + 1;
        int passStage = maxStage % ActData._CIRCLE_STAGE_COUNT;

        List<int> selected = new List<int>();
        List<int> diffList = new List<int>();
        for (int i = 1; i < maxDiff + 1; ++i)
        {
            diffList.Add(i);
            int minLevel = GameDataValue.GetStageLevel((i-1)* ActData._CIRCLE_STAGE_COUNT + 1, STAGE_TYPE.NORMAL);
            int maxLevel = GameDataValue.GetStageLevel((i) * ActData._CIRCLE_STAGE_COUNT, STAGE_TYPE.NORMAL);
            if (RoleData.SelectRole.TotalLevel + ActData.LEVEL_LIMIT >= minLevel
                && RoleData.SelectRole.TotalLevel + ActData.LEVEL_LIMIT <= maxLevel)
            {
                selected.Add(i);
            }
        }

        if (selected.Count == 0)
        {
            selected.Add(maxDiff);
        }

        _DiffContainer.InitSelectContent(diffList, selected, OnDiffSelect);
    }

    public void OnDiffSelect(object diffObj)
    {
        _SelectedDiff = (int)diffObj;
        InitStages();
    }
    
    #endregion

    #region stagePanel

    public UIContainerSelect _StageContainer;

    private int _MaxStageId = 0;
    public int MaxStageId
    {
        get
        {
            return _MaxStageId;
        }
    }

    private static int StageCircle = 20;
    private int _NormalMinID = 1;
    private int _RandomMinID = 21;

    private StageInfoItem _SelectedStage;

    public void InitStages()
    {

        List<StageInfoItem> stageList = new List<StageInfoItem>();


        for (int i = 0; i < ActData._CIRCLE_STAGE_COUNT; ++i)
        {
            StageInfoItem stageInfo = new StageInfoItem();
            if (ActData.Instance._NormalStageIdx <= _RandomMinID)
            {
                stageInfo._StageRecord = TableReader.StageInfo.GetRecord((i + _NormalMinID).ToString());
            }
            else
            {
                stageInfo._StageRecord = TableReader.StageInfo.GetRecord((i + _RandomMinID).ToString());
            }
            stageInfo._StageIdx = (_SelectedDiff - 1) * ActData._CIRCLE_STAGE_COUNT + i + 1;
            stageInfo._Level = GameDataValue.GetStageLevel(stageInfo._StageIdx, STAGE_TYPE.NORMAL);
            
            stageList.Add(stageInfo);
        }

        List<StageInfoItem> selectedStage = new List<StageInfoItem>();
        int passStage = ActData.Instance._NormalStageIdx - (ActData._CIRCLE_STAGE_COUNT * (_SelectedDiff - 1));
        int maxLevel = GameDataValue.GetStageLevel(ActData.Instance._NormalStageIdx, STAGE_TYPE.NORMAL);

        if (passStage <= 0)
        {
            selectedStage.Add(stageList[0]);
        }
        else if (passStage >= ActData._CIRCLE_STAGE_COUNT)
        {
            selectedStage.Add(stageList[ActData._CIRCLE_STAGE_COUNT - 1]);
        }
        else if (RoleData.SelectRole.TotalLevel + ActData.LEVEL_LIMIT <= maxLevel)
        {
            int delta = maxLevel - RoleData.SelectRole.TotalLevel - ActData.LEVEL_LIMIT;
            selectedStage.Add(stageList[passStage - delta - 1]);
        }
        else
        {
            selectedStage.Add(stageList[passStage]);
        }
        _StageContainer.InitSelectContent(stageList, selectedStage, OnSelectStage);
    }

    public void InitPassedStages()
    {

        if (ActData.Instance._NormalStageIdx == 0)
            return;

        List<StageInfoItem> stageList = new List<StageInfoItem>();
        List<StageInfoItem> selectedStage = new List<StageInfoItem>();

        for (int i = 0; i < ActData._MAX_NROMAL_PRE_STAGE; ++i)
        {
            int stageIdx = ActData.Instance._NormalStageIdx - i + 5;
            if (stageIdx > 0)
            {
                StageInfoItem stageInfo = new StageInfoItem();
                stageInfo._StageRecord = ActData.Instance.GetNormalStageRecord(stageIdx);
                stageInfo._StageIdx = stageIdx;
                stageInfo._Level = GameDataValue.GetStageLevel(stageInfo._StageIdx, STAGE_TYPE.NORMAL);

                stageList.Add(stageInfo);

                if (ActData.Instance._NormalStageIdx == 0 || stageInfo._StageIdx == ActData.Instance._NormalStageIdx + 1)
                {
                    selectedStage.Add(stageInfo);
                }
            }
        }

        stageList.Reverse();
        
        
        _StageContainer.InitSelectContent(stageList, selectedStage, OnSelectStage);
    }

    private void OnSelectStage(object stageObj)
    {
        StageInfoItem stageInfo = stageObj as StageInfoItem;
        if (stageInfo == null)
            return;

        _SelectedStage = stageInfo;

        SetStageInfo(_SelectedStage);
    }

    #endregion

    #region stageInfo

    public Text _StageName;
    public Text _StageLevel;
    public Text _StageDesc;
    public Text _Condition;
    public Image _BossIcon;
    public UIContainerBase _BossDrops;

    private void SetStageInfo(StageInfoItem stage)
    {
        int stageID = _SelectedStage._StageIdx;
        int stageLevel = _SelectedStage._Level;
        _StageName.text = StrDictionary.GetFormatStr(stage._StageRecord.Name);
        string colorStageLevel = stageLevel.ToString();
        if (RoleData.SelectRole.TotalLevel - stageLevel >= 5)
        {
            colorStageLevel = StrDictionary.GetFormatStr(72004, stageLevel.ToString());
        }
        else if (RoleData.SelectRole.TotalLevel - stageLevel >= -5)
        {
            colorStageLevel = StrDictionary.GetFormatStr(72005, stageLevel.ToString());
        }
        else if (RoleData.SelectRole.TotalLevel - stageLevel >= -10)
        {
            colorStageLevel = StrDictionary.GetFormatStr(72006, stageLevel.ToString());
        }
        else
        {
            colorStageLevel = StrDictionary.GetFormatStr(72007, stageLevel.ToString());
        }
        _StageLevel.text = StrDictionary.GetFormatStr(2300079) + colorStageLevel;
        _StageDesc.text = StrDictionary.GetFormatStr(stage._StageRecord.Desc);

        _Condition.text = "";

        var bossRecord = TableReader.MonsterBase.GetRecord(stage._StageRecord.ExParam[2].ToString());
        ResourceManager.Instance.SetImage(_BossIcon, bossRecord.HeadIcon);

        var dropItems = bossRecord.ValidSpDrops;
        _BossDrops.InitContentItem(dropItems, OnDropClick);
        //if (stageLevel > RoleData.SelectRole.TotalLevel + 10)
        //{
        //    _Condition.text = StrDictionary.GetFormatStr(71100);
        //}
        //else if (stageID < GetMaxStageID())
        //{
        //    _Condition.text = StrDictionary.GetFormatStr(71102);
        //}
    }

    public void OnDropClick(object spDrop)
    {
        var commonItem = spDrop as CommonItemRecord;
        var equipItem = Tables.TableReader.EquipItem.GetRecord(commonItem.Id);
        UILegendaryItemTooltips.ShowAsyn(equipItem);
    }

    public void OnEnterStage()
    {
        //int stageLevel = _SelectedStage._Level;
        //if (RoleData.SelectRole.TotalLevel + ActData.LEVEL_LIMIT < stageLevel)
        //{
        //    UIMessageTip.ShowMessageTip(StrDictionary.GetFormatStr( 71103, stageLevel));
        //    return;
        //}

        //if (BackBagPack.Instance.PageItems.GetItemCnt(GameDataValue._BOSS_TICKET_ID) >= GameDataValue._MONSTER_DROP_BOSS_TICKET_MAX_STORE)
        //{
        //    UIMessageBox.Show(71107, OnEnterStageOk, null);
        //}
        //else
        //{
        //    OnEnterStageOk();
        //}

        if (ActData.Instance._NormalStageIdx + 1 < _SelectedStage._StageIdx)
        {
            UIMessageTip.ShowMessageTip(71102);
            return;
        }

        UIStageDiffTips.ShowForEnsure(_SelectedStage._StageIdx, ()=>
        {
            ActData.Instance.StartStage(_SelectedStage._StageIdx, STAGE_TYPE.NORMAL);
        });
        
    }

    private void OnEnterStageOk()
    {
        ActData.Instance.StartStage(_SelectedStage._StageIdx, STAGE_TYPE.NORMAL);
        //LogicManager.Instance.EnterFight(_SelectedStage._StageRecord);
    }

    #endregion

    #region test pass

    public void TestPassStage()
    {
        if (BackBagPack.Instance.PageItems.GetItemCnt(GameDataValue._BOSS_TICKET_ID) >= GameDataValue._MONSTER_DROP_BOSS_TICKET_MAX_STORE)
        {
            UIMessageBox.Show(71107, TestPassStageOk, null);
        }
        else
        {
            TestPassStageOk();
        }
    }

    public void TestPassStageOk()
    {
        int stageID = _SelectedStage._StageIdx;
        int stageLevel = _SelectedStage._Level;
        if (RoleData.SelectRole.TotalLevel + ActData.LEVEL_LIMIT < stageLevel)
        {
            UIMessageTip.ShowMessageTip(StrDictionary.GetFormatStr(71103, stageLevel));
            return;
        }

        ActData.Instance.StartStage(stageID, STAGE_TYPE.NORMAL);
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
            normalMonstarCnt = 180;
            eliteMonsterCnt = 20;
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
            var monRecord = TableReader.MonsterBase.GetRecord("1");
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

        InitDiffs();

        //GemData.Instance.AutoLevelAll();

        Debug.Log("Player level:" + RoleData.SelectRole.TotalLevel);
    }

    #endregion
}

