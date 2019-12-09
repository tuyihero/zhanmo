using UnityEngine;
using System.Collections;

public class ImpactFly : ImpactHit
{
    public float _FlyHeight = 0.6f;
    public float _UpSpeed = 0;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        HitFlyMotion(senderManager, reciverManager);

        ProcessDamge(senderManager, reciverManager);
    }

    protected virtual void HitFlyMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        if (_HitPauseTime > 0)
        {
            senderManager.ActionPause(_HitPauseTime);
            reciverManager.ActionPause(_HitPauseTime);
        }

        reciverManager.FlyEvent(_FlyHeight + reciverManager.JumpBody.localPosition.y, _HitEffect, _HitAudio,  senderManager, this, Vector3.zero, 0, 5);
    }

    protected virtual void HitFlyMotion(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        if (_HitPauseTime > 0)
        {
            senderManager.ActionPause(_HitPauseTime);
            reciverManager.ActionPause(_HitPauseTime);
        }

        if (senderManager.ActingSkill != null)
        {
            reciverManager.FlyEvent(_FlyHeight + reciverManager.JumpBody.localPosition.y, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime / senderManager.ActingSkill.SkillSpeed, _UpSpeed);
        }
        else
        {
            reciverManager.FlyEvent(_FlyHeight + reciverManager.JumpBody.localPosition.y, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime, _UpSpeed);
        }
    }

    protected virtual void HitFlyMotionWithoutSpeed(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        if (_HitPauseTime > 0)
        {
            senderManager.ActionPause(_HitPauseTime);
            reciverManager.ActionPause(_HitPauseTime);
        }

        {
            reciverManager.FlyEvent(_FlyHeight + reciverManager.JumpBody.localPosition.y, _HitEffect, _HitAudio, senderManager, this, moveDirect, moveTime, _UpSpeed);
        }
    }

}
