using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFly : StateBase
{

    protected override string GetAnimName()
    {
        return "down";
    }

    public override void InitState(MotionManager motionManager)
    {
        base.InitState(motionManager);

        _MotionManager.AddAnimationEndEvent(_Animation);
        _MotionManager.AnimationEvent.AddEvent(_Animation, 0.65f, FlyEnd);
        _FlyBody = _MotionManager.JumpBody;
    }

    public override bool CanStartState(params object[] args)
    {
        return IsBuffCanBeHit((MotionManager)args[2], (ImpactHit)args[3]);
    }

    public override void StartState(params object[] args)
    {
        //base.StartState(args);
        var param1 = (float)args[0];
        var param2 = (int)args[1];
        var param3 = (int)args[6];
        var param4 = (MotionManager)args[2];
        var param5 = (float)args[8];
        MotionFly((float)args[0], (int)args[1], (int)args[6], (MotionManager)args[2], (float)args[8]);
        SetHitMove((Vector3)args[4], (float)args[5]);

        if (_MotionManager._BehitAudio != null)
        {
            _MotionManager.PlayAudio(_MotionManager._BehitAudio);
        }
    }

    public override void StateOpt(MotionOpt opt, params object[] args)
    {
        switch (opt)
        {
            case MotionOpt.Pause_State:
                _MotionManager.PauseAnimation(_Animation, (float)args[0]);
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
                MotionFlyStay((float)args[0], (int)args[1], (int)args[6], (MotionManager)args[2], (bool)args[7]);
                //MotionFly(0.2f, (int)args[1], (int)args[6], (MotionManager)args[2]);
                SetHitMove((Vector3)args[4], (float)args[5]);
                break;
            case MotionOpt.Fly:
                MotionFly((float)args[0], (int)args[1], (int)args[6], (MotionManager)args[2], (float)args[8]);
                SetHitMove((Vector3)args[4], (float)args[5]);
                break;
            case MotionOpt.Catch:
                _MotionManager.TryEnterState(_MotionManager._StateCatch, args);
                break;
            case MotionOpt.Anim_Event:
                DispatchFlyEvent(args[0] as string, args[1]);
                break;
            default:
                break;
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        FlyUpdate();
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

    private const float _ConstUpSpeed = 5;
    private float _UpSpeed = 5;
    private const float _ConstDownSpeed = 40;
    private float _DownSpeed = 0;

    private Transform _FlyBody;
    public Transform FlyBody
    {
        get
        {
            return _FlyBody;
        }
    }
    private float _FlyHeight = 0;
    private float _StayTime = 0;

    private bool IsFlyEnd = false;

    private void DispatchFlyEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.KEY_FRAME:
                if (_FlyHeight > 0)
                {
                    _MotionManager.PauseAnimation();
                }
                break;
            case AnimEventManager.ANIMATION_END:
                FlyEnd();
                break;
        }
    }

    public void SetHitMove(Vector3 moveDirect, float moveTime)
    {
        if (moveTime <= 0)
            return;

        _MotionManager.SetMove(moveDirect, moveTime);
    }

    private void FlyEnd()
    {
        IsFlyEnd = true;
    }

    public void MotionFly(float flyHeight, int effectID, int audioID, MotionManager impactSender, float upSpeed)
    {
        //Debug.Log("MotionFly");
        _UpSpeed = upSpeed;
        if (upSpeed == 0)
        {
            _UpSpeed = _ConstUpSpeed;
        }
        _MotionManager.PlayHitEffect(impactSender, effectID);
        if (audioID > 0)
        {
            _MotionManager.PlayAudio(ResourcePool.Instance._CommonAudio[audioID]);
        }

        _MotionManager.SetLookAt(impactSender.transform.position);
        _FlyHeight = flyHeight * (GameDataValue.ConfigIntToFloat(_MotionManager.RoleAttrManager.GetBaseAttr(RoleAttrEnum.FlyGravity)));
        if (_MotionManager.IsInAir())
        {
            _FlyHeight += _MotionManager.JumpBody.localPosition.y;
            _MotionManager.ResetJump();
        }

        IsFlyEnd = false;
        _MotionManager.SetCorpsePrior();

        _MotionManager.RePlayAnimation(_Animation, 1);
    }

    public void MotionFlyStay(float time, int effectID, int audioID, MotionManager impactSender, bool isPauseFly)
    {
        //Debug.Log("MotionFlyStay");
        _MotionManager.PlayHitEffect(impactSender, effectID);

        if (audioID > 0)
            _MotionManager.PlayAudio(ResourcePool.Instance._CommonAudio[audioID]);

        //Debug.Log("MotionFlyStay isPauseFly:" + isPauseFly);
        if (isPauseFly)
        {
            var flyStayTime = time * (GameDataValue.ConfigIntToFloat(_MotionManager.RoleAttrManager.GetBaseAttr(RoleAttrEnum.FlyGravity)));
            _MotionManager.PauseAnimation(_Animation, flyStayTime);
            _StayTime = flyStayTime;
        }
        else
        {
            _MotionManager.RePlayAnimation(_Animation, 1);
            _StayTime = 0.2f;
        }

        
    }

    public void ResetFly()
    {
        _FlyBody.localPosition = Vector3.zero;
    }

    public void FlyUpdate()
    {
        if (_StayTime > 0)
        {
            _StayTime -= Time.fixedDeltaTime;
            if (_StayTime <= 0)
            {
                _MotionManager.ResumeAnimation(_Animation);
            }
        }
        else if (_FlyHeight > 0)
        {
            _FlyBody.localPosition += _UpSpeed * Time.fixedDeltaTime * Vector3.up;

            if (_FlyBody.localPosition.y > _FlyHeight)
            {
                _FlyBody.localPosition = new Vector3(0, _FlyHeight, 0);
                _FlyHeight = 0;
                _DownSpeed = 0;
            }

            //Debug.Log("_FlyHeight:" + _FlyBody.transform.localPosition.y);
        }
        else if (_FlyBody.localPosition.y > 0.0f)
        {
            _DownSpeed += _ConstDownSpeed * Time.fixedDeltaTime;
            _FlyBody.localPosition -= _DownSpeed * Time.fixedDeltaTime * Vector3.up;
            Debug.Log("_FlyBody.localPosition:" + _FlyBody.localPosition.y + ",isDown:" + (_FlyBody.localPosition.y <= 0.01f));

            if (_FlyBody.localPosition.y <= 0.01f)
            {
                _FlyBody.localPosition = Vector3.zero;
                _MotionManager.ResumeAnimation();
            }
            //Debug.Log("_FlyBody.transform.localPosition.y:" + _FlyBody.transform.localPosition.y);
        }
        else if (IsFlyEnd)
        {
            if (_MotionManager.IsMotionDie)
            {
                _MotionManager.TryEnterState(_MotionManager._StateDie);
            }
            else
            {
                _MotionManager.TryEnterState(_MotionManager._StateLie);
            }
        }
    }

    #endregion
}
