using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffDefence : ImpactBuffSub
{

    public float _DefenceRate = 0.5f;
    public float _DefenceHitTime = 0.1f;
    public float _DefenceHitSpeed = 5.0f;
    public float _DefenceAngle = 45;
    public bool _IsDefenceDebuff;
    
    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        
    }

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit damageImpact)
    {
        float targetAngle = Mathf.Abs(Vector3.Angle(damageImpact.SenderMotion.transform.position - _BuffOwner.transform.position, _BuffOwner.transform.forward));
        //if (targetAngle > _DefenceAngle)
        //{
        //    return true;
        //}
        if (targetAngle > 60)
        {
            _BuffOwner.SetLookAt(damageImpact.SenderMotion.transform.position);
        }
        _BuffOwner.SetMove(-_BuffOwner.transform.forward * _DefenceHitSpeed, _DefenceHitTime);
        if (!IsInCD())
        {
            ActSubImpacts();
        }
        return false;
    }

    public override bool IsCanAddBuff(ImpactBuff newBuff)
    {
        if(_IsDefenceDebuff)
            return newBuff._BuffType != BuffType.Debuff;

        return true;
    }

    public override void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        orgDamage.TotalDamageValue = (int)(orgDamage.TotalDamageValue * (1 - _DefenceRate));
    }

}
