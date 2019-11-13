using UnityEngine;
using System.Collections;

public class ImpactHitDelay : ImpactHit
{
    public float _DelayTime = 0.1f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        StartCoroutine(ActDelay(senderManager, reciverManager));
    }

    private IEnumerator ActDelay(MotionManager senderManager, MotionManager reciverManager)
    {
        yield return new WaitForSeconds(_DelayTime);

        HitMotion(senderManager, reciverManager);

        ProcessDamge(senderManager, reciverManager);
    }

}
