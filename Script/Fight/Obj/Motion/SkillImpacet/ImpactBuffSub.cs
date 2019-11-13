using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffSub : ImpactBuffCD
{
    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        if (_SubImpactGO != null)
        {
            _SubImpacts = new List<ImpactBase>(_SubImpactGO.GetComponentsInChildren<ImpactBase>());
            //foreach (var subImpact in _SubImpacts)
            //{
            //    if (subImpact is ImpactDamage)
            //    {
            //        (subImpact as ImpactDamage)._DamageRate *= _DamageRate;
            //    }
            //    else if (subImpact is BulletEmitterBase)
            //    {
            //        (subImpact as BulletEmitterBase)._Damage *= _DamageRate;
            //    }

            //}
        }
    }

    #region sub impact

    public GameObject _SubImpactGO;
    public float _DamageRate;

    protected List<ImpactBase> _SubImpacts;

    protected void ActSubImpacts()
    {
        foreach (var subImpact in _SubImpacts)
        {
            subImpact.ActImpact(_BuffSender, _BuffOwner);
        }
    }

    protected void ActSubImpacts(MotionManager sender, MotionManager reciver)
    {
        if (_SubImpacts == null)
            return;

        foreach (var subImpact in _SubImpacts)
        {
            subImpact.ActImpact(sender, reciver);
        }
    }
    #endregion
}
