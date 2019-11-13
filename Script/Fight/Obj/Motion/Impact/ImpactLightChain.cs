using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactLightChain : ImpactBase
{
    public float _HitInterval = 0.1f;
    public EffectLightChain _Effect;
    public ImpactBase[] SubImpacts;

    private List<MotionManager> _ExcludeMotions = new List<MotionManager>();

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        _ExcludeMotions.Clear();
        _ExcludeMotions.Add(reciverManager);

        SendImpactNext(senderManager, reciverManager.transform.position);
    }

    public IEnumerator ActNext(MotionManager senderManager, MotionManager lastManager)
    {
        yield return new WaitForSeconds(_HitInterval);

        SendImpactNext(senderManager, lastManager.transform.position);

    }

    private void SendImpactNext(MotionManager senderManager, Vector3 startPos)
    {
        var nearTarget = SelectTargetCommon.GetNearMotion(senderManager, startPos, _ExcludeMotions);
        
        if (nearTarget != null)
        {
            _ExcludeMotions.Add(nearTarget);
            foreach (var impact in SubImpacts)
            {
                impact.ActImpact(senderManager, nearTarget);
            }

            Hashtable hash = new Hashtable();
            hash.Add("PosStart", startPos);

            var targetBindTrans = nearTarget.GetBindTransform(_Effect._BindPos);
            if (targetBindTrans != null)
                hash.Add("PosEnd", targetBindTrans.position);
            else
                hash.Add("PosEnd", nearTarget.transform.position);

            senderManager.PlayDynamicEffect(_Effect, hash);
            StartCoroutine(ActNext(senderManager, nearTarget));
        }
    }

}
