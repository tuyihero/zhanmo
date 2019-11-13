using UnityEngine;
using System.Collections;

public class ImpactBuffBlock : ImpactBuff
{
    public EffectController _HitEffect;
    public bool _IsBlockBullet;
    public bool _IsBlockNotBullet;
    

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);
    }

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit impactHit)
    {
        if (impactHit == null)
            return true;

        if (impactHit._IsBulletHit && _IsBlockBullet)
            return false;

        if (!impactHit._IsBulletHit && _IsBlockNotBullet)
            return false;

        return true;
    }

    public override void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        if (damageImpact == null)
            return;

        var hitImpact = damageImpact as ImpactHit;
        if(hitImpact == null)
            return;

        if (hitImpact._IsBulletHit && _IsBlockBullet)
        {
            orgDamage.TotalDamageValue = 0;
            return;
        }

        if (!hitImpact._IsBulletHit && _IsBlockNotBullet)
        {
            orgDamage.TotalDamageValue = 0;
            return;
        }
    }
}
