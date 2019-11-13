using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;

public class RoleAttrImpactSkillDamage : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _DamageModify = GameDataValue.ConfigIntToFloat(args[0]);
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();

        valList.Add(GameDataValue.GetSkillDamageRate(skillInfo.SkillActureLevel, skillInfo.SkillRecord.EffectValue));

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var skillDamages = skillMotion.GetComponentsInChildren<ImpactDamage>(true);
        foreach (var damage in skillDamages)
        {
            if (damage._IsCharSkillDamage)
            {
                damage._DamageRate *= _DamageModify;
            }
        }
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        var damageModify = GameDataValue.GetSkillDamageRate(attrParams[1], skillRecord.EffectValue);
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, GameDataValue.ConfigIntToPersent((int)(damageModify)));
        return strFormat;
    }

    #region 

    public float _DamageModify;
    
    #endregion
}
