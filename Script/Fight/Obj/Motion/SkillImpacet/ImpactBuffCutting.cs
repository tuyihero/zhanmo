using UnityEngine;
using System.Collections;

public class ImpactBuffCutting : ImpactBuff
{

    public int _Rate;
    public float _DmgRate;

    public override void CastDamage(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        base.CastDamage(orgDamage, damageImpact);

        if (!GameRandom.IsInRate(_Rate))
            return;

        orgDamage.AttachDamageValue += (int)(orgDamage.TotalDamageValue * _DmgRate);

    }


}
