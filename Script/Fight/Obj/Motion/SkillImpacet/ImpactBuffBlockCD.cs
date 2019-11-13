using UnityEngine;
using System.Collections;

public class ImpactBuffBlockCD : ImpactBuffCD
{
    

    public override bool IsBuffCanCatch(ImpactCatch damageImpact)
    {
        if (IsInCD())
            return true;
        else
        {

            return false;
        }
    }

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit damageImpact)
    {
        if (IsInCD())
        {
            return true;
        }
        else
        {
            
            return false;
        }
    }

    public override void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        if (IsInCD())
        {
            base.DamageModify(orgDamage, damageImpact);
        }
        else
        {
            ActEffect();
            //ReciveMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_ActAudio]);
            orgDamage.TotalDamageValue = 0;
            SetCD();
        }

    }

    #region act effect

    public int _ActAudio = 0;
    public EffectController _ActEffect;
    public int _DynamicActEffect;

    private void ActEffect()
    {
        _ActEffect._EffectLastTime = 1.0f;
        if (_ActEffect != null)
        {
            _DynamicActEffect = _BuffOwner.PlayDynamicEffect(_ActEffect);
        }

        _BuffOwner.PlayAudio(ResourcePool.Instance._CommonAudio[_ActAudio]);
    }

    #endregion
}
