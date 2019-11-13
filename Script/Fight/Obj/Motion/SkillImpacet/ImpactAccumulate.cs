using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAccumulate : ImpactBase
{
    public void Update()
    {
        if (_IsActingImpact)
        {
            if (!InputManager.Instance.IsKeyHold("k"))
            {
                ReleaseAccumulate();
            }
            else if (Time.time - _HoldTime >= _AccumulateTime)
            {
                ReleaseAccumulate();
            }

        }
    }

    private AnimationClip _AccumulateAnim;
    public float _AccumulateTime;
    public float _AccumulateDamage;

    private float _HoldTime;

    public override void Init(ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        base.Init(skillMotion, selector);

        string animPath = SkillMotion.MotionManager._MotionAnimPath + "/Act_Skill_Accumulate";
        ResourceManager.Instance.LoadAnimation(animPath, (resName, resData, hash)=>
        {
            _AccumulateAnim = resData;
            if (_AccumulateAnim == null)
            {
                Debug.LogError("ImpactAccumulate init anim error: animPath");
                return;
            }

            SkillMotion._NextAnim.Insert(0, _AccumulateAnim);
            SkillMotion._NextEffect.Insert(0, null);
            skillMotion.MotionManager.AnimationEvent.AddSelectorEvent(_AccumulateAnim, 0, selector._ColliderID);
            skillMotion.MotionManager.AnimationEvent.AddSelectorFinishEvent(_AccumulateAnim, 2.0f, selector._ColliderID);
            skillMotion.MotionManager.InitAnimation(_AccumulateAnim);
        }, null);

        //foreach (var animEvent in _AccumulateAnim.events)
        //{
        //    Debug.Log("accumulateAnimEvent:" + animEvent.time + "," + animEvent.functionName);
        //}
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);
        _IsActingImpact = true;
        _HoldTime = Time.time;

        reciverManager.RoleAttrManager.AccumulateDamageRate = 0;
        PlayEffect();
    }

    public override void FinishImpact(MotionManager reciverManager)
    {
        //base.RemoveImpact(reciverManager);
    }

    public override void StopImpact()
    {
        base.StopImpact();

        _IsActingImpact = false;
    }

    private void ReleaseAccumulate()
    {
        float accumulateRate = (Time.time - _HoldTime / _AccumulateTime);
        accumulateRate = Mathf.Clamp(accumulateRate, 0, 1);
        _IsActingImpact = false;
        ReciveMotion.RoleAttrManager.AccumulateDamageRate = accumulateRate * _AccumulateDamage;

        SkillMotion.PlayerNextAnim();
        StopEffect();
    }

    #region play effect

    public EffectController _EffectController;

    private int _DynamicEffect;

    public void PlayEffect()
    {
        SenderMotion.PlaySkillEffect(_EffectController);
    }

    public void StopEffect()
    {
        SenderMotion.StopSkillEffect(_EffectController);
    }

    #endregion
}
