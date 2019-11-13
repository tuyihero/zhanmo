using UnityEngine;
using System.Collections;

public class ImpactFly : ImpactHit
{
    public float _FlyHeight = 0.6f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        HitFlyMotion(senderManager, reciverManager);

        ProcessDamge(senderManager, reciverManager);
    }

    protected virtual void HitFlyMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        reciverManager.FlyEvent(_FlyHeight, _HitEffect, _HitAudio,  senderManager, this, Vector3.zero, 0);
    }

    protected virtual void HitFlyMotion(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        if (senderManager.ActingSkill != null)
        {
            reciverManager.FlyEvent(_FlyHeight, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime / senderManager.ActingSkill.SkillSpeed);
        }
        else
        {
            reciverManager.FlyEvent(_FlyHeight, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime);
        }
    }

    protected virtual void HitFlyMotionWithoutSpeed(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        {
            reciverManager.FlyEvent(_FlyHeight, _HitEffect, _HitAudio, senderManager, this, moveDirect, moveTime);
        }
    }

}
