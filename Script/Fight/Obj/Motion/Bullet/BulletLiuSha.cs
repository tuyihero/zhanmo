using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletLiuSha : BulletBase
{
    public float _LastTime = 10;
    public float _DamageInterval = 0.1f;

    private ImpactDamage _ImpactDamage;
    private float _LastDamageTime;

    private List<MotionManager> _EffectMotions = new List<MotionManager>();

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion,emitterBase);

        _ImpactDamage = gameObject.GetComponent<ImpactDamage>();

        StartCoroutine(StartHit());
    }

    protected override void BulletFinish()
    {
        base.BulletFinish();

        DisableAll();
    }

    IEnumerator StartHit()
    {
        yield return new WaitForSeconds(_LastTime);

        BulletFinish();
    }
    
    void OnTriggerEnter(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        BulletHit(targetMotion);
        _LastDamageTime = Time.time;
    }

    void OnTriggerStay(Collider other)
    {
        if (Time.time - _LastDamageTime < _DamageInterval)
            return;

        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        _LastDamageTime = Time.time;
        if (_ImpactDamage != null)
        {
            _ImpactDamage.ActImpact(_SkillMotion, targetMotion);
        }
        if (!_EffectMotions.Contains(targetMotion))
        {
            _EffectMotions.Add(targetMotion);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        foreach (var impact in _ImpactList)
        {
            impact.FinishImpact(targetMotion);
        }
    }

    private void DisableAll()
    {
        foreach (var effectMotion in _EffectMotions)
        {
            if (!effectMotion.IsMotionDie)
            {
                foreach (var impact in _ImpactList)
                {
                    impact.FinishImpact(effectMotion);
                }
            }
        }
        _EffectMotions.Clear();
    }

}
