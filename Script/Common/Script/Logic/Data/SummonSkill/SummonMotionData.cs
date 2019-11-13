using System.Collections;
using System.Collections.Generic;
using Tables;

public class SummonMotionData : ItemBase
{
    public string SummonRecordID
    {
        get
        {
            return ItemDataID;
        }
        set
        {
            ItemDataID = value;
        }
    }
    
    public int Exp
    {
        get
        {
            return _DynamicDataInt[1];
        }
        set
        {
            _DynamicDataInt[1] = value;
        }
    }

    public int StarExp
    {
        get
        {
            return _DynamicDataInt[2];
        }
        set
        {
            _DynamicDataInt[2] = value;
        }
    }

    public SummonMotionData()
    {
        SummonRecordID = "";
        _DynamicDataInt.Add(1);
        _DynamicDataInt.Add(0);
        _DynamicDataInt.Add(0);
    }

    public SummonMotionData(string recordID)
    {
        SummonRecordID = recordID;
        _DynamicDataInt.Add(1);
        _DynamicDataInt.Add(0);
        _DynamicDataInt.Add(0);
    }

    private SummonSkillRecord _SummonRecord;
    public SummonSkillRecord SummonRecord
    {
        get
        {
            if (_SummonRecord == null)
            {
                _SummonRecord = Tables.TableReader.SummonSkill.GetRecord(SummonRecordID);
            }
            return _SummonRecord;
        }
    }

    #region level

    private int _Level = -1;
    public int Level
    {
        get
        {
            //if (_Level < 0)
            //{
            //    CalculateLevel();
            //}
            return _Level;
        }
        set
        {
            _Level = value;
        }
    }

    private int _CurLvExp = -1;
    public int CurLvExp
    {
        get
        {
            if (_CurLvExp < 0)
            {
                CalculateLevel();
            }
            return _CurLvExp;
        }
    }

    public bool IsCanAddExp()
    {
        if (Level < StarLevel * 10)
            return true;

        return false;
    }

    public void AddExp(int expValue)
    {
        Exp += expValue;
        CalculateLevel();
    }

    private void CalculateLevel()
    {
        int tempExp = Exp;
        int level = 1;
        while (true)
        {
            var summonAttrRecord = TableReader.SummonSkillAttr.GetRecord(level.ToString());
            if (tempExp >= summonAttrRecord.Cost[0])
            {
                tempExp = tempExp - summonAttrRecord.Cost[0];
                ++level;

                if (level == StarLevel * 10)
                {
                    tempExp = 0;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        _Level = level;
        _CurLvExp = tempExp;

        UpdateAttrs();
    }

    public static int GetLevelByExp(int exp, int starLevel, out int lastExp)
    {
        int tempExp = exp;
        int level = 1;
        while (true)
        {
            var summonAttrRecord = TableReader.SummonSkillAttr.GetRecord(level.ToString());
            if (tempExp >= summonAttrRecord.Cost[0])
            {
                tempExp = tempExp - summonAttrRecord.Cost[0];
                ++level;

                if (level == starLevel * 10)
                {
                    tempExp = 0;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        

        lastExp = tempExp;
        return level;
    }

    #endregion

    #region star

    private int _StarLevel = -1;
    public int StarLevel
    {
        get
        {
            if (_StarLevel < 0)
            {
                CalculateStarLevel();
            }
            return _StarLevel;
        }
    }

    private int _CurStarExp = -1;
    public int CurStarExp
    {
        get
        {
            if (_CurStarExp < 0)
            {
                CalculateStarLevel();
            }
            return _CurStarExp;
        }
    }

    private int _MaxStarExp = -1;
    public int MaxStarExp
    {
        get
        {
            if (_MaxStarExp < 0)
            {
                _MaxStarExp = 0;
                for (int i = 0; i < SummonRecord.StarExp.Count; ++i)
                {
                    _MaxStarExp += SummonRecord.StarExp[i];
                }
            }
            return _MaxStarExp;
        }
    }

    public int GetCanAddExp()
    {
        return MaxStarExp - StarExp;
    }

    public int CurStarLevelExp()
    {
        if (StarLevel < SummonRecord.StarExp.Count)
        {
            return SummonRecord.StarExp[StarLevel];
        }
        return 0;
    }

    public void AddStarExp(int expValue)
    {
        StarExp += expValue;
        CalculateStarLevel();
        UpdateAttrs();
    }

    private void CalculateStarLevel()
    {
        int tempExp = StarExp;
        int starLv = 0;
        for (int i = 0; i < SummonRecord.StarExp.Count; ++i)
        {
            if (tempExp >= SummonRecord.StarExp[i])
            {
                tempExp = tempExp - SummonRecord.StarExp[i];
                ++starLv;
            }
            else
            {
                break;
            }
        }

        _StarLevel = starLv;
        _CurStarExp = tempExp;

        //UpdateAttrs();
    }

    public bool IsStageMax()
    {
        return _StarLevel >= SummonRecord.StarExp.Count;
    }

    public static int GetStarLevelByExp(SummonMotionData motionData, int exp, out int lastExp)
    {
        int tempExp = exp;
        int starLv = 0;
        for (int i = 0; i < motionData.SummonRecord.StarExp.Count; ++i)
        {
            if (tempExp >= motionData.SummonRecord.StarExp[i])
            {
                tempExp = tempExp - motionData.SummonRecord.StarExp[i];
                ++starLv;
            }
            else
            {
                break;
            }
        }

        lastExp = tempExp;
        return starLv;
    }

    #endregion

    #region attr

    public static List<RoleAttrEnum> StageAttrEnums = new List<RoleAttrEnum>()
    { RoleAttrEnum.Attack, RoleAttrEnum.CriticalHitChance, RoleAttrEnum.CriticalHitDamge, RoleAttrEnum.RiseHandSpeed, RoleAttrEnum.AttackSpeed};

    public static List<int> StageAttrValues = new List<int>()
    { 1, 1500,5000,2500,2500 };

    private List<EquipExAttr> _SummonAttrs;
    public List<EquipExAttr> SummonAttrs
    {
        get
        {
            if (_SummonAttrs == null)
            {
                UpdateAttrs();
            }

            return _SummonAttrs;
        }
    }

    public void UpdateAttrs()
    {
        _SummonAttrs = new List<EquipExAttr>();
        _SummonAttrs.Clear();
        //InitStageAttr();

        int atkValue = (int)(GameDataValue.GetSummonAtk(SummonSkillData.Instance.SummonLevel) * GameDataValue.ConfigIntToFloat( SummonRecord.AttrModelfy));
        int critiDmgValue = 0;
        if (StarLevel > 1)
        {
            critiDmgValue = StageAttrValues[1];
        }
        int critiRateValue = 0;
        if (StarLevel > 2)
        {
            critiRateValue = StageAttrValues[2];
        }
        int riseHandSpeedValue = 0;
        if (StarLevel > 3)
        {
            riseHandSpeedValue = StageAttrValues[3];
        }
        int atkSpeedValue = 0;
        if (StarLevel > 4)
        {
            atkSpeedValue = StageAttrValues[4];
        }

        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[0], atkValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[1], critiDmgValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[2], critiRateValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[3], riseHandSpeedValue));
        _SummonAttrs.Add(EquipExAttr.GetBaseExAttr(StageAttrEnums[4], atkSpeedValue));
    }
    #endregion
}

