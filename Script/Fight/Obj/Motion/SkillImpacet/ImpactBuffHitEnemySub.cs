using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffHitEnemySub : ImpactBuffSub
{
    public int _Rate;
    public bool _IsNeedTarget = false;
    
    public override void HitEnemy()
    {
        if (_IsNeedTarget)
            return;

        if (!IsInCD())
        {
            if (!GameRandom.IsInRate(_Rate))
                return;

            ActSubImpacts();
            SetCD();
        }
    }

    public override void HitEnemy(ImpactHit hitImpact, List<MotionManager> hittedMotions)
    {
        if (!_IsNeedTarget)
            return;

        if (!IsInCD())
        {
            if (!GameRandom.IsInRate(_Rate))
                return;

            int randomIdx = Random.Range(0, hittedMotions.Count);
            ActSubImpacts(_BuffOwner, hittedMotions[randomIdx]);
            SetCD();
        }
    }

}
