using UnityEngine;
using System.Collections;

public class BulletLeiZhenZiLightBall : BulletBase
{
    //public Vector3 _StartSpeed;
    public float _Speed1 = 5;
    public float _Speed2 = 10;
    public float _AccelerateRange = 5;
    public float _BoomTime = 0.5f;
    public EffectController _SubEffect;

    private MotionManager _TargetMotion;
    private Vector3 _TargetPosition;
    private float _MoveSpeed;
    private float _LastBoomTime;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        var target = SelectTargetCommon.GetNearMotion(senderMotion, senderMotion.transform.position, null);
        if (target != null)
        {
            _TargetMotion = target.GetComponent<MotionManager>();
        }

        _LastBoomTime = _BoomTime;
        _MoveSpeed = _Speed1;
        _TargetPosition = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetDirect = _TargetPosition;
        if (targetDirect == Vector3.zero)
        {
            targetDirect = _TargetMotion.transform.position - transform.position;
        }
        else
        {
            targetDirect = _TargetPosition - transform.position;
        }
        var moveSpeed = _MoveSpeed * targetDirect.normalized;
        transform.position += moveSpeed * Time.fixedDeltaTime;

        if (_TargetPosition == Vector3.zero && targetDirect.magnitude < _AccelerateRange)
        {
            _TargetPosition = _TargetMotion.transform.position;
            _MoveSpeed = _Speed2;
        }
        else if (_TargetPosition != Vector3.zero && targetDirect.magnitude < 0.1f)
        {
            BulletFinish();
        }
    }

    protected override void BulletFinish()
    {
        if (_SubEffect != null)
        {
            ResourcePool.Instance.PlaySceneEffect(_SubEffect, transform.position, Vector3.zero);
        }

        base.BulletFinish();
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        BulletHit(targetMotion);

        if (_SubEffect != null)
        {
            ResourcePool.Instance.PlaySceneEffect(_SubEffect, transform.position, Vector3.zero);
        }

        BulletFinish();
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollierEnter:" + other.ToString());
    }
}
