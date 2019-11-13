using UnityEngine;
using System.Collections;

public class ImpactBuffRelive : ImpactBuffSub
{

    public override bool IsBuffCanDie()
    {
        if (IsInCD())
        {
            return true;
        }
        else
        {
            SetCD();
            ActSubImpacts();
            return false;
        }
    }
}
