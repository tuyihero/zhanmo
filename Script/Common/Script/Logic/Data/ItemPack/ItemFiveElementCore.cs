using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

public class ItemFiveElementCore : ItemFiveElement
{
    public ItemFiveElementCore(string datID) : base(datID)
    {

    }

    public ItemFiveElementCore():base()
    {

    }

    public override void CalculateCombatValue()
    {
        _CombatValue = 0;

        foreach (var exAttrs in EquipExAttrs)
        {
            if (exAttrs.AttrType == "RoleAttrImpactBaseAttr")
            {
                _CombatValue += GameDataValue.GetAttrValue((RoleAttrEnum)exAttrs.AttrParams[0], exAttrs.AttrParams[1]);
            }
            else
            {
                _CombatValue += 0;
            }
        }
    }

    #region elevemt data

    private FiveElementCoreRecord _FiveElementCoreRecord;
    public FiveElementCoreRecord FiveElementCoreRecord
    {
        get
        {
            if (_FiveElementCoreRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _FiveElementCoreRecord = TableReader.FiveElementCore.GetRecord(_ItemDataID);
            }
            return _FiveElementCoreRecord;
        }
    }
    
    public override string GetElementNameWithColor()
    {
        string equipName = StrDictionary.GetFormatStr(CommonItemRecord.NameStrDict);
        return CommonDefine.GetQualityColorStr(FiveElementCoreRecord.Quality) + equipName + "</color>";
    }

    public bool IsHaveCondition(int idx)
    {
        if (idx < 0 || idx >= FiveElementCoreRecord.PosCondition.Count)
            return false;

        if (FiveElementCoreRecord.PosCondition[idx] == -1)
        {
            return false;
        }

        return true;
    }

    public int ConditionState(int idx)
    {
        if (idx < 0 || idx >= FiveElementCoreRecord.PosCondition.Count)
            return -1;

        if (FiveElementCoreRecord.PosCondition[idx] == -1)
        {
            return -1;
        }

        List<int> subCons = new List<int>();
        int tempCon = FiveElementCoreRecord.PosCondition[idx];
        while (tempCon >= 10)
        {
            int subCon = (int)tempCon % 10;
            tempCon /= 10;
            subCons.Add(subCon);
        }
        subCons.Add(tempCon);

        ItemFiveElement usingElement = FiveElementData.Instance._UsingElements[(int)FiveElementCoreRecord.ElementType];
        bool allConditionComplate = true;
        for (int i = 0; i < subCons.Count; ++i)
        {
            if(usingElement.EquipExAttrs.Count <= subCons[i])
            {
                allConditionComplate = false;
                break;
            }

            if (FiveElementCoreRecord.PosAttrLimit[subCons[i]] >= 0 
                && usingElement.EquipExAttrs[subCons[i]].AttrParams[0] != FiveElementCoreRecord.PosAttrLimit[subCons[i]])
            {
                allConditionComplate = false;
                break;
            }
        }

        if (allConditionComplate)
            return 1;

        return 0;
    }

    public string GetConditionDesc(int idx)
    {
        if (idx < 0 || idx >= FiveElementCoreRecord.PosCondition.Count)
            return "";

        if (FiveElementCoreRecord.PosCondition[idx] == -1)
        {
            return "";
        }

        List<int> subCons = new List<int>();
        int tempCon = FiveElementCoreRecord.PosCondition[idx];
        while (tempCon >= 10)
        {
            int subCon = (int)tempCon % 10;
            tempCon /= 10;
            subCons.Add(subCon);
        }
        subCons.Add(tempCon);

        ItemFiveElement usingElement = FiveElementData.Instance._UsingElements[(int)FiveElementCoreRecord.ElementType];
        string desc = "";
        for (int i = 0; i < subCons.Count; ++i)
        {
            string attrStr = "";
            if (FiveElementCoreRecord.PosAttrLimit[subCons[i]] > 0)
            {
                attrStr = StrDictionary.GetFormatStr(FiveElementCoreRecord.PosAttrLimit[subCons[i]]);
            }
            else
            {
                attrStr = StrDictionary.GetFormatStr("1350002");
            }

            desc += StrDictionary.GetFormatStr("1350001", subCons[i] + 1, attrStr);

            if (i != subCons.Count - 1)
            {
                desc += " \n& ";
            }
            
        }

        return desc;
    }

    #endregion

    #region 

    public override ITEM_QUALITY GetQuality()
    {
        return FiveElementCoreRecord.Quality;
    }

    #endregion


}

