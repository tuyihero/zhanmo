using UnityEngine;
using System.Collections;

public class BulletRot : BulletBase
{
    public float _LifeTime = 2.0f;
    public Vector3 _RotSpeed = new Vector3(0, 30, 0);
    public int _HitTimes = 1;

    private float _AwakeTime = 0;
    private int _AlreadyHitTimes = 0;
    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _AlreadyHitTimes = 0;
        _AwakeTime = Time.time;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + _RotSpeed * Time.fixedDeltaTime);

        if (Time.time - _AwakeTime > _LifeTime)
        {
            BulletFinish();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        if (!_IsBulletHitLie && (targetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR || targetMotion.MotionPrior == BaseMotionManager.RISE_PRIOR))
            return;

        BulletHit(targetMotion);
        ++_AlreadyHitTimes;

        if (_HitTimes > 0 && _AlreadyHitTimes >= _HitTimes)
        {
            BulletFinish();
        }
    }
}
