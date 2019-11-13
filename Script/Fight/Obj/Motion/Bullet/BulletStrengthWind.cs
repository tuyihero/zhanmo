using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletStrengthWind : BulletLineHitInterval
{
    private CapsuleCollider _CapsuleCollider;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _CapsuleCollider = gameObject.GetComponent<CapsuleCollider>();
    }

   
}
