﻿using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassiveSubLastTime : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _SubBuffLastTime = GetSubBuffLastTimeFromTab(attrTab, args[1]);
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
                var subBuffs = buffGO.GetComponentsInChildren<ImpactBuff>();
                foreach (var subBuff in subBuffs)
                {
                    if (subBuff.gameObject == buffGO)
                        continue;
                    subBuff._LastTime = _SubBuffLastTime;
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
        var lastTime = GetSubBuffLastTimeFromTab(attrTab, attrParams[1]);
        var strFormat = StrDictionary.GetFormatStr(attrDescID, (lastTime));
        return strFormat;
    }

    #region 

    private float _SubBuffLastTime;

    private static float GetSubBuffLastTimeFromTab(AttrValueRecord attrRecord, int level)
    {
        var lastTime = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[0] + attrRecord.AttrParams[1] * (level - 1));
        lastTime = Mathf.Min(lastTime, attrRecord.AttrParams[2]);
        return lastTime;
    }
    
    #endregion
}