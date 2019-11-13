using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactBase
{
    public string _SkillInput;

    public virtual void InitImpact(string skillInput, List<int> args)
    {

    }

    public virtual void InitImpact(ItemEquip equip)
    { }

    public virtual List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        return new List<int>();
    }

    public virtual bool AddData(List<int> attrParam)
    {
        return false;
    }

    public virtual void ModifySkillBeforeInit(MotionManager roleMotion)
    {

    }

    public virtual void ModifySkillAfterInit(MotionManager roleMotion)
    {

    }


}
