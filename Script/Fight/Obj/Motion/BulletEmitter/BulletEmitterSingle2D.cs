using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterSingle2D : BulletEmitterElement
{
    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var bullet = InitBulletGO2D<BulletBase>();
    }
    
}
