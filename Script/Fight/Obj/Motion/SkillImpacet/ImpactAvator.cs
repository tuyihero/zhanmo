using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAvator : ImpactBase
{
    private EffectAfterAnim _AvatorEffect;
    private string _AvatorEffectName = "Effect/Skill/Effect_Char_AfterAnim";

    public int _AvatorCnt;
    public float _AvatorDamage;

    public override void Init(ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        base.Init(skillMotion, selector);
        ResourcePool.Instance.LoadEffect(_AvatorEffectName, (resName, effect, hash)=>
        {
            _AvatorEffect = effect as EffectAfterAnim;
        }, null);
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var effectTime = _SkillMotion.GetTotalAnimLength();
        _AvatorEffect._Duration = effectTime;
        _AvatorEffect._Interval = 0.05f;
        _AvatorEffect._FadeOut = 0.05f * _AvatorCnt;
        _SkillMotion.MotionManager.PlaySkillEffect(_AvatorEffect);
    }

    public override void StopImpact()
    {
        base.StopImpact();

        _SkillMotion.MotionManager.StopSkillEffect(_AvatorEffect);
    }
}
