using UnityEngine;
using System.Collections;

public class ImpactHitFar : ImpactHitForward
{
    public float _MaxDis = 2;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        Vector3 direct = reciverManager.transform.position - senderManager.transform.position;
        float length = direct.magnitude - _MaxDis;
        if (length > 0)
            return;

        Vector3 destMove = senderManager.GetMotionForward().normalized * _Speed * _Time;

        HitMotion(senderManager, reciverManager, destMove, _Time);

        ProcessDamge(senderManager, reciverManager);
    }

}
