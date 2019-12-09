using UnityEngine;
using System.Collections;

public class ImpactPushToTargetBackAct : ImpactBase
{
    public float _Distance = -1;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        Vector3 pos = reciverManager.transform.position + reciverManager.GetMotionForward() * _Distance;
        senderManager.SetPosition(pos);
        senderManager.SetLookAt(reciverManager.transform.position);
    }

}
