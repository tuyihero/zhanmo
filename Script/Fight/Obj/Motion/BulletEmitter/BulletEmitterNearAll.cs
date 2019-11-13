using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterNearAll : BulletEmitterElement
{
    public float _DelayTime;
    public float _Range;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        StartCoroutine(SendBulletDelay());
    }

    private IEnumerator SendBulletDelay()
    {
        yield return new WaitForSeconds(_DelayTime);

        var nearMotions = SelectTargetCommon.GetNearMotions(SenderMotion, _Range);

        foreach (var hitMotion in nearMotions)
        {
            if (SelectTargetCommon.IsTargetEnemy(SenderMotion, hitMotion))
            {
                var bullet = InitBulletGO<BulletBase>();
                bullet.transform.position = hitMotion.transform.position;
            }
        }
    }

}
