using UnityEngine;
using System.Collections;

public class ImpactBuffBloodSucking : ImpactBuffSub
{

    public int _Rate;
    public float _SuckingRate;

    public override void CastDamage(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        base.CastDamage(orgDamage, damageImpact);

        if (!IsInCD())
        {
            if (!GameRandom.IsInRate(_Rate))
                return;

            for (int i = 0; i < _SubImpacts.Count; ++i)
            {
                if (_SubImpacts[i] is ImpactResumeHP)
                {
                    (_SubImpacts[i] as ImpactResumeHP)._HPValue = (int)(orgDamage.TotalDamageValue * _SuckingRate);
                }
            }
            ActSubImpacts();
            SetCD();
        }
    }


}
