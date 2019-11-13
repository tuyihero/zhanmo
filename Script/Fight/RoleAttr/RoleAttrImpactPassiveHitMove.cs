﻿using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassiveHitMove : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _MoveSpeed = GetValueFromTab(attrTab, args[1]);
        _LastTime = GetValue2FromTab(attrTab, args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        ResourcePool.Instance.LoadConfig("Bullet\\Passive\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var buffGO = resGO;
            buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
            var buffs = buffGO.GetComponents<ImpactBuff>();
            foreach (var buff in buffs)
            {
                var subBuffs = buffGO.GetComponentsInChildren<ImpactBuffAttrAdd>();
                foreach (var subBuff in subBuffs)
                {
                    if (subBuff.gameObject == buffGO)
                        continue;
                    subBuff._LastTime = _LastTime;
                    subBuff._AddValue = _MoveSpeed;
                }

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
        var strFormat = StrDictionary.GetFormatStr(attrDescID, GameDataValue.ConfigIntToPersent((int)value1), value2);
        return strFormat;
    }
    #region 

    private float _LastTime;
    private float _MoveSpeed;

    private static float GetValueFromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[0] + attrRecord.AttrParams[1] * (level - 1));
        theValue = Mathf.Min(theValue, GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[2]));
        return theValue;
    }

    private static float GetValue2FromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[3] + attrRecord.AttrParams[4] * (level - 1));
        theValue = Mathf.Min(theValue, GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[5]));
        return theValue;
    }
    #endregion
}