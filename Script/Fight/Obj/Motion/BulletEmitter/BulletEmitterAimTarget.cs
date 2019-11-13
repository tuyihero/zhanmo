using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterAimTarget : BulletEmitterBase
{

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        BulletDelay(reciverManager);
    }

    private void BulletDelay(MotionManager reciverManager)
    {

        var bullet = InitBulletGO<BulletBase>();
        var aiBase = SenderMotion.gameObject.GetComponent<AI_Base>();
        if (aiBase == null)
            return;
        if (aiBase._TargetMotion == null)
            return;

        var rotEuler = aiBase._TargetMotion.transform.position - SenderMotion.transform.position;
        Debug.Log("rotEuler:" + rotEuler);
        bullet.transform.rotation = Quaternion.LookRotation(rotEuler);
    }
}
