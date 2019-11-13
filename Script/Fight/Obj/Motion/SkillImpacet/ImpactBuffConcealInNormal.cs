using UnityEngine;
using System.Collections;

public class ImpactBuffConcealInNormal : ImpactBuff
{
    #region 

    public EffectController _ConcealActEffect;

    private EffectController _DynamicConcealEffect;
    #endregion

    public override void ActBuff(MotionManager senderManager, MotionManager ownerManager)
    {
        base.ActBuff(senderManager, ownerManager);
        if (_ConcealActEffect != null)
        {
            _DynamicConcealEffect = ownerManager.PlaySkillEffect(_ConcealActEffect);
        }
    }

    public override void RemoveBuff(MotionManager ownerManager)
    {
        base.RemoveBuff(ownerManager);

        _DynamicConcealEffect.HideEffect();
    }

    public override void UpdateBuff()
    {
        base.UpdateBuff();

        if (_BuffOwner._ActionState == _BuffOwner._StateIdle
            || _BuffOwner._ActionState == _BuffOwner._StateMove)
        {
            _DynamicConcealEffect.PlayEffect();
        }
        else
        {
            _DynamicConcealEffect.HideEffect();
        }
    }

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit damageImpact)
    {
        if(_BuffOwner._ActionState == _BuffOwner._StateIdle
            || _BuffOwner._ActionState == _BuffOwner._StateMove)
        return false;

        return true;
    }

    public override bool IsBuffCanCatch(ImpactCatch damageImpact)
    {
        if (_BuffOwner._ActionState == _BuffOwner._StateIdle
            || _BuffOwner._ActionState == _BuffOwner._StateMove)
            return false;

        return true;
    }

    public override void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        if (_BuffOwner._ActionState == _BuffOwner._StateIdle
            || _BuffOwner._ActionState == _BuffOwner._StateMove)
            orgDamage.TotalDamageValue = 0;
    }
}
