using UnityEngine;
using System.Collections;

public class AI_EliteHeroBaiQi : AI_HeroStrNormal
{

    protected override void AIUpdate()
    {
        base.AIUpdate();

        SkillUpdate();
    }

    #region skill move

    private ImpactBuffBeHitSub _PassiveBuff;

    private void SkillUpdate()
    {
        if (_PassiveBuff == null)
        {
            _PassiveBuff = _SelfMotion.GetComponentInChildren<ImpactBuffBeHitSub>();
        }

        if (_PassiveBuff == null)
            return;

        _SelfMotion.SkillProcessing = _PassiveBuff.GetCDProcess();
    }

    #endregion
}
