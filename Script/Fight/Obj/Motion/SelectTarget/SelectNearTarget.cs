using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectNearTarget : SelectBase
{
    public float _SelectRange = 8;

    public override void ColliderStart()
    {
        //base.ColliderStart();
        Debug.Log("ColliderStart:" + _ColliderID);
        if (_ObjMotion == null)
            return;

        var selectTarget = SelectTargetCommon.GetNearMotion(_ObjMotion, transform.position, null, SelectTargetType.Enemy);

        Debug.Log("selectTarget:" + selectTarget.name);
        foreach (var impact in _ImpactList)
        {
            impact.ActImpact(_ObjMotion, selectTarget);
        }

    }
}
