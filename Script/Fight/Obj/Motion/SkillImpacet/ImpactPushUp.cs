using UnityEngine;
using System.Collections;

public class ImpactPushUp : ImpactBase
{
    public float _Time = 0.6f;
    public Vector2 _Speed = Vector2.zero;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        reciverManager.SetSkillJump(_Speed, _Time);
    }

}
