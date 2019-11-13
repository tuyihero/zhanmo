using UnityEngine;
using System.Collections;

public class AI_EliteBase : AI_HeroBase
{

    protected override void Init()
    {
        base.Init();
        InitSkills();
    }

    protected override void AIUpdate()
    {
        //base.UpdateCriticalAI();
        base.AIUpdate();

        if (_TargetMotion == null)
            return;

        if (!_AIAwake)
        {
            //float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
            float distance = GetPathLength(transform.position, _TargetMotion.transform.position);
            if (distance > _AlertRange)
                return;

            _AIAwake = true;
            AIManager.Instance.GroupAwake(GroupID);
        }

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (!IsCancelNormalAttack && _SelfMotion.ActingSkill != null)
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

    protected override bool StartSkill()
    {
        if (_TargetMotion == null)
            return false;

        return base.StartSkill();
    }

    #region super armor


    #endregion


    
}

