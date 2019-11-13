using UnityEngine;
using System.Collections;

public class AI_HeroWuZeTian : AI_IntHeroBase
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

        if (StartSkill())
            return;

        if (!IsActMove())
        {
            StartSkill();
        }
    }

    protected override bool StartSkill()
    {

        //if (!IsRandomActSkill())
        //    return false;

        //float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        //for (int i = _AISkills.Count - 1; i >= 0; --i)
        //{
        //    if (!_AISkills[i].IsSkillCD())
        //        continue;

        //    if (!IsCommonCD())
        //        continue;

        //    if (_AISkills[i].SkillRange < dis)
        //        continue;

        //    StartSkill(_AISkills[i]);
        //    return true;

        //}

        return base.StartSkill();
    }

    #endregion
}
