using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffAttackSub : ImpactBuffSub
{
    public int _Rate;

    public override void Attack()
    {
        if (!IsInCD())
        {
            if (!GameRandom.IsInRate(_Rate))
                return;

            SetCD();
            ActSubImpacts();
        }
    }

}
