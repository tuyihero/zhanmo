using UnityEngine;
using System.Collections;

public class ImpactPlayEffect : ImpactBase
{
    public EffectController _EffectController;
    public bool _PlayReciver = true;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        if (_PlayReciver)
        {
            reciverManager.PlaySkillEffect(_EffectController);
        }
        else
        {
            senderManager.PlaySkillEffect(_EffectController);
        }
    }

}
