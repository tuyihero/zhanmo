using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffDamageReduse : ImpactBuff
{

    public float _ReduseRate = 0.9f;
    
    public override void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        float damageRate = Mathf.Min(1 - _ReduseRate, 1);
        orgDamage.TotalDamageValue = (int)(orgDamage.TotalDamageValue * damageRate);
    }

}
