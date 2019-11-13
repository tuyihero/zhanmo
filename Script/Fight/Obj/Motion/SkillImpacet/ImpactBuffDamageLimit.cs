using UnityEngine;
using System.Collections;

public class ImpactBuffDamageLimit : ImpactBuff
{
    public float _LimitHPPersent = 0.33f;

    public override void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        int hpPersent = (int)(_BuffOwner.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax) * _LimitHPPersent);
        if (orgDamage.TotalDamageValue > hpPersent)
            orgDamage.TotalDamageValue = hpPersent;

        base.DamageModify(orgDamage, damageImpact);
    }
}
