using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletLineBoom : BulletBase
{
    public float _LifeTime = 2.0f;
    public float _Speed = 10;
    public GameObject _BoomEffect;

    private List<ImpactBase>  _BoomImpacts;

    private float _AwakeTime = 0;
    private int _AlreadyHitTimes = 0;
    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _AlreadyHitTimes = 0;
        _AwakeTime = Time.time;

        _BoomEffect.gameObject.SetActive(false);
        _BoomImpacts = new List<ImpactBase>( _BoomEffect.GetComponents<ImpactBase>());
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position += transform.forward.normalized * _Speed * Time.fixedDeltaTime;

        if (Time.time - _AwakeTime > _LifeTime)
        {
            Boom(null);
        }
    }

    void OnTriggerStay(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        Debug.Log("OnTriggerEnter:" + targetMotion.ToString());
        if (!_IsBulletHitLie && (targetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR || targetMotion.MotionPrior == BaseMotionManager.RISE_PRIOR))
            return;

        BulletHit(targetMotion);
        ++_AlreadyHitTimes;

        Boom(targetMotion);
    }

    void Boom(MotionManager target)
    {
        foreach (var impact in _BoomImpacts)
        {
            impact.ActImpact(_SkillMotion, target);
        }
        BulletFinish();
    }

}
