using UnityEngine;
using System.Collections;

public class ImpactFly : ImpactHit
{
    public float _UpSpeed = 5;
    public float _UpTime = 0.2f;
    public bool _IsBorder = true;

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

        reciverManager.FlyEvent(_UpTime, _HitEffect, _HitAudio,  senderManager, this, Vector3.zero, 0, _UpSpeed, _IsBorder);
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
            reciverManager.FlyEvent(_UpTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime / senderManager.ActingSkill.SkillSpeed, _UpSpeed, _IsBorder);
        }
        else
        {
            reciverManager.FlyEvent(_UpTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime, _UpSpeed, _IsBorder);
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
            reciverManager.FlyEvent(_UpTime, _HitEffect, _HitAudio, senderManager, this, moveDirect, moveTime, _UpSpeed);
        }
    }

}
