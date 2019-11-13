using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_EliteHeroDianWei : AI_HeroStrNormal
{
    protected override void Init()
    {
        base.Init();

    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        UpdateSkill();
    }

    #region 

    public int _ControlSkillIdx = 1;
    public int _DamageSkillIdx = 2;

    private bool _ActDamageSkill = false;

    protected override void StartSkill(AI_Skill_Info skillInfo, bool isIgnoreCD = false)
    {
        base.StartSkill(skillInfo, isIgnoreCD);

        var idx = _AISkills.IndexOf(skillInfo);
        if (idx == _ControlSkillIdx)
        {
            _ActDamageSkill = true;
        }
    }

    private void UpdateSkill()
    {
        if (!_ActDamageSkill)
            return;

        if (_SelfMotion._ActionState == _SelfMotion._StateIdle
            || _SelfMotion._ActionState == _SelfMotion._StateMove)
        {
            StartSkill(_AISkills[_DamageSkillIdx], true);
            _ActDamageSkill = false;
        }

        
    }

    #endregion

}

