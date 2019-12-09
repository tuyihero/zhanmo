using UnityEngine;
using System.Collections;

public class ImpactPushDown : ImpactBase
{
    public float _Time = 0.6f;
    public Vector2 _Speed = Vector2.zero;
    public string _JumpFinishActSkill = "";

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        reciverManager.SetSkillJump(_Speed, _Time, JumpFinish);
    }

    private void JumpFinish()
    {
        if (string.IsNullOrEmpty(_JumpFinishActSkill))
            return;

        SenderMotion.ActSkill(SenderMotion._StateSkill._SkillMotions[_JumpFinishActSkill]);
    }

}
