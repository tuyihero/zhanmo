using UnityEngine;
using System.Collections;

public class ImpactCatchFinish : ImpactBase
{

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        reciverManager.StopCatch();
    }

}
