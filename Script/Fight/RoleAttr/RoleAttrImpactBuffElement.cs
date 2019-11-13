using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffElement : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ElementType = args[0];
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
        var impactBuff = skillMotion.GetComponentInChildren<ImpactBuffAttrAdd>();
        var enhanceImpact = impactBuff.gameObject.AddComponent<ImpactBuffAttrAdd>();
        if (impactBuff._Attr == RoleAttrEnum.Attack)
        {
            switch (_ElementType)
            {
                case 0:
                    impactBuff.ModifiedAttr = RoleAttrEnum.FireAttackAdd;
                    enhanceImpact._Attr = RoleAttrEnum.FireEnhance;
                    break;
                case 1:
                    impactBuff.ModifiedAttr = RoleAttrEnum.ColdAttackAdd;
                    enhanceImpact._Attr = RoleAttrEnum.ColdEnhance;
                    break;
                case 2:
                    impactBuff.ModifiedAttr = RoleAttrEnum.LightingAttackAdd;
                    enhanceImpact._Attr = RoleAttrEnum.LightingEnhance;
                    break;
                case 3:
                    impactBuff.ModifiedAttr = RoleAttrEnum.WindAttackAdd;
                    enhanceImpact._Attr = RoleAttrEnum.WindEnhance;
                    break;
            }
        }

        enhanceImpact._AddType = ImpactBuffAttrAdd.ADDTYPE.Value;
        enhanceImpact._AddValue = 10;
        enhanceImpact._LastTime = impactBuff._LastTime + impactBuff.ExLastTime;

    }

    #region 

    public int _ElementType;
    
    #endregion
}
