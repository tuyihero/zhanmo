using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBulletEmitter : SelectBase
{

    public override void ColliderStart()
    {
        //base.ColliderStart();

        if (_ObjMotion != null)
        {
            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_ObjMotion, _ObjMotion);
            }
        }
    }

    public override void ColliderFinish()
    {
        //base.ColliderFinish();
    }
}
