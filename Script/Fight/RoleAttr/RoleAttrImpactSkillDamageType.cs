using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactSkillDamageType : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = args[0].ToString();
        _DamageType = (ElementType)args[1];
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var damageImpacts = skillMotion.GetComponentsInChildren<ImpactDamage>();
        foreach (var damage in damageImpacts)
        {
            damage._DamageType = _DamageType;
        }
        //skillMotion.SkillAddSpeed += (_SpeedModify);
    }

    #region 

    public ElementType _DamageType;
    
    #endregion
}
