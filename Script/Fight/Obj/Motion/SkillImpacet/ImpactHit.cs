using UnityEngine;
using System.Collections;

public class ImpactHit : ImpactDamage
{
    public float _HitTime = 0.6f;
    public int _HitEffect = 0;
    public int _HitAudio = -1;
    public bool _IsBulletHit = false;
    public bool _ForceHit = false;
    public float _HitPauseTime = 0;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        //base.ActImpact(senderManager, reciverManager);
        InitActImpact(senderManager, reciverManager);

        HitMotion(senderManager, reciverManager);

        ProcessDamge(senderManager, reciverManager);
    }

    protected virtual void InitActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        _SenderMotion = senderManager;
        _ReciveMotion = reciverManager;
        _IsActingImpact = true;
    }

    protected virtual void HitMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        if (_HitPauseTime > 0)
        {
            senderManager.ActionPause(_HitPauseTime);
            reciverManager.ActionPause(_HitPauseTime);
        }
        //reciverManager.BaseMotionManager.HitEvent(_HitTime, _HitEffect, senderManager, this);
        reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, Vector3.zero, 0);
    }

    protected virtual void HitMotion(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        if (_HitPauseTime > 0)
        {
            senderManager.ActionPause(_HitPauseTime);
            reciverManager.ActionPause(_HitPauseTime);
        }
        //reciverManager.BaseMotionManager.HitEvent(_HitTime, _HitEffect, senderManager, this);
        if (senderManager.ActingSkill != null)
        {
            reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime / senderManager.ActingSkill.SkillSpeed);
        }
        else
        {
            reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime);
        }
    }

    protected virtual void HitMotionWithoutSpeed(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        if (_HitPauseTime > 0)
        {
            senderManager.ActionPause(_HitPauseTime);
            reciverManager.ActionPause(_HitPauseTime);
        }

        {
            reciverManager.HitEvent(_HitTime, _HitEffect, _HitAudio,  senderManager, this, moveDirect, moveTime);
        }
    }
}
