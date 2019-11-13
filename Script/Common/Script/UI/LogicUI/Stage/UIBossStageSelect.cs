
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIBossStageSelect : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIBossStageSelect, UILayer.PopUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitDiffs();
    }

    #region difficult panel

    public UIContainerSelect _DiffContainer;

    private int _SelectedDiff = 0;

    public void InitDiffs()
    {
        int maxDiff = ActData.Instance.GetBossDiff();
        //int maxDiff = 9;

        List<int> diffList = new List<int>();
        for (int i = 1; i < maxDiff + 1; ++i)
        {
            diffList.Add(i);
        }
        _DiffContainer.InitSelectContent(diffList, new List<int>() { maxDiff }, OnDiffSelect);
    }

    public void OnDiffSelect(object diffObj)
    {
        _SelectedDiff = (int)diffObj;
        InitStages();

        UnSelectAct();
    }

    #endregion

    #region stagePanel

    public UIContainerSelect _StageContainer;

    private BossStageInfoItem _SelectedStage;

    public void InitStages()
    {
        var diffStages = TableReader.BossStage.GetBossRecordsByDiff(_SelectedDiff);
        List<BossStageInfoItem> bossStageInfos = new List<BossStageInfoItem>();
        for (int i = 0; i < diffStages.Count; ++i)
        {
            BossStageInfoItem bossStageInfoItem = new BossStageInfoItem();
            bossStageInfoItem._StageRecord = diffStages[i];
            bossStageInfoItem._StageIdx = (_SelectedDiff - 1) * ActData._CIRCLE_STAGE_COUNT + i + 1;
            bossStageInfoItem._Level = GameDataValue.GetStageLevel(bossStageInfoItem._StageIdx, STAGE_TYPE.BOSS);

            bossStageInfos.Add(bossStageInfoItem);
        }
        int passStage = ActData.Instance._BossStageIdx - (ActData._CIRCLE_STAGE_COUNT * (_SelectedDiff - 1)) - 1;
        if (passStage <= 0)
        {
            _SelectedStage = bossStageInfos[0];
        }
        else if (passStage >= ActData._CIRCLE_STAGE_COUNT)
        {
            _SelectedStage = bossStageInfos[ActData._CIRCLE_STAGE_COUNT - 1];
        }
        else
        {
            _SelectedStage = bossStageInfos[passStage];
        }

        _StageContainer.InitSelectContent(bossStageInfos, 
            new List<BossStageInfoItem>() { _SelectedStage }, OnSelectStage);
    }

    private void OnSelectStage(object stageObj)
    {
        BossStageInfoItem stageInfo = stageObj as BossStageInfoItem;
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
    public UICurrencyItem _UITicket;
    public Text _CostTicket;
    public Text _ActTicketDesc;

    private void SetStageInfo(BossStageInfoItem stageInfo)
    {
        int stageID = stageInfo._StageIdx;
        int stageLevel = stageInfo._Level;
        _StageName.text = StrDictionary.GetFormatStr(stageInfo._StageRecord.Name);
        _StageLevel.text = stageLevel.ToString();
        _StageDesc.text = StrDictionary.GetFormatStr(stageInfo._StageRecord.Desc);
        _UITicket.ShowOwnCurrency(ActData._BOSS_TICKET);
        _CostTicket.text = "/1";
        _ActTicketDesc.text = "";
        _Condition.text = "";

    }

    public void OnEnterStage()
    {
        if (_ActSelected.activeSelf)
        {
            if (ActData.Instance.IsCanStartAct(_UsingTicket))
            {
                ActData.Instance.StartStage(_SelectedStage._StageIdx, _SelectedActStage._StageRecord.StageType);
                LogicManager.Instance.EnterFight(_SelectedActStage._StageRecord);
            }
        }
        else
        {
            if (ActData.Instance.IsCanStartBossStage())
            {
                ActData.Instance.StartStage(_SelectedStage._StageIdx, STAGE_TYPE.BOSS);
                LogicManager.Instance.EnterFight(Tables.TableReader.StageInfo.GetRecord("100"));
            }
            else
            {
                UIMessageTip.ShowMessageTip(71106);
            }
        }
    }

    public void OnBtnTestPass()
    {
        if (_ActSelected.activeSelf)
        {
            TestPassAct();
        }
        else
        {
            TestPassBoss();
        }
    }

    private void TestPassBoss()
    {
        if (!ActData.Instance.IsCanStartBossStage())
        {
            UIMessageTip.ShowMessageTip(71106);
            return;
        }

        int stageID = _SelectedStage._StageIdx;
        int stageLevel = _SelectedStage._Level;

        ActData.Instance.StartStage(stageID, STAGE_TYPE.BOSS);
        ActData.Instance.PassStage(STAGE_TYPE.BOSS);

        int normalMonstarCnt = 36;
        int eliteMonsterCnt = 4;
        int bossMonsterCnt = 1;

        for (int i = 0; i < normalMonstarCnt; ++i)
        {
            var monRecord = TableReader.MonsterBase.GetRecord("21");
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel, STAGE_TYPE.BOSS);
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
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel, STAGE_TYPE.BOSS);
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
            var monRecord = _SelectedStage._StageRecord.BossID;
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel, STAGE_TYPE.BOSS);
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
    }

    

    #endregion

    #region act 

    public GameObject _ActSelected;
    public GameObject _StagePanel;
    public GameObject _ActPanel;
    public UIContainerSelect _ActStageContainer;

    private static List<string> _StageRecord = new List<string>() { "200", "201" };
    private bool _InitAct = false;
    private StageInfoItem _SelectedActStage;
    private int _UsingTicket = 1;

    public void OnBtnActDiff()
    {
        if (_ActSelected.activeSelf)
            return;

        _ActSelected.SetActive(true);
        _DiffContainer.SetSelect(null);
        SelectAct();
    }

    public void SelectAct()
    {
        _StagePanel.SetActive(false);
        _ActPanel.SetActive(true);
        if (_InitAct)
            return;
        _InitAct = true;

        List<StageInfoItem> stageRecords = new List<StageInfoItem>();
        for (int i = 0; i < _StageRecord.Count; ++i)
        {
            StageInfoItem stageInfo = new StageInfoItem();
            stageInfo._StageRecord = TableReader.StageInfo.GetRecord( _StageRecord[i]);
            stageInfo._StageIdx = 1;
            stageInfo._Level = GameDataValue.GetStageLevel(stageInfo._StageIdx, stageInfo._StageRecord.StageType);
            var stageRecord = TableReader.StageInfo.GetRecord(_StageRecord[i]);
            stageRecords.Add(stageInfo);
        }
        _ActStageContainer.InitSelectContent(stageRecords, new List<StageInfoItem>() { stageRecords[0] }, OnSelectActStage, null);
    }

    public void UnSelectAct()
    {
        _StagePanel.SetActive(true);
        _ActPanel.SetActive(false);
        if (_ActSelected.activeSelf)
        {
            _ActSelected.SetActive(false);
        }
    }

    private void OnSelectActStage(object stageObj)
    {
        StageInfoItem stageInfo = stageObj as StageInfoItem;
        if (stageInfo == null)
            return;

        _SelectedActStage = stageInfo;

        SetStageInfo(_SelectedStage);
        SetActTicketDesc();
    }

    public void AddActUsingTicket()
    {
        ++_UsingTicket;
        SetActTicketDesc();
    }

    public void DecActUsingTicket()
    {
        --_UsingTicket;
        _UsingTicket = Mathf.Max(1, _UsingTicket);
        SetActTicketDesc();
    }

    private void SetActTicketDesc()
    {
        int Ownvalue = BackBagPack.Instance.PageItems.GetItemCnt(ActData._ACT_TICKET);
        _UITicket.ShowCurrency(ActData._ACT_TICKET, Ownvalue);
        _UsingTicket = Mathf.Min(_UsingTicket, Ownvalue, ActData._MAX_ACT_USING_TICKET);
        _CostTicket.text = "/" + _UsingTicket.ToString();
        _ActTicketDesc.text = StrDictionary.GetFormatStr(71105, CommonDefine.GetQualityItemName(ActData._ACT_TICKET, true), _UsingTicket, ActData._DropGoldRateAdds[_UsingTicket]);
    }

    public void AddActTicket()
    {
        ActData.Instance.AddActTicket();
    }

    private void TestPassAct()
    {
        if (!ActData.Instance.IsCanStartAct(_UsingTicket))
        {
            return;
        }

        int stageID = _SelectedActStage._StageIdx;
        int stageLevel = _SelectedActStage._Level;

        ActData.Instance._ActConsumeTickets = _UsingTicket;
        ActData.Instance.StartStage(stageID, _SelectedActStage._StageRecord.StageType);

        int normalMonstarCnt = 450;

        for (int i = 0; i < normalMonstarCnt; ++i)
        {
            var monRecord = TableReader.MonsterBase.GetRecord("21");
            var monsterDrops = MonsterDrop.GetMonsterDrops(monRecord, monRecord.MotionType, stageLevel, _SelectedActStage._StageRecord.StageType);
            foreach (var dropItem in monsterDrops)
            {
                MonsterDrop.PickItem(dropItem);
            }
            int dropExp = GameDataValue.GetMonsterExp(MOTION_TYPE.Normal, stageLevel, RoleData.SelectRole.RoleLevel, _SelectedActStage._StageRecord.StageType);
            RoleData.SelectRole.AddExp(dropExp);

            Hashtable hash = new Hashtable();
            MotionManager objMotion = new MotionManager();
            objMotion.RoleAttrManager = new RoleAttrManager();
            objMotion.RoleAttrManager.InitEnemyAttr(monRecord, stageLevel, MOTION_TYPE.Hero);
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);
        }
        
        ActData.Instance.PassStage(_SelectedActStage._StageRecord.StageType);
        //InitDiffs();
    }

    #endregion
}

