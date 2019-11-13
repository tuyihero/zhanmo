using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletHitInterval : BulletBase
{

    public GameObject _HitObj;
    public float _AlertTime = 0.6f;
    public float _AlertSize = 0.6f;
    public float _ShowHitObjDelay = 0f;
    public float _FirstHitDelay = 0f;
    public float _HitInterval = 0.1f;
    public bool _RestartHitObj = false;
    public float _StayTime = 10;

    private bool _StartHit = false;
    private float _LiftTime = 0;
    private float _LastHitTime = 0;
    private Collider _Collider;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion,emitterBase);

        _Collider = gameObject.GetComponent<Collider>();
        _LiftTime = _StayTime;
        _LastHitTime = 0;
        ClearHitFlag();

        StartCoroutine(StartHit());
    }

    IEnumerator StartHit()
    {
        _HitObj.SetActive(false);
        _Collider.enabled = false;
        _StartHit = false;
        if (_AlertTime > 0)
        {
            BulletAlert.ShowAlert(transform, _AlertTime, _AlertSize);
        }

        yield return new WaitForSeconds(_AlertTime);

        //_Collider.enabled = true;

        yield return new WaitForSeconds(_ShowHitObjDelay);
        _HitObj.SetActive(true);

        yield return new WaitForSeconds(_FirstHitDelay);

        if (_BornAudio > 0)
        {
            _SkillMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_BornAudio]);
        }

        _StartHit = true;
    }

    void FixedUpdate()
    {
        if (!_StartHit)
            return;

        if (_HitInterval > 0)
        {
            if (_LastHitTime + _HitInterval < Time.time)
            {
                StartCoroutine(CalculateHit());
                _LastHitTime = Time.time;
            }
        }
        else if(_LastHitTime == 0)
        {
            StartCoroutine(CalculateHit());
            _LastHitTime = Time.time;
        }

        _LiftTime -= Time.fixedDeltaTime;
        if (_LiftTime < 0)
        {
            BulletFinish();
        }
    }

    IEnumerator CalculateHit()
    {
        ClearHitFlag();
        _Collider.enabled = true;
        if (_RestartHitObj)
        {
            _HitObj.SetActive(false);
            _HitObj.SetActive(true);
        }

        yield return new WaitForFixedUpdate();

        PlayNoHitAudio();
        _Collider.enabled = false;
    }
    
    void OnTriggerStay(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        if (!_IsBulletHitLie && targetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR)
            return;

        BulletHit(targetMotion);
        PlayHitAudio();
    }
    
}
