using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactManager
{
    public static RoleAttrImpactBase GetAttrImpact(EquipExAttr equipAttr)
    {
        //if (equipAttr.AttrID >= RoleAttrEnum.Skill1FireBoom && equipAttr.AttrID <= RoleAttrEnum.Skill3WindAimTarget)
        //{
        //    RoleAttrImpactEleBullet impactEleBullet = new RoleAttrImpactEleBullet();
        //    impactEleBullet.InitEleBullet(equipAttr.AttrID, equipAttr.AttrValues[0], equipAttr.AttrValues[1]);
        //    return impactEleBullet;
        //}
        var impactType = Type.GetType(equipAttr.AttrType);
        if (impactType == null)
            return null;

        var impactBase = Activator.CreateInstance(impactType) as RoleAttrImpactBase;
        if (impactBase == null)
            return null;

        impactBase.InitImpact("", equipAttr.AttrParams);
        return impactBase;
    }

    public static RoleAttrImpactBase GetAttrImpact(ItemSkill skillInfo)
    {
        var impactType = Type.GetType(skillInfo.SkillRecord.SkillAttr.AttrImpact);
        if (impactType == null)
            return null;

        var impactBase = Activator.CreateInstance(impactType) as RoleAttrImpactBase;
        if (impactBase == null)
            return null;

        impactBase.InitImpact(skillInfo.SkillRecord.SkillInput, impactBase.GetSkillImpactVal(skillInfo));
        return impactBase;
    }
}
