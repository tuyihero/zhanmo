using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletXiShiFeng : BulletBase
{
    //public Vector3 _StartSpeed;
    public float _Speed;
    public float _MaxRange;
    public float _Stage1Time = 2.0f;
    public float _Stage2Time = 4.0f;

    private Vector3 _InitPos;
    private float _MoveRange = 0;
    private bool _IsMoveBack = false;
    private Vector3 _MoveDirect;
    private float _AwakeTime = 0;

    private static List<MotionManager> _HittedMotion = new List<MotionManager>();

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _AwakeTime = Time.time;
        _InitPos = transform.position;
        _MoveRange = 0;
        _IsMoveBack = false;

        _HittedMotion.Clear();
    }

    public void SetDirect(Vector3 direct)
    {
        _MoveDirect = direct.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - _AwakeTime < _Stage1Time)
        {
            var moveDis = transform.forward.normalized * _Speed * Time.fixedDeltaTime;
            transform.position += moveDis;
            //var angle = Mathf.Asin(moveDis.magnitude / _MaxRange) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, angle, 0));
        }
        else if (Time.time - _AwakeTime < _Stage1Time + _Stage2Time)
        {

        }
        else
        {
            BulletFinish();
        }
    }

    protected override void BulletFinish()
    {
        base.BulletFinish();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;
        if (Time.time - _AwakeTime < _Stage1Time)
        {
            if (!_HittedMotion.Contains(targetMotion))
            {
                BulletHit(targetMotion);
                _HittedMotion.Add(targetMotion);
            }
        }
        else
        {
            BulletHit(targetMotion);
        }
    }
}
