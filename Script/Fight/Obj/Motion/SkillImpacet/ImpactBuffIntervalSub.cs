using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffIntervalSub : ImpactBuffSub
{
    public float _Interval = 1;

    protected CapsuleCollider _Collider;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _SubImpacts = new List<ImpactBase>(_SubImpactGO.GetComponentsInChildren<ImpactBase>());
        StartCoroutine(Interval());
    }

    private IEnumerator Interval()
    {
        yield return new WaitForSeconds(_Interval);

        SendImpactInterval();

        StartCoroutine(Interval());
    }

    protected virtual void SendImpactInterval()
    {
        ActSubImpacts();
    }
    
}
