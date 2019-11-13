﻿using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactEleCorePos : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {

    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {

    }

    public new static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var pos = GetPosFromTab(attrTab);
        var value = GetValueFromTab(attrTab);
        var strFormat = StrDictionary.GetFormatStr(attrTab.StrParam[2], pos, GameDataValue.ConfigFloatToPersent(value));
        return strFormat;
    }

    #region 

    public static int GetPosFromTab(AttrValueRecord attrRecord)
    {
        var theValue = attrRecord.AttrParams[0];
        return theValue;
    }

    public static float GetValueFromTab(AttrValueRecord attrRecord)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[1]);
        return theValue;
    }

    #endregion
}