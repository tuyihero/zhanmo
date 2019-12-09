using UnityEngine;
using System.Collections;

public class ImpactFlyBack : ImpactFly
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        Vector3 destMove = -senderManager.GetMotionForward().normalized * _Speed * _Time;

        HitFlyMotion(senderManager, reciverManager, destMove, _Time);

        ProcessDamge(senderManager, reciverManager);

    }

}
