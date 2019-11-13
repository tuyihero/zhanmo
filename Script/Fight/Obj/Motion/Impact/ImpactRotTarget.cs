using UnityEngine;
using System.Collections;

public class ImpactRotTarget : ImpactBase
{

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        senderManager.transform.LookAt(reciverManager.transform);
    }

}
