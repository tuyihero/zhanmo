using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffBeHitSub : ImpactBuffSub
{
    //public float _Interval = 1;

    private CapsuleCollider _Collider;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        if (_SubToSender != null)
        {
            _SubImpactsToSender = new List<ImpactBase>(_SubToSender.GetComponentsInChildren<ImpactBase>());
        }
    }

    public override void BeHit(MotionManager hitSender, ImpactHit hitImpact)
    {
        if (IsInCD())
            return;

        SetCD();
        HitAct(hitSender, hitImpact);
    }

    public virtual void HitAct(MotionManager hitSender, ImpactHit hitImpact)
    {
        ActSubImpacts(_BuffSender, _BuffOwner);

        if (_SubImpactsToSender != null)
        {
            foreach (var subImpact in _SubImpactsToSender)
            {
                subImpact.ActImpact(_BuffOwner, hitSender);
            }
        }
    }

    #region sub to sender

    public GameObject _SubToSender;

    protected List<ImpactBase> _SubImpactsToSender;


    #endregion
}
