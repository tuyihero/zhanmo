using UnityEngine;
using System.Collections;

public class BulletEmitterTargetPosSingle : BulletEmitterBase
{
    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var bullet = InitBulletGO<BulletBase>();

        bullet.transform.position = reciverManager.transform.position;
    }
}
