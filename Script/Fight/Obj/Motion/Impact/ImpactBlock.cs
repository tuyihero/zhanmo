using UnityEngine;
using System.Collections;

public class ImpactBlock : ImpactBuff
{
    public EffectController _HitEffect;

    private MotionManager _BuffOwner;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _BuffOwner = reciverManager;
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);
        
    }

    private void HitEvent(object sender, Hashtable eventArgs)
    {
        eventArgs.Add("StopEvent", true);
        //GlobalEffect.Instance.Pause(0.1f);
        _BuffOwner.ResetMove();
        //_BuffOwner.SkillPause(0.3f);
        _BuffOwner.PlaySkillEffect(_HitEffect);
    }

    private void FlyEvent(object sender, Hashtable eventArgs)
    {
        eventArgs.Add("StopEvent", true);
        //GlobalEffect.Instance.Pause(0.1f);
        _BuffOwner.ResetMove();
        //_BuffOwner.SkillPause(0.3f);
        _BuffOwner.PlaySkillEffect(_HitEffect);

    }
}
