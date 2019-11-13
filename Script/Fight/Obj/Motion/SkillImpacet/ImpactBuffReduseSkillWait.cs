using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffReduseSkillWait : ImpactBuff
{
    public float _SkillWaitRedusePersent;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        var aiBase = _BuffOwner.GetComponent<AI_Base>();
        for (int i = 0; i < aiBase._AISkills.Count; ++i)
        {
            aiBase._AISkills[i].AfterSkillWait *= _SkillWaitRedusePersent;
        }
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        var aiBase = _BuffOwner.GetComponent<AI_Base>();
        for (int i = 0; i < aiBase._AISkills.Count; ++i)
        {
            aiBase._AISkills[i].AfterSkillWait /= _SkillWaitRedusePersent;
        }
    }
}
