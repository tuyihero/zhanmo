using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletArcTarget2D : BulletLine2D
{
    public float _LifeTime;

    private Vector3 _TargetPos;
    private Vector3 _Direct;

    private Vector2 _MoveSpeed = Vector2.zero;
    private float _MoveTime = 0;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        var senderAI = senderMotion.GetComponent<AI_Base>();
        _TargetPos = MotionManager.GetForward2D(senderMotion.transform.rotation.eulerAngles) * 3;
        _Direct = MotionManager.GetForward2D(senderMotion.transform.rotation.eulerAngles);
        if (senderAI != null && senderAI._TargetMotion != null)
        {
            var targetPos = senderAI._TargetMotion.transform.position;
            _TargetPos = targetPos;
            _Direct = targetPos - transform.position;
        }

        _MoveSpeed.x = _Direct.magnitude / _LifeTime;
        _MoveSpeed.y = 10;
        _MoveTime = 0;
    }

    protected override void MoveUpdate()
    {
        if (_MoveTime < _LifeTime * 0.5f)
        {
            transform.position += _Direct.normalized * _MoveSpeed.x * Time.fixedDeltaTime;
            transform.position += new Vector3(0, 1, 0) * _MoveSpeed.y * Time.fixedDeltaTime;
        }
        else
        {
            transform.position += _Direct.normalized * _MoveSpeed.x * Time.fixedDeltaTime;
            transform.position -= new Vector3(0, 1, 0) * _MoveSpeed.y * Time.fixedDeltaTime;
        }

        RefreshSpriteRot();

        _MoveTime += Time.fixedDeltaTime;

        if (_MoveTime > _LifeTime + 0.2f)
        {
            BulletFinish();
        }
    }
    
}
