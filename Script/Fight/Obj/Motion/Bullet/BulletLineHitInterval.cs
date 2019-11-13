using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletLineHitInterval : BulletBase
{
    //public Vector3 _StartSpeed;
    public float _MoveSpeed = 4;
    public float _HitInterval = 0.1f;
    public float _StayTime = 10;
    public GameObject _SubImpact;

    private float _LiftTime = 0;
    private float _NextSpeed = 0;
    private float _LastHitTime = 0;
    private Collider _Collider;

    private List<ImpactBase> _ImpactBase;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _Collider = gameObject.GetComponent<Collider>();
        _NextSpeed = _MoveSpeed;
        _LiftTime = _StayTime;

        if (_BornAudio > 0)
        {
            _SkillMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_BornAudio]);
        }
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * _NextSpeed * Time.fixedDeltaTime;

        if (_LastHitTime + _HitInterval < Time.time)
        {
            if (_Collider != null)
            {
                StartCoroutine(CalculateHit());
            }
            ActSubImpacts();
            _LastHitTime = Time.time;
        }

        _LiftTime -= Time.fixedDeltaTime;
        if (_LiftTime < 0)
        {
            BulletFinish();
        }
    }

    protected IEnumerator CalculateHit()
    {
        ClearHitFlag();

        _Collider.enabled = true;

        yield return new WaitForFixedUpdate();

        _NextSpeed = _MoveSpeed;
        _Collider.enabled = false;
        PlayNoHitAudio();
    }
    
    void OnTriggerStay(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        if (!_IsBulletHitLie && targetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR)
            return;

        _NextSpeed = 0;

        BulletHit(targetMotion);
        PlayHitAudio();
    }

    private void ActSubImpacts()
    {
        if (_ImpactBase == null)
        {
            _ImpactBase = new List<ImpactBase>();
            if (_SubImpact != null)
            {
                _ImpactBase.AddRange(_SubImpact.GetComponents<ImpactBase>());
            }
        }

        for (int i = 0; i < _ImpactBase.Count; ++i)
        {
            _ImpactBase[i].ActImpact(_SkillMotion, _SkillMotion);
        }
    }

}
