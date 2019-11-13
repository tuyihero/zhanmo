using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

public class EquipExAttr
{
    public static EquipExAttr GetBaseExAttr(RoleAttrEnum roleAttr, int value)
    {
        EquipExAttr exAttr = new EquipExAttr();
        exAttr.AttrType = "RoleAttrImpactBaseAttr";
        exAttr.Value = value;
        exAttr.AttrParams.Add((int)roleAttr);
        exAttr.AttrParams.Add(Mathf.CeilToInt(GameDataValue.GetAttrToValue(roleAttr) * value));

        return exAttr;
    }

    public string AttrType;

    public int Value;

    public List<int> AttrParams;

    public ITEM_QUALITY AttrQuality = ITEM_QUALITY.BLUE;

    public void InitAttrQuality()
    {
        if (AttrType != "RoleAttrImpactBaseAttr")
        {
            AttrQuality = ITEM_QUALITY.ORIGIN;
            return;
        }
        if (AttrParams.Count < 3)
        {
            AttrQuality = ITEM_QUALITY.WHITE;
            return;
        }

        AttrQuality = (ITEM_QUALITY)AttrParams[2];
    }


    public EquipExAttr()
    {
        AttrParams = new List<int>();
        //AttrValues.Add(0);
    }

    public EquipExAttr(string attrType, int value, params int[] attrValues)
    {
        AttrType = attrType;
        Value = value;
        AttrParams = new List<int>(attrValues);

        InitAttrQuality();
    }

    public EquipExAttr(EquipExAttr copyInstance)
    {
        AttrType = copyInstance.AttrType;
        Value = copyInstance.Value;
        AttrParams = copyInstance.AttrParams;

        InitAttrQuality();
    }

    public string GetAttrStr(bool eqiupAttr = true)
    {
        var attrStr = GetAttrStr(AttrType, AttrParams);

        if (eqiupAttr)
        {
            if (!AttrType.Equals("RoleAttrImpactBaseAttr") 
                && !AttrType.Equals("RoleAttrImpactSetAttrByEquip")
                && !AttrType.Equals("RoleAttrImpactEleCorePos")
                && !AttrType.Equals("RoleAttrImpactEleCoreAttr"))
            {
                attrStr += " Lv." + AttrParams[1];
            }
        }

        return attrStr;
    }

    public static string GetAttrStr(string attrType, List<int> attrParams)
    {
        var impactType = Type.GetType(attrType);
        if (impactType == null)
            return "";

        var method = impactType.GetMethod("GetAttrDesc");
        if (method == null)
            return "";

        var attrStr = method.Invoke(null, new object[] { attrParams }) as string;

        return attrStr;
    }

    public bool Add(EquipExAttr d)
    {
        if (d.AttrType != AttrType)
            return false;

        if (AttrType == "RoleAttrImpactBaseAttr")
        {
            for (int i = 0; i < AttrParams.Count; ++i)
            {
                AttrParams[i] += d.AttrParams[i];
            }
            return true;
        }

        return false;
    }
}

public class ItemEquip : ItemBase
{
    public ItemEquip(string datID) : base(datID)
    {

    }

    public ItemEquip():base()
    {

    }

    #region equipData

    private static int MAX_INT_CNT = 5;
    public List<int> DynamicDataInt
    {
        get
        {
            if (_DynamicDataInt == null || _DynamicDataInt.Count == 0)
            {
                _DynamicDataInt = new List<int>() { 0, 0, 0, 0 };
            }
            else if (_DynamicDataInt.Count < MAX_INT_CNT)
            {
                for (int i = 0; i < MAX_INT_CNT; ++i)
                {
                    _DynamicDataInt.Add(0);
                }
            }
            return _DynamicDataInt;
        }
    }

    private EquipItemRecord _EquipItemRecord;
    public EquipItemRecord EquipItemRecord
    {
        get
        {
            if (_EquipItemRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _EquipItemRecord = TableReader.EquipItem.GetRecord(_ItemDataID);
            }
            return _EquipItemRecord;
        }
    }

    public override CommonItemRecord CommonItemRecord
    {
        get
        {
            if (_CommonItemRecord == null)
            {
                if (string.IsNullOrEmpty(CommonItemDataID.ToString()))
                    return null;

                if (CommonItemDataID <= 0)
                    return null;

                _CommonItemRecord = TableReader.CommonItem.GetRecord(CommonItemDataID.ToString());
            }
            return _CommonItemRecord;
        }
    }

    public int EquipLevel
    {
        get
        {
            return DynamicDataInt[0];
        }
        set
        {
            DynamicDataInt[0] = value;
        }
    }

    public ITEM_QUALITY EquipQuality
    {
        get
        {
            return (ITEM_QUALITY)DynamicDataInt[1];
        }
        set
        {
            DynamicDataInt[1] = (int)value;
        }
    }

    public int EquipValue
    {
        get
        {
            return DynamicDataInt[2];
        }
        set
        {
            DynamicDataInt[2] = value;
        }
    }

    public int EquipRefreshCostMatrial
    {
        get
        {
            return DynamicDataInt[3];
        }
        set
        {
            DynamicDataInt[3] = value;
        }
    }

    public int CommonItemDataID
    {
        get
        {
            if (DynamicDataInt == null || DynamicDataInt.Count < 5)
                return -1;
            return DynamicDataInt[4];
        }
        set
        {
            DynamicDataInt[4] = value;
        }
    }

    private int _CombatValue = 0;
    public int CombatValue
    {
        get
        {
            return _CombatValue;
        }
    }

    public void CalculateCombatValue()
    {
        _CombatValue = 0;

        int baseValue = GameDataValue.GetEquipLvValue(EquipLevel, EquipItemRecord.Slot);
        if (EquipItemRecord.Slot != EQUIP_SLOT.AMULET
            && EquipItemRecord.Slot != EQUIP_SLOT.RING)
        {
            if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
            {
                _CombatValue += baseValue * 2 + 100;
            }
            else if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO)
            {
                _CombatValue += (int)(baseValue * 1.5f) + 75;
            }
            else if (EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
            {
                _CombatValue += (int)(baseValue * 1.5f) + 75;
            }

            var record = TableReader.EquipItem.GetRecord(CommonItemDataID.ToString());
            if (record.LevelLimit == 10)
            {
                _CombatValue += (int)(_CombatValue * 0.05f);
            }
            else if (record.LevelLimit == 20)
            {
                _CombatValue += (int)(_CombatValue * 0.1f);
            }
            else if (record.LevelLimit == 30)
            {
                _CombatValue += (int)(_CombatValue * 0.15f);
            }
        }
        else
        {
            _CombatValue = 0;
        }

        foreach (var exAttrs in EquipExAttrs)
        {
            if (exAttrs.AttrType != "RoleAttrImpactBaseAttr")
            {
                _CombatValue += (int)(baseValue * 3.0f);
            }
            else
            {
                _CombatValue += (int)(baseValue * GameDataValue._ExAttrQualityPersent[(int)exAttrs.AttrQuality]);
            }

        }
    }

    private List<EquipExAttr> _EquipExAttrs;
    public List<EquipExAttr> EquipExAttrs
    {
        get
        {
            if (_EquipExAttrs == null)
            {
                _EquipExAttrs = new List<global::EquipExAttr>();
                foreach (var strParam in _DynamicDataEx)
                {
                    EquipExAttr exAttr = new global::EquipExAttr();
                    exAttr.AttrType = strParam._StrParams[0];
                    exAttr.Value = int.Parse(strParam._StrParams[1]);
                    for (int i = 2; i < strParam._StrParams.Count; ++i)
                    {
                        exAttr.AttrParams.Add(int.Parse(strParam._StrParams[i]));
                    }
                    exAttr.InitAttrQuality();
                    _EquipExAttrs.Add(exAttr);
                }
            }
            return _EquipExAttrs;
        }
        set
        {
            _EquipExAttrs = value;
            BakeExAttr();
        }
    }

    public bool IsLegandaryEquip()
    {
        if (ItemDataID.Equals(CommonItemDataID.ToString()))
        {
            return false;
        }
        return true;
    }

    public string GetEquipLegendaryID()
    {
        if (IsLegandaryEquip())
        {
            return ItemDataID;
        }
        else
        {
            return "";
        }
    }

    public string GetEquipLegandaryName()
    {
        string equipName = StrDictionary.GetFormatStr(EquipItemRecord.NameStrDict);
        return CommonDefine.GetQualityColorStr(EquipQuality) + equipName + "</color>";
    }

    public string GetEquipNameWithColor()
    {
        string equipName = StrDictionary.GetFormatStr(CommonItemRecord.NameStrDict);
        //if (SpSetRecord != null)
        //{
        //    equipName = StrDictionary.GetFormatStr(SpSetRecord.Name) + "-" + equipName;
        //}
        return CommonDefine.GetQualityColorStr(EquipQuality) + equipName + "</color>";
    }

    public string GetBaseAttrStr()
    {
        string attrStr = "";
        if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
        {
            string attackValue = BaseAttack.ToString();
            if (_ExBaseAtk)
            {
                attackValue = CommonDefine.GetQualityColorStr(ITEM_QUALITY.BLUE) + attackValue + "</color>";
            }
            attrStr = RandomAttrs.GetAttrName(RoleAttrEnum.Attack) + " + " + attackValue;
        }
        else if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO || EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
        {
            string hpValue = BaseHP.ToString();
            if (_ExBaseHp)
            {
                hpValue = CommonDefine.GetQualityColorStr(ITEM_QUALITY.BLUE) + hpValue + "</color>";
            }
            attrStr = RandomAttrs.GetAttrName(RoleAttrEnum.HPMax) + " + " + hpValue;

            string defenceValue = BaseDefence.ToString();
            if (_ExBaseDef)
            {
                defenceValue = CommonDefine.GetQualityColorStr(ITEM_QUALITY.BLUE) + defenceValue + "</color>";
            }
            attrStr += "\n" + RandomAttrs.GetAttrName(RoleAttrEnum.Defense) + " + " + defenceValue;
        }
        return attrStr;

    }

    public bool IsMatchRole(PROFESSION profession)
    {
        if (EquipItemRecord.ProfessionLimit > 0 &&
            ((EquipItemRecord.ProfessionLimit >> (int)profession) & 1) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    #region fun

    public override void RefreshItemData()
    {
        base.RefreshItemData();
        _EquipItemRecord = null;
        _EquipExAttrs = null;
        _BaseAttack = -1;
        _BaseHP = -1;
        _BaseDefence = -1;

        if (EquipItemRecord != null)
        {
            CalculateCombatValue();
        }
    }

    public override void ResetItem()
    {
        base.ResetItem();
        _EquipItemRecord = null;
    }

    public override ITEM_QUALITY GetQuality()
    {
        return EquipQuality;
    }

    public override int GetLevel()
    {
        return EquipLevel;
    }

    #endregion

    #region equipExAttr

    public EquipExAttr GetExAttr(int idx)
    {
        if (EquipExAttrs.Count > idx)
            return EquipExAttrs[idx];
        return null;
    }

    public void AddExAttr(EquipExAttr attr)
    {
        EquipExAttrs.Add(attr);
        ItemExData exData = new ItemExData();
        exData._StrParams.Add(attr.AttrType);
        exData._StrParams.Add(attr.Value.ToString());
        for (int i = 0; i < attr.AttrParams.Count; ++i)
        {
            exData._StrParams.Add(attr.AttrParams[i].ToString());
        }
        _DynamicDataEx.Add(exData);
    }

    public void AddExAttr(List<EquipExAttr> attrs)
    {
        foreach (var exAttr in attrs)
        {
            AddExAttr(exAttr);
        }
    }

    public void BakeExAttr()
    {
        _DynamicDataEx.Clear();
        foreach (var exAttr in EquipExAttrs)
        {
            ItemExData exData = new ItemExData();
            exData._StrParams.Add(exAttr.AttrType);
            exData._StrParams.Add(exAttr.Value.ToString());
            for (int i = 0; i < exAttr.AttrParams.Count; ++i)
            {
                exData._StrParams.Add(exAttr.AttrParams[i].ToString());
            }
            _DynamicDataEx.Add(exData);
        }
        SaveClass(true);
    }


    #endregion

    #region equipBase

    private int _BaseAttack = -1;
    private bool _ExBaseAtk = false;
    public int BaseAttack
    {
        get
        {
            if (_BaseAttack < 0)
            {
                if (EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
                {
                    _BaseAttack = GameDataValue.CalWeaponAttack(EquipLevel);
                    if (BaseModelAttrs.Count > 0 && BaseModelAttrs[0].AttrType == "RoleAttrImpactBaseAttr" && BaseModelAttrs[0].AttrParams[0] == (int)RoleAttrEnum.AttackPersent)
                    {
                        _ExBaseAtk = true;
                        _BaseAttack += Mathf.CeilToInt(_BaseAttack * GameDataValue.ConfigIntToFloatDex1(BaseModelAttrs[0].AttrParams[1]));
                    }
                }
                else
                {
                    _BaseAttack = 0;
                }
                
            }
            return _BaseAttack;
        }
    }

    private int _BaseHP = -1;
    private bool _ExBaseHp = false;
    public int BaseHP
    {
        get
        {
            if (_BaseHP < 0)
            {
                _BaseHP = 0;
                if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO)
                {
                    _BaseHP = GameDataValue.CalEquipTorsoHP(EquipLevel);
                }
                else if(EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
                {
                    _BaseHP = GameDataValue.CalEquipLegsHP(EquipLevel);
                }
                if (BaseModelAttrs.Count > 0 && BaseModelAttrs[0].AttrType == "RoleAttrImpactBaseAttr" && BaseModelAttrs[0].AttrParams[0] == (int)RoleAttrEnum.HPMaxPersent)
                {
                    _ExBaseHp = true;
                    _BaseHP += Mathf.CeilToInt(_BaseHP * GameDataValue.ConfigIntToFloatDex1(BaseModelAttrs[0].AttrParams[1]));
                }
            }
            return _BaseHP;
        }
    }

    private int _BaseDefence = -1;
    private bool _ExBaseDef = false;
    public int BaseDefence
    {
        get
        {
            if (_BaseDefence < 0)
            {
                _BaseDefence = 0;
                if (EquipItemRecord.Slot == EQUIP_SLOT.TORSO)
                {
                    _BaseDefence = GameDataValue.CalEquipTorsoDefence(EquipLevel);
                }
                else if (EquipItemRecord.Slot == EQUIP_SLOT.LEGS)
                {
                    _BaseDefence = GameDataValue.CalEquipLegsDefence(EquipLevel);
                }
                if (BaseModelAttrs.Count > 0 && BaseModelAttrs[0].AttrType == "RoleAttrImpactBaseAttr" && BaseModelAttrs[0].AttrParams[0] == (int)RoleAttrEnum.DefensePersent)
                {
                    _ExBaseDef = true;
                    _BaseDefence += Mathf.CeilToInt(_BaseDefence * GameDataValue.ConfigIntToFloatDex1(BaseModelAttrs[0].AttrParams[1]));
                }
            }
            return _BaseDefence;
        }
    }

    public void RefreshEquip()
    {
        _BaseAttack = -1;
        _BaseHP = -1;
        _BaseDefence = -1;

        CalculateCombatValue();
    }

    private int _RequireLevel = -1;
    public int RequireLevel
    {
        get
        {
            //if (_RequireLevel < 0)
            {
                int exValue = 0;
                _RequireLevel = EquipLevel;
                //foreach (var exAttr in _DynamicDataVector)
                //{
                //    if (exAttr.AttrID == FightAttr.FightAttrType.LEVEL_REQUIRE)
                //    {
                //        exValue += exAttr.AttrValue1;
                //    }
                //}
                _RequireLevel -= exValue;
            }
            return _RequireLevel;
        }
        set
        {
            _RequireLevel = value;
        }
    }

    public void SetEquipAttr(RoleAttrStruct roleAttr)
    {
        if (!IsVolid())
            return;

        roleAttr.AddValue(RoleAttrEnum.Attack, BaseAttack);
        roleAttr.AddValue(RoleAttrEnum.HPMax, BaseHP);
        roleAttr.AddValue(RoleAttrEnum.Defense, BaseDefence);

        foreach (var exAttrs in EquipExAttrs)
        {
            if (exAttrs.AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)exAttrs.AttrParams[0], exAttrs.AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(exAttrs));
            }
        }

        foreach (var modelAttr in BaseModelAttrs)
        {
            if (modelAttr.AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)modelAttr.AttrParams[0], modelAttr.AttrParams[1]);
            }
        }
    }

    public void SetBaseModelAttrs(EquipItemRecord equipRecord)
    {
        _BaseModelAttrs = new List<EquipExAttr>();
        foreach (var baseAttr in equipRecord.BaseAttrs)
        {
            if (baseAttr == null)
                continue;

            var attrType = (RoleAttrEnum)baseAttr.AttrParams[0];
            int exValue = 0;
            if (attrType == RoleAttrEnum.AttackPersent
                || attrType == RoleAttrEnum.DefensePersent
                || attrType == RoleAttrEnum.HPMaxPersent
                || attrType == RoleAttrEnum.AttackSpeed
                || attrType == RoleAttrEnum.CriticalHitChance)
            {
                exValue = baseAttr.AttrParams[1];
            }
            else
            {
                var attrValue = GameDataValue.CalLvValue(EquipLevel, equipRecord.Slot);
                exValue = (int)(attrValue * GameDataValue.ConfigIntToFloat(baseAttr.AttrParams[1]));
            }
            
            var exAttr = EquipExAttr.GetBaseExAttr((RoleAttrEnum)baseAttr.AttrParams[0], exValue);
            _BaseModelAttrs.Add(exAttr);
        }
    }

    private List<EquipExAttr> _BaseModelAttrs;
    public List<EquipExAttr> BaseModelAttrs
    {
        get
        {
            if (_BaseModelAttrs == null)
            {
                var record = TableReader.EquipItem.GetRecord(CommonItemDataID.ToString());
                SetBaseModelAttrs(record);
            }
            return _BaseModelAttrs;
        }
    }

    #endregion

    #region create equip

    public static ItemEquip CreateEquipByMonster(int level, Tables.ITEM_QUALITY quality, int value)
    {
        return null;
    }

    public static ItemEquip CreateBaseWeapon(PROFESSION profession)
    {
        int weapon = 1;
        //if (profession == PROFESSION.GIRL_DEFENCE || profession == PROFESSION.GIRL_DOUGE)
        //{
        //    weapon = 1001;
        //}
        ItemEquip itemEquip = new ItemEquip(weapon.ToString());
        itemEquip.EquipLevel = 1;
        itemEquip.EquipQuality = ITEM_QUALITY.WHITE;
        itemEquip.EquipValue = 10;
        itemEquip.RequireLevel = 1;
        itemEquip.CommonItemDataID = weapon;

        itemEquip.CalculateCombatValue();

        return itemEquip;
    }

    public static ItemEquip CreateBaseCloth(PROFESSION profession)
    {
        int equipItem = 10001;
        //if (profession == PROFESSION.GIRL_DEFENCE || profession == PROFESSION.GIRL_DOUGE)
        //{
        //    equipItem = 10001;
        //}
        ItemEquip itemEquip = new ItemEquip(equipItem.ToString());
        itemEquip.EquipLevel = 1;
        itemEquip.EquipQuality = ITEM_QUALITY.WHITE;
        itemEquip.EquipValue = 10;
        itemEquip.RequireLevel = 1;
        itemEquip.CommonItemDataID = equipItem;

        itemEquip.CalculateCombatValue();

        return itemEquip;
    }

    public static ItemEquip CreateBaseShoes(PROFESSION profession)
    {
        int equipItem = 11001;
        //if (profession == PROFESSION.GIRL_DEFENCE || profession == PROFESSION.GIRL_DOUGE)
        //{
        //    equipItem = 11001;
        //}
        ItemEquip itemEquip = new ItemEquip(equipItem.ToString());
        itemEquip.EquipLevel = 1;
        itemEquip.EquipQuality = ITEM_QUALITY.WHITE;
        itemEquip.EquipValue = 10;
        itemEquip.RequireLevel = 1;
        itemEquip.CommonItemDataID = equipItem;

        itemEquip.CalculateCombatValue();

        return itemEquip;
    }

    public static ItemEquip CreateEquip(int level, Tables.ITEM_QUALITY quality, int legencyEquipID = -1, int equipSlotIdx = -1, int prePro = -1, int baseItem = -1)
    {
        Tables.ITEM_QUALITY equipQuality = quality;
        EquipItemRecord legencyEquip = null;
        if (legencyEquipID > 0)
        {
            legencyEquip = TableReader.EquipItem.GetRecord(legencyEquipID.ToString());
        }
        if (legencyEquip != null)
        {
            equipQuality = ITEM_QUALITY.ORIGIN;
        }

        EquipItemRecord baseEquip = null;
        CommonItemRecord commonItemRecord = null;

        EQUIP_SLOT equipSlot = EQUIP_SLOT.AMULET;
        int profession = prePro;
        if (equipSlotIdx < 0)
        {
            equipSlot = GameDataValue.GetRandomItemSlot(quality);
        }
        else
        {
            equipSlot = (EQUIP_SLOT)equipSlotIdx;
        }
        if (legencyEquip != null)
        {
            profession = legencyEquip.ProfessionLimit;
            equipSlot = legencyEquip.Slot;
        }

        if(profession <= 0)
        {
            if (equipSlot == EQUIP_SLOT.WEAPON)
            {
                int randomVal = UnityEngine.Random.Range(0, 2);
                if (randomVal == 0)
                {
                    profession = 5;
                }
                else
                {
                    profession = 10;
                }
            }
        }

        var equipLevel = GameDataValue.GetEquipLv(equipSlot, level);
        int value = GameDataValue.GetEquipLvValue(equipLevel, equipSlot);
        if (baseItem < 0)
        {
            baseEquip = GetRandomItem(equipSlot, equipLevel, profession);
        }
        else
        {
            baseEquip = Tables.TableReader.EquipItem.GetRecord(baseItem.ToString());
        }
        if (baseEquip == null)
            return null;

        commonItemRecord = TableReader.CommonItem.GetRecord(baseEquip.Id);

        string equipID = baseEquip.Id;
        if (legencyEquip != null)
        {
            equipID = legencyEquip.Id;
        }

        ItemEquip itemEquip = new ItemEquip(equipID);
        itemEquip.EquipLevel = equipLevel;
        itemEquip.EquipQuality = equipQuality;
        itemEquip.EquipValue = value;
        itemEquip.RequireLevel = equipLevel;
        itemEquip.CommonItemDataID = int.Parse(commonItemRecord.Id);
        itemEquip.SetBaseModelAttrs(baseEquip);

        //RandomEquipAttr(itemEquip);
        itemEquip.AddExAttr(RandomAttrs.GetRandomEquipExAttrs(baseEquip.Slot, equipLevel, value, equipQuality, RoleData.SelectRole.Profession, legencyEquip));
        if (legencyEquip != null)
        {
            var exLevel = GameDataValue.GetLegencyLv(itemEquip.EquipLevel);
            EquipExAttr legencyAttr = legencyEquip.ExAttr[exLevel - 1].GetExAttr(exLevel);
            itemEquip.AddExAttr(legencyAttr);
        }

        itemEquip.CalculateCombatValue();

        return itemEquip;
    }

    public static ItemEquip GetBaseEquip(string id, int level, ITEM_QUALITY quality, int value, int requireLv)
    {
        ItemEquip itemEquip = new ItemEquip(id);
        itemEquip.EquipLevel = level;
        itemEquip.EquipQuality = quality;
        itemEquip.EquipValue = value;
        itemEquip.RequireLevel = requireLv;

        return itemEquip;
    }


    private static EquipItemRecord GetRandomItem(EQUIP_SLOT equipSlot, int level, int profession)
    {
        List<EquipItemRecord> randomItems = null;
        if (equipSlot == EQUIP_SLOT.WEAPON)
        {
            if (profession == 5)
            {
                randomItems = TableReader.EquipItem.AxeWeapons;
            }
            else
            {
                randomItems = TableReader.EquipItem.SwordWeapons;
            }
        }
        else
        {
            randomItems = TableReader.EquipItem.ClassedEquips[equipSlot];
        }

        int value = randomItems.Count / 4;
        List<int> randomVals = new List<int>();

        for (int i = 0; i < randomItems.Count; ++i)
        {
            if (level >= randomItems[i].LevelLimit && randomItems[i].LevelLimit == 0)
            {
                randomVals.Add(10);
            }
            else if (level >= randomItems[i].LevelLimit && randomItems[i].LevelLimit == 10)
            {
                randomVals.Add(20);
            }
            else if (level >= randomItems[i].LevelLimit && randomItems[i].LevelLimit == 20)
            {
                randomVals.Add(30);
            }
            else if (level >= randomItems[i].LevelLimit && randomItems[i].LevelLimit == 30)
            {
                randomVals.Add(40);
            }
        }

        int randomIdx = GameRandom.GetRandomLevel(randomVals.ToArray());

        return randomItems[randomIdx];
    }

    

    #endregion

    #region equip sp attr

    public static string _DefauletSpSetID = "1";
    public static int _ActSetLeastExCnt = 3;
    public static float _ActSetValPersent = 1.1f;
    public static int _MinLv = 25;

    private EquipSpAttrRecord _SpSetRecord;
    public EquipSpAttrRecord SpSetRecord
    {
        get
        {
            return _SpSetRecord;
        }
    }

    public void CalculateSet()
    {
    //    if (_SpSetRecord != null)
    //    {
    //        EquipSet.Instance.RemoveActingSpAttr(_SpSetRecord, EquipValue);
    //    }
    //    _SpSetRecord = null;
    //    if (EquipExAttrs.Count < _ActSetLeastExCnt)
    //    {
    //        return;
    //    }
    //    if (EquipLevel < _MinLv)
    //        return;

    //    int valAttrCnt = 0;
    //    List<EquipExAttr> randomAttrs = new List<global::EquipExAttr>();
    //    foreach (var exAttr in EquipExAttrs)
    //    {
    //        if (exAttr.AttrType != "RoleAttrImpactBaseAttr")
    //        {
    //            ++valAttrCnt;
    //            continue;
    //        }


    //        if (exAttr.AttrQuality == ITEM_QUALITY.ORIGIN)
    //        {
    //            ++valAttrCnt;
    //        }

    //        if ((exAttr.AttrParams[0] != (int)RoleAttrEnum.AttackPersent
    //            && exAttr.AttrParams[0] != (int)RoleAttrEnum.HPMaxPersent
    //            && exAttr.AttrParams[0] != (int)RoleAttrEnum.MoveSpeed))
    //        {
    //            randomAttrs.Add(exAttr);
    //        }
    //    }
    //    if (valAttrCnt < _ActSetLeastExCnt)
    //    {
    //        return;
    //    }


    //    foreach (var setRecord in TableReader.EquipSpAttr.Records.Values)
    //    {
    //        if (setRecord.Id == _DefauletSpSetID)
    //            continue;

    //        _SpSetRecord = setRecord;
    //        foreach (var exAttr in randomAttrs)
    //        {
    //            bool isAttrOk = false;
    //            for (int i = 0; i < setRecord.ExAttrEnum.Count; ++i)
    //            {
    //                if (exAttr.AttrParams[0] == setRecord.ExAttrEnum[i])
    //                {
    //                    isAttrOk = true;
    //                    break;
    //                }
    //            }
    //            if (!isAttrOk)
    //            {
    //                _SpSetRecord = null;
    //                break;
    //            }
    //        }
    //        if (_SpSetRecord != null)
    //        {
    //            break;
    //        }
    //    }

    //    if (_SpSetRecord == null)
    //    {
    //        _SpSetRecord = TableReader.EquipSpAttr.GetRecord(_DefauletSpSetID);
    //    }
    //    EquipSet.Instance.ActingSpAttr(_SpSetRecord, EquipValue);
    }

    public static bool IsAttrSpToEquip(EquipExAttr exAttr)
    {
        if (exAttr.AttrType != "RoleAttrImpactBaseAttr")
            return false;

        if (((RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.AttackPersent
                        || (RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.DefensePersent
                        || (RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.HPMaxPersent
                        || (RoleAttrEnum)exAttr.AttrParams[0] == RoleAttrEnum.MoveSpeed))
        {
            return true;
        }
        return false;
    }

    public static bool IsAttrBaseAttr(EquipExAttr exAttr)
    {
        if (exAttr.AttrType != "RoleAttrImpactBaseAttr")
            return false;

        return true;
    }
    #endregion
}

