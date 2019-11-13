using UnityEngine;
using System.Collections;

public class AI_HeroDaJi : AI_IntHeroBase
{

    #region

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_TargetMotion == null)
            return;

        //float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        //if (distance > _AlertRange)
        //    return;

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (!IsCancelNormalAttack && _SelfMotion.ActingSkill != null)
            return;

        if (StartSkill())
            return;

        //specil:do not attack when target lie on floor
        if (_TargetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.RISE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.FLY_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.HIT_PRIOR)
            return;

        if (StartSkill())
            return;

        if (!IsActMove())
        {
            StartSkill();
        }
    }
    

    #endregion
}
