using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffShadowHit : ImpactBuff
{
    public EffectAfterAnim _EffectAfterAnim;
    public float _HitDuration = 0.1f;
    public int _ShadowCnt = 1;
    public float _DamageRate = 0.1f;
    public float _HitPause = 0.05f;

    private ImpactBuff _SubBuffInstance;

    public override void ActBuff(MotionManager senderManager, MotionManager ownerManager)
    {
        base.ActBuff(senderManager, ownerManager);

        _EffectAfterAnim._Duration = _LastTime;
        _EffectAfterAnim._Interval = _HitDuration;
        _EffectAfterAnim._FadeOut = _HitDuration * _ShadowCnt;

        _DynamicEffect = ownerManager.PlayDynamicEffect(_EffectAfterAnim);
    }

    public override void UpdateBuff()
    {
        base.UpdateBuff();


    }

    public override void HitEnemy(ImpactHit hitImpact, List<MotionManager> hittedMotions)
    {
        base.HitEnemy();

        StartCoroutine(HitDelay(hitImpact, hittedMotions));

    }

    int _HittedCnt = 0;
    public IEnumerator HitDelay(ImpactHit hitImpact, List<MotionManager> hittedMotions)
    {
        _HittedCnt = _ShadowCnt;
        while (_HittedCnt > 0)
        {
            --_HittedCnt;
            HitTarget(hitImpact, hittedMotions);
            GlobalEffect.Instance.Pause(_HitPause);
            yield return null;
        }

    }

    public void HitTarget(ImpactHit hitImpact, List<MotionManager> hittedMotions)
    {
        for (int i = 0; i < hittedMotions.Count; ++i)
        {
            hittedMotions[0].HitEvent(hitImpact._HitTime, hitImpact._HitEffect, hitImpact._HitAudio, _BuffOwner, hitImpact, Vector3.zero, 0);
            _BuffOwner.RoleAttrManager.SendDamageEvent(hittedMotions[0], hitImpact._DamageRate * _DamageRate, hitImpact._DamageType, hitImpact);
        }
    }

}
