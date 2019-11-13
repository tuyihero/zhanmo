using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ImpactBuffDodge : ImpactBuff
{

    private ObstacleAvoidanceType _OrgAvoidType;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        _LastTime = SkillMotion._NextAnim[0].length * SkillMotion.SkillActSpeed;

        reciverManager.SetMotionNoDamage();

        base.ActBuff(senderManager, reciverManager);
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        reciverManager.ResetMotionNoDamage();
    }
}
