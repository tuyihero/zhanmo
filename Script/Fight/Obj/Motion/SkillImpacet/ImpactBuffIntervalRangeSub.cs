using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffIntervalRangeSub : ImpactBuffSub
{
    public float _Interval = 1;

    protected CapsuleCollider _Collider;
    public float _Range = 1.5f;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _Collider = gameObject.AddComponent<CapsuleCollider>();
        _Collider.direction = 1;
        _Collider.radius = _Range;
        _Collider.height = 20;
        _Collider.center = new Vector3(0, 1, 0);
        _Collider.enabled = false;
        _Collider.isTrigger = true;
        gameObject.layer = FightLayerCommon.GetBulletLayer(senderManager);

        _SubImpacts = new List<ImpactBase>(_SubImpactGO.GetComponentsInChildren<ImpactBase>());
        StartCoroutine(Interval());
    }

    private IEnumerator Interval()
    {
        yield return new WaitForSeconds(_Interval);

        _Collider.enabled = true;
        yield return new WaitForFixedUpdate();
        _Collider.enabled = false;

        StartCoroutine(Interval());
    }

    void OnTriggerEnter(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;
        if (targetMotion.IsMotionDie)
            return;

        ActSubImpacts(_BuffSender, targetMotion);
    }

}
