using UnityEngine;
using System.Collections;

public class ImpactHitPauseFly : ImpactHit
{

    protected override void HitMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        //reciverManager.BaseMotionManager.HitEvent(_HitTime, _HitEffect, senderManager, this);
        reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, Vector3.zero, 0, false);
    }

    protected override void HitMotion(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        //reciverManager.BaseMotionManager.HitEvent(_HitTime, _HitEffect, senderManager, this);
        if (senderManager.ActingSkill != null)
        {
            reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime / senderManager.ActingSkill.SkillSpeed, false);
        }
        else
        {
            reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime, false);
        }
    }

    protected override void HitMotionWithoutSpeed(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {

        {
            reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime, false);
        }
    }
}
