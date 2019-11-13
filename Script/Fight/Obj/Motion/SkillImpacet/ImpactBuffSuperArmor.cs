using UnityEngine;
using System.Collections;

public class ImpactBuffSuperArmor : ImpactBuff
{
    public EffectController _BuffSkillEffect;
    public EffectController _HitEffect;
    public float _BlockTime = 0.0f;

    private EffectOutLine _DynamicBuffEffect;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        if (_BuffOwner._ActionState == _BuffOwner._StateCatch
            || _BuffOwner._ActionState == _BuffOwner._StateHit)
        {
            _BuffOwner.TryEnterState(_BuffOwner._StateIdle);
        }
        if (_BuffSkillEffect != null)
        {
            _BuffOwner.PlaySkillEffect(_BuffSkillEffect);
        }
        _BuffEffect = null;
        base.ActBuff(senderManager, reciverManager);
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        _BuffOwner.StopSkillEffect(_BuffSkillEffect);
        base.RemoveBuff(reciverManager);
    }

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit impactHit)
    {
        //GlobalEffect.Instance.Pause(0.1f);

        if (impactHit._ForceHit)
            return true;

        if (_BlockTime > 0)
        {
            _BuffOwner.ResetMove();
            _BuffOwner.ActionPause(_BlockTime);

            if (!impactHit._IsBulletHit)
            {
                impactSender.ActionPause(_BlockTime);
            }
        }

        if (_HitEffect != null)
            _BuffOwner.PlaySkillEffect(_HitEffect);

        if (_DynamicBuffEffect == null && _DynamicEffect > 0 && ReciveMotion._DynamicEffects.ContainsKey(_DynamicEffect))
        {

            _DynamicBuffEffect = ReciveMotion._DynamicEffects[_DynamicEffect].GetComponent<EffectOutLine>();
        }
        if (_DynamicBuffEffect != null)
        {
            _DynamicBuffEffect.PlayHitted();
        }

        Hashtable hash = new Hashtable();
        hash.Add("Motion", _SenderMotion);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_SOMEONE_SUPER_ARMOR, this, hash);

        return false;
    }
}
