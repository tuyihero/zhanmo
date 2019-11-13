using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffQiLinHitSub : ImpactBuffBeHitSub
{
    public int _ActTimes = 3;

    private EffectQiLinIceBuff _QiLinIceBuff;
    private int _ActedTimes;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        _ActedTimes = _ActTimes;
        if (_QiLinIceBuff != null)
        {
            _QiLinIceBuff.SetPlayEffectCnt(_ActTimes);
        }

        base.ActBuff(senderManager, reciverManager);

    }

    public override void HitAct(MotionManager hitSender, ImpactHit hitImpact)
    {
        base.HitAct(hitSender, hitImpact);

        ++_ActedTimes;
        if (_QiLinIceBuff == null && _DynamicEffect > 0 && ReciveMotion._DynamicEffects.ContainsKey(_DynamicEffect))
        {

            _QiLinIceBuff = ReciveMotion._DynamicEffects[_DynamicEffect].GetComponent<EffectQiLinIceBuff>();
        }
        if (_QiLinIceBuff != null)
        {
            _QiLinIceBuff.DecEffect();
        }

        if (_ActedTimes == 0)
        {
            _BuffOwner.RemoveBuff(this);
        }
    }

}
