using UnityEngine;
using System.Collections;

public class ImpactFlyAway : ImpactFly
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        Vector3 destMove = (reciverManager.transform.position - senderManager.transform.position).normalized * _Speed * _Time;

        HitFlyMotion(senderManager, reciverManager, destMove, _Time);

        ProcessDamge(senderManager, reciverManager);
    }

}
