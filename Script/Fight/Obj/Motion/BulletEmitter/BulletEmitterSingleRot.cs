using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterSingleRot : BulletEmitterElement
{
    public Vector3 _EmitterRotOffset;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var bullet = InitBulletGO<BulletBase>();
        bullet.transform.rotation = Quaternion.Euler(bullet.transform.rotation.eulerAngles + _EmitterRotOffset);
    }
    
}
