using UnityEngine;
using System.Collections;

public class ImpactPushForward : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var moveTime = _Time / SkillMotion.SkillSpeed;
        Vector3 destMove = senderManager.GetMotionForward().normalized * _Speed * _Time * SkillMotion.SkillSpeed;
        reciverManager.SetMove(destMove, moveTime);
    }

}
