using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterHittedTarget : BulletEmitterElement
{
    public float _DelayTime;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        StartCoroutine(SendBulletDelay());
    }

    private IEnumerator SendBulletDelay()
    {
        yield return new WaitForSeconds(_DelayTime);

        var skillBase = transform.parent.GetComponent<ObjMotionSkillBase>();
        if (skillBase == null)
            yield break;

        foreach (var hitMotion in skillBase._SkillHitMotions)
        {
            var bullet = InitBulletGO<BulletBase>();
            bullet.transform.position = hitMotion.transform.position;
        }

        skillBase._SkillHitMotions.Clear();
    }

}
