using UnityEngine;
using System.Collections;

public class BulletLineArc : BulletBase
{
    public float _LifeTime = 2.0f;
    public float _Speed = 10;
    public float _Range = 5;
    public int _HitTimes = 1;
    public int _SingleMotionHitTimes = 1;

    // Use this for initialization
    private int _AlreadyHitTimes = 0;
    private float _AwakeTime = 0;
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
        var moveDis = transform.forward.normalized * _Speed * Time.fixedDeltaTime; 
        transform.position += moveDis;
        var angle = Mathf.Asin(moveDis.magnitude / _Range) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, angle, 0));

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

        if (_EmitterBase.GetMotionHitimes(targetMotion) >= _SingleMotionHitTimes)
        {
            return;
        }

        _EmitterBase.AddHitTimes(targetMotion);
        BulletHit(targetMotion);
        ++_AlreadyHitTimes;

        if (_HitTimes > 0 && _AlreadyHitTimes >= _HitTimes)
        {
            BulletFinish();
        }
    }
}
