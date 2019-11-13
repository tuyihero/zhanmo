using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletDelay : BulletBase
{
    public float _DelayTime = 1;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        //Invoke("BulletFinish", 1);
    }

    void OnTriggerStay(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        if (!_IsBulletHitLie && targetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR)
            return;

        BulletHit(targetMotion);
        PlayHitAudio();
    }
}
