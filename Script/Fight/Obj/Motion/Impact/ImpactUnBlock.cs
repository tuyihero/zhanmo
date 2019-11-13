using UnityEngine;
using System.Collections;

public class ImpactUnBlock : ImpactBase
{
    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        reciverManager.RemoveBuff(typeof(ImpactBlock));
    }

}
