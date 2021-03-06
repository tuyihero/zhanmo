﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHit : StateBase
{
    protected override string GetAnimName()
    {
        return "hurt";
    }

    public override void InitState(MotionManager motionManager)
    {
        base.InitState(motionManager);

        _MotionManager.AddAnimationEndEvent(_Animation);
    }

    public override bool CanStartState(Hashtable args)
    {
        return IsBuffCanBeHit((MotionManager)args["SenderMotion"], (ImpactHit)args["HitImpact"]);
    }

    public override void StartState(Hashtable args)
    {
        {
            MotionHit((float)args["HitTime"], (int)args["HitEffect"], (int)args["HitAudio"], (MotionManager)args["SenderMotion"]);
            SetHitMove((Vector3)args["MoveDirect"], (float)args["MoveTime"], (bool)args["IsBorder"]);
        }

        if (_MotionManager._BehitAudio != null)
        {
            _MotionManager.PlayAudio(_MotionManager._BehitAudio);
        }
    }

    public override void StateOpt(MotionOpt opt, Hashtable args)
    {
        switch (opt)
        {
            case MotionOpt.Pause_State:
                _MotionManager.PauseAnimation(_Animation, (float)args["PauseTime"]);
                break;
            case MotionOpt.Resume_State:
                _MotionManager.ResumeAnimation(_Animation);
                break;
            case MotionOpt.Act_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Input_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Hit:
                //_MotionManager.TryEnterState(_MotionManager._StateHit, args);
                MotionHit((float)args["HitTime"], (int)args["HitEffect"], (int)args["HitAudio"], (MotionManager)args["SenderMotion"]);
                SetHitMove((Vector3)args["MoveDirect"], (float)args["MoveTime"], (bool)args["IsBorder"]);
                break;
            case MotionOpt.Fly:
                _MotionManager.TryEnterState(_MotionManager._StateFly, args);
                break;
            case MotionOpt.Catch:
                _MotionManager.TryEnterState(_MotionManager._StateCatch, args);
                break;
            case MotionOpt.Anim_Event:
                DispatchHitEvent(args["FuncName"] as string, args["Param"]);
                break;

            default:
                break;
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        ConsumeAnimUpdate();
    }

    #region 

    Hashtable _BuffArg = new Hashtable();
    public bool IsBuffCanBeHit(MotionManager impactSender, ImpactHit impactHit)
    {
        _BuffArg.Clear();
        _BuffArg.Add(ImpactBuff.BuffModifyType.IsCanHit, true);
        _MotionManager.ForeachBuffModify(ImpactBuff.BuffModifyType.IsCanHit, _BuffArg, impactSender, impactHit);
        return (bool)_BuffArg[ImpactBuff.BuffModifyType.IsCanHit];
    }

    private float _StopKeyFrameTime = 0.0f;
    private void DispatchHitEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.KEY_FRAME:
                HitKeyframe(param);
                break;
            case AnimEventManager.ANIMATION_END:
                _MotionManager.TryEnterState(_MotionManager._StateIdle, null);
                break;
        }
    }

    public void SetHitMove(Vector3 moveDirect, float moveTime, bool isBorder)
    {
        if (moveTime <= 0)
            return;

        _MotionManager.SetMove(moveDirect, moveTime, isBorder);
    }

    public void MotionHit(float hitTime, int hitEffect, int hitAudio, MotionManager impactSender)
    {
        _MotionManager.PlayHitEffect(impactSender, hitEffect);

        var realHitTime = hitTime * (GameDataValue.ConfigIntToFloat(_MotionManager.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HitBack)));

        if (hitAudio > 0)
            _MotionManager.PlayAudio(ResourcePool.Instance._CommonAudio[hitAudio]);
        if (realHitTime <= 0)
            return;
        
        
        float speed = 1;
        if (realHitTime > _Animation.length)
        {
            _StopKeyFrameTime = realHitTime - _Animation.length;
        }
        else
        {
            _StopKeyFrameTime = 0;
            speed = (_Animation.length / realHitTime);
        }

        _StopFramTime = 0;
        _MotionManager.RePlayAnimation(_Animation, speed);
        //_MotionManager.SetLookAt(impactSender.transform.position);
    }

    public void HitKeyframe(object param)
    {
        if (_StopKeyFrameTime > 0)
        {
            _MotionManager.PauseAnimation(_Animation, -1);
            _StopFramTime = _StopKeyFrameTime;
        }
    }

    private float _StopFramTime = 0;
    public void ConsumeAnimUpdate()
    {
        if (_StopFramTime > 0)
        {
            _StopFramTime -= Time.fixedDeltaTime;
            if (_StopFramTime <= 0)
            {
                _MotionManager.ResumeAnimation(_Animation);
            }
        }
    }

    public IEnumerator ComsumeAnim()
    {
        yield return new WaitForSeconds(_StopKeyFrameTime);

        _MotionManager.ResumeAnimation(_Animation);
    }

    #endregion
}
