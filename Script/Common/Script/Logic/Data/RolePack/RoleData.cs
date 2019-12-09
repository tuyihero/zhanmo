using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class RoleData
{
    //default info
    public string MainBaseName;
    public string MotionFold;
    public string ModelName;
    public PROFESSION Profession;
    public string DefaultWeaponModel;
    public string IconName;

    public static RoleData SelectRole
    {
        get
        {
            return PlayerDataPack.Instance._SelectedRole;
        }
    }

    public void InitRoleData()
    {
        bool needSave = false;
        needSave |= InitLevel();
        needSave |= InitEquipList();
    }

    #region equipManager


    private List<ItemEquip> _EquipList;
    public List<ItemEquip> EquipList
    {
        get
        {
            if (_EquipList == null)
            {
                InitEquipList();
            }
            return _EquipList;
        }
    }

    private bool InitEquipList()
    {
        int equipSlotCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length + 1;
        if (PlayerDataPack.Instance._EquipList == null || PlayerDataPack.Instance._EquipList.Count != equipSlotCnt)
        {
            if (PlayerDataPack.Instance._EquipList == null)
            {
                PlayerDataPack.Instance._EquipList = new List<ItemEquip>();
            }

            int startIdx = PlayerDataPack.Instance._EquipList.Count;
            for (int i = startIdx; i < equipSlotCnt; ++i)
            {
                ItemEquip newItemEquip = new ItemEquip("-1");
                //newItemEquip._SaveFileName = _SaveFileName + ".Equip" + i;
                PlayerDataPack.Instance._EquipList.Add(newItemEquip);
            }
            InitEquipFromPlayerData();
            //var equipItem = ItemEquip.CreateBaseWeapon(PROFESSION.BOY_DEFENCE);
            //PutOnEquip(EQUIP_SLOT.WEAPON, equipItem);

            //var equipGirlItem = ItemEquip.CreateBaseWeapon(PROFESSION.GIRL_DOUGE);
            //PutOnEquip(EQUIP_SLOT.WEAPON, equipGirlItem);

            //var equipCloth = ItemEquip.CreateBaseCloth(PROFESSION.GIRL_DOUGE);
            //PutOnEquip(EQUIP_SLOT.TORSO, equipCloth);

            //var equipShoes = ItemEquip.CreateBaseShoes(PROFESSION.GIRL_DOUGE);
            //PutOnEquip(EQUIP_SLOT.LEGS, equipShoes);

            return true;
        }
        else
        {
            InitEquipFromPlayerData();
        }
        foreach (var itemEquip in EquipList)
        {
            if (itemEquip.IsVolid())
            {
                itemEquip.CalculateSet();
                itemEquip.CalculateCombatValue();
            }
        }
        return false;
    }

    private void InitEquipFromPlayerData()
    {
        _EquipList = new List<ItemEquip>();
        int equipSlotCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
        for (int i = 0; i < equipSlotCnt; ++i)
        {
            _EquipList.Add(PlayerDataPack.Instance._EquipList[i]);
        }

        //if (Profession == PROFESSION.GIRL_DEFENCE || Profession == PROFESSION.GIRL_DOUGE)
        //{
        //    _EquipList[0] = PlayerDataPack.Instance._EquipList[equipSlotCnt];
        //}
        
    }

    public ItemEquip GetEquipItem(EQUIP_SLOT equipSlot)
    {
        return EquipList[(int)equipSlot];
    }

    public ItemEquip GetEquipItemOtherWeaon()
    {
        //if (Profession == PROFESSION.GIRL_DEFENCE || Profession == PROFESSION.GIRL_DOUGE)
        //{
        //    return PlayerDataPack.Instance._EquipList[0];
        //}
        //else
        {
            int equipSlotCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
            return PlayerDataPack.Instance._EquipList[equipSlotCnt];
        }
    }

    public bool IsCanEquipItem(EQUIP_SLOT equipSlot, ItemEquip equipItem)
    {
        if (equipItem == null)
            return false;

        if (equipItem.EquipItemRecord == null)
            return false;

        if (equipItem.EquipItemRecord.Slot != equipSlot)
            return false;

        if (equipItem.RequireLevel > RoleLevel)
        {
            return false;
        }

        //if (equipItem.EquipItemRecord.ProfessionLimit > 0 &&
        //    ((equipItem.EquipItemRecord.ProfessionLimit >> (int)Profession) & 1) == 0)
        //{
        //    return false;
        //}

        return true;
    }

    public bool PutOnEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
    {
        if (!IsCanEquipItem(equipSlot, equipItem))
            return false;

        if (!equipItem.IsMatchRole(Profession))
        {
            if (equipSlot == EQUIP_SLOT.WEAPON)
            {
                //if (Profession == PROFESSION.GIRL_DEFENCE || Profession == PROFESSION.GIRL_DOUGE)
                //{
                //    PlayerDataPack.Instance._EquipList[0].ExchangeInfo(equipItem);
                //}
                //else
                {
                    int equipSlotCnt = Enum.GetValues(typeof(EQUIP_SLOT)).Length;
                    PlayerDataPack.Instance._EquipList[equipSlotCnt].ExchangeInfo(equipItem);
                }
            }
        }
        else
        {
            EquipList[(int)equipSlot].ExchangeInfo(equipItem);
        }

        UIEquipPack.RefreshBagItems();

        PlayerDataPack.Instance.SaveClass(true);

        CalculateAttr();

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", equipItem);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_PUT_ON, this, hash);

        return true;
    }

    public void PutOffEquip(EQUIP_SLOT equipSlot, ItemEquip equipItem)
    {
        EquipList[(int)equipSlot].CalculateCombatValue();
        var backPackPos = BackBagPack.Instance.AddEquip(equipItem);

        UIEquipPack.RefreshBagItems();

        CalculateAttr();

        PlayerDataPack.Instance.SaveClass(true);
    }



    public string GetWeaponModelName()
    {
        var equip = GetEquipItem(EQUIP_SLOT.WEAPON);
        if (equip != null && equip.EquipItemRecord != null)
        {
            return equip.EquipItemRecord.Model;
        }
        else
        {
            return DefaultWeaponModel;
        }
    }

    #endregion

    #region role attr

    public float GetBaseMoveSpeed()
    {
        return 4.5f;
    }

    public int GetBaseAttackSpeed()
    {
        return 1;
    }

    public int GetBaseAttack()
    {
        return 10;
    }

    public int GetBaseHP()
    {
        return 65;
    }

    public int GetBaseDefence()
    {
        return 5;
    }

    //baseAttrs
    public RoleAttrStruct _BaseAttr = new RoleAttrStruct();

    public void CalculateAttr()
    {
        _BaseAttr.ResetBaseAttr();
        SetRoleLevelAttr(_BaseAttr);
        SkillData.Instance.SetSkillAttr(_BaseAttr);
        SetEquipAttr(_BaseAttr);
        GemData.Instance.SetGemAttr(_BaseAttr);
        LegendaryData.Instance.SetAttr(_BaseAttr);
        GlobalBuffData.Instance.SetAttr(_BaseAttr);
        FiveElementData.Instance.SetAttr(_BaseAttr);

        CalculateSecondAttr(_BaseAttr);
        CalculateCombatValue(_BaseAttr);
    }

    public void SetRoleLevelAttr(RoleAttrStruct roleAttr)
    {
        roleAttr.SetValue(RoleAttrEnum.Strength, Strength);
        roleAttr.SetValue(RoleAttrEnum.Dexterity, Dexterity);
        roleAttr.SetValue(RoleAttrEnum.Vitality, Vitality);
        roleAttr.SetValue(RoleAttrEnum.Intelligence, Intelligence);
        roleAttr.SetValue(RoleAttrEnum.Attack, RoleLevel * GameDataValue._AtkPerRoleLevel + GameDataValue._AtkRoleLevelBase);
        roleAttr.SetValue(RoleAttrEnum.HPMax, RoleLevel * GameDataValue._HPPerRoleLevel + GameDataValue._HPRoleLevelBase);
        roleAttr.SetValue(RoleAttrEnum.CriticalHitChance, 500);
        roleAttr.SetValue(RoleAttrEnum.CriticalHitDamge, 5000);
        roleAttr.SetValue(RoleAttrEnum.MPMax, 100);
    }

    public void SetEquipAttr(RoleAttrStruct roleAttr)
    {
        foreach (var equipInfo in EquipList)
        {
            equipInfo.SetEquipAttr(roleAttr);
        }
    }

    public void SetGemAttr(RoleAttrStruct roleAttr)
    {
        GemData.Instance.SetGemAttr(roleAttr);
    }

    public void CalculateSecondAttr(RoleAttrStruct roleAttr)
    {
        var strength = roleAttr.GetValue(RoleAttrEnum.Strength);
        var baseAttack = roleAttr.GetValue(RoleAttrEnum.Attack);
        float attackByStrength = strength * GameDataValue._AttackPerStrength;
        float PhyEnhanceByStrength = strength * GameDataValue._DmgEnhancePerStrength;
        roleAttr.AddValue(RoleAttrEnum.Attack, (int)attackByStrength);
        roleAttr.AddValue(RoleAttrEnum.PhysicDamageEnhance, (int)PhyEnhanceByStrength);

        var dexteriry = roleAttr.GetValue(RoleAttrEnum.Dexterity);
        int def = (int)(dexteriry * GameDataValue._DefencePerDex);
        //int criticalRate = (int)(dexteriry * GameDataValue._CriticalRatePerDex);
        //int criticalDamage = (int)(dexteriry * GameDataValue._CriticalDmgPerDex);
        //int ignoreAttack = (int)(dexteriry * GameDataValue._IgnoreAtkPerDex);
        roleAttr.AddValue(RoleAttrEnum.Defense, def);

        var intelligence = roleAttr.GetValue(RoleAttrEnum.Intelligence);
        int eleAtk = (int)(intelligence * GameDataValue._EleAtkPerInt);
        int eleEnhance = (int)(intelligence * GameDataValue._EleEnhancePerInt);

        roleAttr.AddValue(RoleAttrEnum.FireAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.FireEnhance, eleEnhance);
        roleAttr.AddValue(RoleAttrEnum.ColdAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.ColdEnhance, eleEnhance);
        roleAttr.AddValue(RoleAttrEnum.LightingAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.LightingEnhance, eleEnhance);
        roleAttr.AddValue(RoleAttrEnum.WindAttackAdd, eleAtk);
        roleAttr.AddValue(RoleAttrEnum.WindEnhance, eleEnhance);

        var vitality = roleAttr.GetValue(RoleAttrEnum.Vitality);
        int hpByVitality = (int)(vitality * GameDataValue._HPPerVit);
        int finalDamageReduse = (int)(vitality * GameDataValue._FinalDmgRedusePerVit);
        roleAttr.AddValue(RoleAttrEnum.HPMax, hpByVitality);
        roleAttr.AddValue(RoleAttrEnum.FinalDamageReduse, finalDamageReduse);

        var allEleAtk = roleAttr.GetValue(RoleAttrEnum.AllEleAtk);
        if (allEleAtk > 0)
        {
            roleAttr.AddValue(RoleAttrEnum.FireAttackAdd, allEleAtk);
            roleAttr.AddValue(RoleAttrEnum.ColdAttackAdd, allEleAtk);
            roleAttr.AddValue(RoleAttrEnum.LightingAttackAdd, allEleAtk);
            roleAttr.AddValue(RoleAttrEnum.WindAttackAdd, allEleAtk);
        }
        var allEleResist = roleAttr.GetValue(RoleAttrEnum.AllResistan);
        if (allEleResist > 0)
        {
            roleAttr.AddValue(RoleAttrEnum.FireResistan, allEleResist);
            roleAttr.AddValue(RoleAttrEnum.ColdResistan, allEleResist);
            roleAttr.AddValue(RoleAttrEnum.LightingResistan, allEleResist);
            roleAttr.AddValue(RoleAttrEnum.WindResistan, allEleResist);
        }
        var allEleEnhance = roleAttr.GetValue(RoleAttrEnum.AllEnhance);
        if (allEleEnhance > 0)
        {
            roleAttr.AddValue(RoleAttrEnum.FireEnhance, allEleEnhance);
            roleAttr.AddValue(RoleAttrEnum.ColdEnhance, allEleEnhance);
            roleAttr.AddValue(RoleAttrEnum.LightingEnhance, allEleEnhance);
            roleAttr.AddValue(RoleAttrEnum.WindEnhance, allEleEnhance);
        }
    }

    public void CalculateCombatValue(RoleAttrStruct roleAttr)
    {
        _CombatValue = roleAttr.GetValue(RoleAttrEnum.Attack)
            + roleAttr.GetValue(RoleAttrEnum.FireAttackAdd)
            + roleAttr.GetValue(RoleAttrEnum.ColdAttackAdd)
            +roleAttr.GetValue(RoleAttrEnum.LightingAttackAdd)
            +roleAttr.GetValue(RoleAttrEnum.WindAttackAdd);
    }

    #endregion

    #region attr Points

    public static int MAX_ROLE_LEVEL = 50;
    public static int POINT_PER_ROLE_LEVEL = 4;
    public static int POINT_PER_ATTR_LEVEL = 1;
    public static int ATTR_PER_LEVEL = 0;

    public int RoleLevel
    {
        get
        {
            return PlayerDataPack.Instance.RoleLevel;
        }
    }

    public int AttrLevel
    {
        get
        {
            return 0;
        }
    }

    public int TotalLevel
    {
        get
        {
            return RoleLevel + AttrLevel;
        }
    }

    public int CurExp
    {
        get
        {
            return PlayerDataPack.Instance.CurExp;
        }
    }

    private int _AddStrength = 0;
    public int Strength
    {
        get
        {
            return (RoleLevel + AttrLevel) * ATTR_PER_LEVEL + _AddStrength;
        }
    }

    private int _AddDexterity = 0;
    public int Dexterity
    {
        get
        {
            return (RoleLevel + AttrLevel) * ATTR_PER_LEVEL + _AddDexterity;
        }
    }

    private int _AddVitality = 0;
    public int Vitality
    {
        get
        {
            return (RoleLevel + AttrLevel) * ATTR_PER_LEVEL + _AddVitality;
        }
    }

    private int _AddIntelligence = 0;
    public int Intelligence
    {
        get
        {
            return (RoleLevel + AttrLevel) * ATTR_PER_LEVEL + _AddIntelligence;
        }
    }

    private int _UnDistrubutePoint = 0;
    public int UnDistrubutePoint
    {
        get
        {
            return _UnDistrubutePoint;
        }
    }

    public int _CombatValue = 0;

    private static int MAX_LEVEL = 50;
    private int _LvUpExp = 0;
    public int LvUpExp
    {
        get
        {
            return _LvUpExp;
        }
    }

    public bool InitLevel()
    {
        if (RoleLevel <= 0)
        {
            PlayerDataPack.Instance.RoleLevel = 1;
        }
        _LvUpExp = GameDataValue.GetLvUpExp(RoleLevel, AttrLevel);
        return false;
    }

    public void AddExp(int value)
    {
        //if (_RoleLevel < MAX_LEVEL)
        //{
        PlayerDataPack.Instance.CurExp += value;
        if (CurExp >= _LvUpExp)
        {
            PlayerDataPack.Instance.CurExp -= _LvUpExp;
            RoleLevelUp();
        }
        //}
        //else
        //{
        //    _CurExp += value;
        //    if (_CurExp >= _LvUpExp)
        //    {
        //        _CurExp -= _LvUpExp;
        //        AttrLevelUp();
        //    }
        //}

        if (CurExp > _LvUpExp)
        {
            AddExp(0);
        }

    }

    private void RoleLevelUp()
    {
        ++PlayerDataPack.Instance.RoleLevel;
        _UnDistrubutePoint += POINT_PER_ROLE_LEVEL;

        CalculateAttr();

        _LvUpExp = GameDataValue.GetLvUpExp(RoleLevel, AttrLevel);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, this, null);
    }

    private void AttrLevelUp()
    {
        return;
        _UnDistrubutePoint += POINT_PER_ATTR_LEVEL;

        _LvUpExp = GameDataValue.GetLvUpExp(RoleLevel, AttrLevel);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, this, null);
    }

    public void ResetPoints()
    {
        _AddStrength = 0;
        _AddDexterity = 0;
        _AddVitality = 0;
        _AddIntelligence = 0;
        _UnDistrubutePoint = RoleLevel * POINT_PER_ROLE_LEVEL + AttrLevel * POINT_PER_ATTR_LEVEL;

        CalculateAttr();
    }

    public void DistributePoint(int distriAttr, int point)
    {
        _UnDistrubutePoint -= point;
        switch (distriAttr)
        {
            case 1:
                _AddStrength += point;
                break;
            case 2:
                _AddDexterity += point;
                break;
            case 3:
                _AddIntelligence += point;
                break;
            case 4:
                _AddVitality += point;
                break;
        }

        CalculateAttr();
    }

    #endregion

    
}
