using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffHpLowSub : ImpactBuffSub
{

    public override void UpdateBuff()
    {
        base.UpdateBuff();

        if (_BuffOwner.RoleAttrManager.HPPersent <= _HPRate && !IsInCD())
        {
            SetCD();
            ActSubImpacts();
        }
    }

    #region hp rate

    public float _HPRate = 0.5f;

    #endregion

}
