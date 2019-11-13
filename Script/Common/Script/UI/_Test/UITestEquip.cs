
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;
using System.IO;

public class UITestEquip : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UITestEquip, UILayer.PopUI, hash);
    }

    public static void ActBuffInFight()
    {
        ActBuffInFightInner();
    }

    #endregion

    #region equip/item test

    public InputField _LegencyID;
    public InputField _InputLevel;
    public InputField _InputQuality;
    public InputField _InputValue;

    public InputField _ItemID;
    public InputField _ItemCnt;

    public InputField _EquipPackQuality;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitBuffAttr();
    }

    public override void Hide()
    {
        base.Hide();

        UIEquipTooltips.HideAsyn();
    }

    public void OnBtnOk()
    {
        int level = int.Parse(_InputLevel.text);
        ITEM_QUALITY quality = (ITEM_QUALITY)(int.Parse(_InputQuality.text));
        int value = int.Parse(_InputValue.text);
        int legencyID = 0;
        if (!int.TryParse(_LegencyID.text, out legencyID))
        {
            legencyID = 0;
        }
        var equipItem = ItemEquip.CreateEquip(level, quality, legencyID);
        var newEquip = BackBagPack.Instance.AddEquip(equipItem);
        
    }

    public void OnBtnItem()
    {
        int itemCnt = int.Parse(_ItemCnt.text);
        int itemID = int.Parse(_ItemID.text);
        //if (itemID > 70000 && itemID < 80000)
        //{
        //    GemData.Instance.CreateGem(itemID.ToString(), itemCnt);
        //}
        //else
        {
            ItemBase.CreateItemInPack(itemID.ToString(), itemCnt);
        }
    }

    public void OnBtnAllBaseItem()
    {
        foreach (var equipRecord in TableReader.EquipItem.Records.Values)
        {
            var equipItem = ItemEquip.CreateEquip(1, ITEM_QUALITY.PURPER, -1, -1,-1,int.Parse(equipRecord.Id));
            var newEquip = BackBagPack.Instance.AddEquip(equipItem);
        }
    }

    public void OnBtnPackEquip()
    {
        int equipQuality = int.Parse(_EquipPackQuality.text);
        int professionLimit = 10;
        //if (RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE
        //    || RoleData.SelectRole.Profession == PROFESSION.BOY_DOUGE)
        //{
        //    professionLimit = 5;
        //}
        if (equipQuality >= (int)Tables.ITEM_QUALITY.WHITE && equipQuality <= (int)Tables.ITEM_QUALITY.ORIGIN)
        {
            for (int i = 0; i <= (int)Tables.EQUIP_SLOT.RING; ++i)
            {
                var equipItem = ItemEquip.CreateEquip(RoleData.SelectRole.TotalLevel, (ITEM_QUALITY)equipQuality, -1, i, professionLimit);
                RoleData.SelectRole.PutOnEquip(equipItem.EquipItemRecord.Slot, equipItem);
            }
        }
        else
        {
            for (int i = 0; i <= (int)Tables.EQUIP_SLOT.RING; ++i)
            {
                var equipItem = ItemEquip.CreateEquip(RoleData.SelectRole.TotalLevel, (ITEM_QUALITY)equipQuality, -1, i, professionLimit);
                RoleData.SelectRole.PutOnEquip(equipItem.EquipItemRecord.Slot, equipItem);
            }
        }
    }
    #endregion

    #region fight test

    public InputField _TargetLevel;
    public Toggle _TestAct;
    public Toggle _TestActAD;
    public Toggle _TestBoss;

    public void AutoLevel()
    {
        int targetLevel = int.Parse(_TargetLevel.text);

        int lastLevel = RoleData.SelectRole.TotalLevel;
        Dictionary<int, List<float>> levelValues = new Dictionary<int, List<float>>();

        if (!levelValues.ContainsKey(1))
        {
            levelValues.Add(1, new List<float>());
        }
        levelValues[1].Add(TestFight.GetDamageAttr());
        levelValues[1].Add(TestFight.GetDamageTotalRate());

        while (targetLevel > RoleData.SelectRole.TotalLevel)
        {
            
            ActData.Instance.StartDefaultStage();

            TestFight.DelAllEquip();
            TestFight.DelSkill();
            TestFight.DelGem();

            if (!levelValues.ContainsKey(RoleData.SelectRole.TotalLevel))
            {
                levelValues.Add(RoleData.SelectRole.TotalLevel, new List<float>());
            }
            var damage = TestFight.GetDamageAttr();
            var damageRate = TestFight.GetDamageTotalRate();
            Debug.Log(string.Format("AutoLevel Damage:{0}, Rage:{1}", damage, damageRate));
            levelValues[RoleData.SelectRole.TotalLevel].Add(damage);
            levelValues[RoleData.SelectRole.TotalLevel].Add(damageRate);

            if (_TestAct.isOn && RoleData.SelectRole.TotalLevel >= ActData._MAX_START_ACT_LEVEL)
            {
                if (lastLevel != RoleData.SelectRole.TotalLevel)
                {
                    lastLevel = RoleData.SelectRole.TotalLevel;
                    ActData.Instance.StartStage(1, STAGE_TYPE.ACT_GOLD, _TestActAD.isOn);

                    TestFight.DelAllEquip();
                    TestFight.DelSkill();
                    TestFight.DelGem();

                    var damageAct = TestFight.GetDamageAttr();
                    var damageRateAct = TestFight.GetDamageTotalRate();
                    levelValues[RoleData.SelectRole.TotalLevel].Add(damageAct);
                    levelValues[RoleData.SelectRole.TotalLevel].Add(damageRateAct);
                }
            }
        }

        string filePath = Application.dataPath + "/combatLog.txt";
        StreamWriter writer = new StreamWriter(filePath);
        foreach (var levelValue in levelValues)
        {
            string line = levelValue.Key.ToString();
            for (int i = 0; i < levelValue.Value.Count; ++i)
            {
                line += "\t" + levelValue.Value[i].ToString();
            }
            writer.WriteLine(line);
        }

        writer.Close();
    }

    public void BtnLevel()
    {
        int targetLevel = int.Parse(_TargetLevel.text);
        while (RoleData.SelectRole.TotalLevel < targetLevel)
        {
            int exp = GameDataValue.GetLvUpExp(RoleData.SelectRole.TotalLevel, 1);
            RoleData.SelectRole.AddExp(exp);
        }
        
    }

    private void GetLevelStage(int level, ref int diff, ref int stageIdx)
    {
        diff = level / 20 + 1;
        diff = Math.Max(diff, 1);

        stageIdx = level % 20 + 1;
        stageIdx = Math.Max(stageIdx, 1);
    }

    int _StageIdx = 0;
    int _Diff = 1;

    int _TotalExp = 0;
    int _TotalGold = 0;

    List<PassStageInfo> _PassInfoList = new List<PassStageInfo>();

    class PassStageInfo
    {
        public int diff;
        public int stateIdx;
        public int level;
        public int exp;
        public int gold;
        public int goldDrop;
        public int levelExp;
        public int atk;
        public int damage1;
        public int damage2;
        public int damage3;
        public int def;
        public int hp;
        public int monValue;
        public int equipMat;
        public int equipGem;
        public int equipPoint;
        public int gemCost1;
        public int gemCost2;
    }

    class PassNormalInfo
    {
        public int _Gold;
        public int _Exp;
        public int _MonsterCnt;
        public int _DropEquipCnt;
    }

    public void OnTestPassStage()
    {
        ItemPackTest.Instance.SaveClass(true);
    }

    public void OnTestDelEquips()
    {
        TestFight.DelAllEquip();
    }

    public void SetTestMode(bool isTestMode)
    {
        TestFight.TestMode = isTestMode;
    }
    
    #endregion

    #region global buff test

    public List<string> _AttrIDs;
    public List<Toggle> _TestBuffToggles;

    public static List<EquipExAttr> _ExAttrs = new List<EquipExAttr>();
    public static List<bool> _IsToggleOn = new List<bool>();

    public void InitBuffAttr()
    {
        foreach (var attrID in _AttrIDs)
        {
            var attrRecord = Tables.TableReader.AttrValue.GetRecord(attrID);
            _ExAttrs.Add(attrRecord.GetExAttr(1));
        }

        OnToggleChange();
    }

    public void OnToggleChange()
    {
        _IsToggleOn.Clear();
        for (int i = 0; i < _TestBuffToggles.Count; ++i)
        {
            _IsToggleOn.Add(_TestBuffToggles[i].isOn);
        }
    }

    public static void ActBuffInFightInner()
    {
        for (int i = 0; i < _IsToggleOn.Count; ++i)
        {
            if (_IsToggleOn[i])
            {
                GlobalBuffData.Instance._ExAttrs.Add(_ExAttrs[i]);
            }
        }

        RoleData.SelectRole.CalculateAttr();
    }

    #endregion

    #region element test

    public InputField _FiveElementCoreID;
    public InputField _FiveElementItemLevel;
    public InputField _FiveElementItemNum;

    public void OnBtnElementItem()
    {
        if (!string.IsNullOrEmpty(_FiveElementCoreID.text))
        {
            string coreID = _FiveElementCoreID.text;
            int level = int.Parse(_FiveElementItemLevel.text);
            FiveElementData.Instance.CreateCoreItem(coreID, level);
        }
        else
        {
            int level = int.Parse(_FiveElementItemLevel.text);
            int num = int.Parse(_FiveElementItemNum.text);
            for (int i = 0; i < num; ++i)
            {
                var elementItem = FiveElementData.CreateElementItem(level);
                
            }
        }
    }

    #endregion

    #region summon soul

    public void OnBtnAllSummon()
    {
        var summonTabs = TableReader.SummonSkill.Records.Values;
        foreach (var summonTab in summonTabs)
        {
            var summonData = SummonSkillData.Instance.AddSummonData(summonTab.Id);
            summonData.AddExp(2500);
            summonData.AddStarExp(20);
        }

        SummonSkillData.Instance.RefreshCollection();


    }

    #endregion

    #region stage

    public void OnBtnStageDiff()
    {
        ActData.Instance._BossStageIdx = 200;
        ActData.Instance._NormalStageIdx = 200;
    }

    #endregion


}

