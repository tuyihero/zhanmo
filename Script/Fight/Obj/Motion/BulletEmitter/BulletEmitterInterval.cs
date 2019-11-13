using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterInterval : BulletEmitterElement
{
    public float _Interval = 0.5f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        StartCoroutine(BulletInterval());
    }

    protected IEnumerator BulletInterval()
    {
        yield return new WaitForSeconds(_Interval);

        var bullet = InitBulletGO<BulletBase>();
    }
}
