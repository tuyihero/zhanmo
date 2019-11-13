using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletHitDelay : BulletBase
{
    public GameObject _AlertObj;
    public GameObject _HitObj;
    public float _AlertTime = 0.6f;
    public float _FirstHitTime = 0f;
    public float _HitInterval = 0.1f;
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

        StartCoroutine(StartHit());
    }

    IEnumerator StartHit()
    {
        if(_AlertObj != null)
            _AlertObj.SetActive(true);
        _HitObj.SetActive(false);
        _Collider.enabled = false;
        _StartHit = false;

        yield return new WaitForSeconds(_AlertTime);

        if (_AlertObj != null)
            _AlertObj.SetActive(false);
        _HitObj.SetActive(true);
        //_Collider.enabled = true;

        yield return new WaitForSeconds(_FirstHitTime);
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

        _LiftTime -= Time.fixedDeltaTime;
        if (_LiftTime < 0)
        {
            BulletFinish();
        }
    }

    IEnumerator CalculateHit()
    {
        _Collider.enabled = true;

        yield return new WaitForFixedUpdate();

        _Collider.enabled = false;
    }
    
    void OnTriggerStay(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        BulletHit(targetMotion);
    }

}
