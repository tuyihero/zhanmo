using UnityEngine;
using System.Collections;

public class ImpactUseSkill : ImpactBase
{
    public string _UseSkillInput = "";

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        if (string.IsNullOrEmpty(_UseSkillInput))
            return;

        senderManager.ActSkill(senderManager._StateSkill._SkillMotions[_UseSkillInput]);
    }
    
}
