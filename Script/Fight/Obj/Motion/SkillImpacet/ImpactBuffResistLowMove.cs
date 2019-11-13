using UnityEngine;
using System.Collections;

public class ImpactBuffResistLowMove : ImpactBuff
{
    public override bool IsCanAddBuff(ImpactBuff newBuff)
    {
        if (newBuff is ImpactBuffAttrAdd)
        {
            ImpactBuffAttrAdd attrBuff = newBuff as ImpactBuffAttrAdd;
            if (attrBuff._Attr == RoleAttrEnum.MoveSpeed && attrBuff._AddValue < 0)
            {
                return false;
            }
        }

        return true;
    }
}
