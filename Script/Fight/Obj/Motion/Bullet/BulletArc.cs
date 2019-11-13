using UnityEngine;
using System.Collections;

public class BulletArc : BulletBase
{
    public float _LifeTime = 2.0f;
    public float _Speed = 10;
    public int _HitTimes = 1;
    public Vector3 _RotSpeed;

    private float _AwakeTime = 0;
    private int _AlreadyHitTimes = 0;
    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _AlreadyHitTimes = 0;
        _AwakeTime = Time.time;
    }

    private IEnumerator FinishDelay()
    {
        yield return new WaitForSeconds(_LifeTime);

        BulletFinish();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position += transform.forward.normalized * _Speed * Time.fixedDeltaTime;

        var angle = _RotSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + angle);

        if (Time.time - _AwakeTime > _LifeTime)
        {
            BulletFinish();
        }
    }

    void OnTriggerStay(Collider other)
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
