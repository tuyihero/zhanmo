using UnityEngine;
using System.Collections;

public class AI_EliteHeroJingKe : AI_HeroDexNormal
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        BeHitSkillUpdate();
    }

    #region behit skill

    public int _BeHitSkillIdx;

    private void BeHitSkillUpdate()
    {
        if (_SelfMotion._ActionState != _SelfMotion._StateHit
            && _SelfMotion._ActionState != _SelfMotion._StateFly)
            return;

        if (!_AISkills[_BeHitSkillIdx].IsSkillCD())
            return;

        StartSkill(_AISkills[_BeHitSkillIdx]);
        _AISkills[2].LastUseSkillTime = 0;
    }

    #endregion
}
