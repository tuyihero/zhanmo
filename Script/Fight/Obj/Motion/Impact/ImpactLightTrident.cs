using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactLightTrident : ImpactBase
{
    public float _HitInterval = 0.1f;
    public EffectLightChain _Effect;
    public ImpactBase[] SubImpacts;

    private List<MotionManager> _ExcludeMotions = new List<MotionManager>();

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        _ExcludeMotions.Clear();
        _ExcludeMotions.Add(reciverManager);

        SendImpactNext(senderManager);
    }

    private void SendImpactNext(MotionManager senderManager)
    {
        var nearTargets = SelectTargetCommon.GetNearMotions(senderManager);
        
        if (nearTargets.Count > 0)
        {
            foreach (var nearTarget in nearTargets)
            {
                foreach (var impact in SubImpacts)
                {
                    impact.ActImpact(senderManager, nearTarget);
                }

                Hashtable hash = new Hashtable();
                hash.Add("PosStart", senderManager.transform.position);
                hash.Add("PosEnd", nearTarget.transform.position);

                senderManager.PlayDynamicEffect(_Effect, hash);
            }

            
        }
    }

}
