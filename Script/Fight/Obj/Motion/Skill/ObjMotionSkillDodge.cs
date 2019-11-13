using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillDodge : ObjMotionSkillBase
{

    new public float SkillActSpeed
    {
        get
        {
            if (_SkillActSpeed < 0)
            {
                //_SkillActSpeed = _MotionManager.RoleAttrManager.AttackSpeed;
                _SkillActSpeed = 0;
                _SkillActSpeed += _SkillAddSpeed;
            }
            return _SkillActSpeed;
        }
    }
}
