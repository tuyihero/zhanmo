using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillInHit : ObjMotionSkillBase
{
    public bool _ActInHit = true;
    public bool _ActInFly = true;
    public bool _ActInLie = true;
    public bool _ActInRise = true;

    public override bool ActSkill(Hashtable exhash)
    {
        MotionManager._StateFly.ResetFly();
        MotionManager.ResetMove();
        return base.ActSkill(exhash);
    }


    public override bool IsCanActSkill()
    {
        if (_MotionManager.IsMotionDie)
            return false;

        if (_ActInHit && _MotionManager._ActionState == _MotionManager._StateHit)
            return true;

        if (_ActInFly && _MotionManager._ActionState == _MotionManager._StateFly)
            return true;

        if (_ActInLie && _MotionManager._ActionState == _MotionManager._StateLie)
            return true;

        if (_ActInRise && _MotionManager._ActionState == _MotionManager._StateRise)
            return true;

        return false;
    }
}
