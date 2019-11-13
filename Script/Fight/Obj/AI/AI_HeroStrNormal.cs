using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeroStrNormal : AI_StrengthHeroBase
{
    protected override void Init()
    {
        base.Init();

    }

    #region

    private int _NextForceSkill;

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_TargetMotion == null)
            return;

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

    protected override bool StartSkill()
    {
        //if (_NextForceSkill > 0)
        //{
        //    StartSkill(_AISkills[_NextForceSkill]);
        //    _NextForceSkill = 0;
        //}

        //if (!IsRandomActSkill())
        //    return false;

        //float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        //for (int i = _AISkills.Count - 1; i >= 0; --i)
        //{
        //    if (_AISkills[i].SkillRange < dis)
        //        continue;

        //    if (!_AISkills[i].IsSkillCD())
        //        continue;

        //    if (!IsCommonCD())
        //        continue;

        //    {
        //        StartSkill(_AISkills[i]);
        //        return true;
        //    }
        //}

        return base.StartSkill();
    }

    #endregion
}

