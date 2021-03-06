﻿using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassiveBloodSucking : RoleAttrImpactPassive
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());

        _ActRate = GetValueFromTab(attrTab, args[1]);
        _BloodRate = GetValue2FromTab(attrTab, args[1]);
        _ActCD = 0;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        ResourcePool.Instance.LoadConfig("Bullet\\Passive\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var buffGO = resGO;
            buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
            var buffs = buffGO.GetComponents<ImpactBuffBloodSucking>();
            foreach (var buff in buffs)
            {
                buff._ActCD = _ActCD;
                buff._Rate = _ActRate;
                buff._SuckingRate = _BloodRate;
                buff.ActImpact(roleMotion, roleMotion);
            }
        }, null);
        
    }

    public new static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var value1 = GetValueFromTab(attrTab, attrParams[1]);
        var value2 = GetValue2FromTab(attrTab, attrParams[1]);
        var strFormat = StrDictionary.GetFormatStr(attrTab.StrParam[2], GameDataValue.ConfigIntToPersent(value1), GameDataValue.ConfigFloatToPersent(value2));
        return strFormat;
    }

    #region 

    private float _ActCD;
    private int _ActRate;
    private float _BloodRate;

    private static int GetValueFromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = (attrRecord.AttrParams[0] + attrRecord.AttrParams[1] * level);
        theValue = Mathf.Min(theValue, (attrRecord.AttrParams[2]));
        return theValue;
    }

    private static float GetValue2FromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[3] + attrRecord.AttrParams[4] * level);
        theValue = Mathf.Min(theValue, GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[5]));
        return theValue;
    }


    #endregion
}
