using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffStack : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ValueModify = (int)args[0];
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillActureLevel);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactBuff = skillMotion.GetComponentInChildren<ImpactBuffDebuff>(true);
        //for (int i = 0; i < impactBuffs.Length; ++i)
        {
            impactBuff.MaxStack = 1 + _ValueModify;
        }
    }

    #region 

    public int _ValueModify;
    
    #endregion
}
