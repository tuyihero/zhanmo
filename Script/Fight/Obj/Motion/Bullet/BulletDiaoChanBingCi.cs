using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletDiaoChanBingCi : BulletBase
{
    //public Vector3 _StartSpeed;
    public GameObject _EffectAlert;
    public GameObject _SubEffect;
    public Collider _Trigger;
    public float _AlertTime = 0.6f;
    public float _ExplodeTime = 1f;
    public float _LifeTime = 1f;
    public GameObject _HitImpactObj;
    public GameObject _ExplodeImpactObj;

    private ImpactBase[] _HitImpacts;
    private ImpactBase[] _ExplodeImpacts;
    private bool _IsExploreStart = false;
    private bool _IsHitStart = false;

    private List<MotionManager> _HitMotions = new List<MotionManager>();

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _EffectAlert.SetActive(true);
        _SubEffect.SetActive(false);
        _Trigger.enabled = false;
        _IsExploreStart = false;
        _IsHitStart = false;

        _HitImpacts = _HitImpactObj.GetComponents<ImpactBase>();
        _ExplodeImpacts = _ExplodeImpactObj.GetComponents<ImpactBase>();
        StartCoroutine(StartHit());
    }

    public IEnumerator StartHit()
    {
        yield return new WaitForSeconds(_AlertTime);

        _EffectAlert.SetActive(false);
        _SubEffect.SetActive(true);
        _Trigger.enabled = true;
        _IsHitStart = true;

        yield return new WaitForFixedUpdate();

        _IsHitStart = false;
        _Trigger.enabled = false;

        yield return new WaitForSeconds(_ExplodeTime);

        _Trigger.enabled = true;
        _IsExploreStart = true;

        yield return new WaitForFixedUpdate();

        _IsExploreStart = false;
        _Trigger.enabled = false;

        yield return new WaitForSeconds(_LifeTime);

        BulletFinish();
    }

    protected override void BulletFinish()
    {
        base.BulletFinish();
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        if (_IsHitStart)
        {
            foreach (var impact in _HitImpacts)
            {
                impact.ActImpact(_SkillMotion, targetMotion);
            }
        }

        if (_IsExploreStart)
        {
            foreach (var impact in _ExplodeImpacts)
            {
                impact.ActImpact(_SkillMotion, targetMotion);
            }
        }
    }

}
