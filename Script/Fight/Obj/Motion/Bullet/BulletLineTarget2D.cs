using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletLineTarget2D : BulletLine2D
{

    private Vector3 _Direct;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        var senderAI = senderMotion.GetComponent<AI_Base>();
        _Direct = MotionManager.GetForward2D(transform.rotation.eulerAngles);
        if (senderAI != null && senderAI._TargetMotion != null)
        {
            var targetPos = senderAI._TargetMotion.transform.position;
            _Direct = targetPos - senderMotion.transform.position;
        }
    }

    private IEnumerator FinishDelay()
    {
        yield return new WaitForSeconds(_LifeTime);

        BulletFinish();
    }

    protected override void MoveUpdate()
    {
        transform.position += _Direct.normalized * _Speed * Time.fixedDeltaTime;

        RefreshSpriteRot();

        if (Time.time - _AwakeTime > _LifeTime)
        {
            BulletFinish();
        }
    }
    
}
