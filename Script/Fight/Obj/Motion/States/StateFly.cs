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

    public override bool CanStartState(Hashtable args)
    {
        return IsBuffCanBeHit((MotionManager)args["SenderMotion"], (ImpactHit)args["HitImpact"]);
    }

    public override void StartState(Hashtable args)
    {
        //base.StartState(args);
        float flyTime = 0;
        if (args.ContainsKey("HitTime"))
        {
            flyTime = (float)args["HitTime"];
        }
        else
        {
            flyTime = (float)args["FlyTime"];
        }
        MotionFly(flyTime, (int)args["HitEffect"], (int)args["HitAudio"], (MotionManager)args["SenderMotion"], (float)args["UpSpeed"]);
        SetHitMove((Vector3)args["MoveDirect"], (float)args["MoveTime"], (bool)args["IsBorder"]);

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
                MotionFlyStay((float)args["HitTime"], (int)args["HitEffect"], (int)args["HitAudio"], (MotionManager)args["SenderMotion"], (bool)args["IsPauseFly"]);
                //MotionFly(0.2f, (int)args[1], (int)args[6], (MotionManager)args[2]);
                SetHitMove((Vector3)args["MoveDirect"], (float)args["MoveTime"], (bool)args["IsBorder"]);
                break;
            case MotionOpt.Fly:
                MotionFly((float)args["FlyTime"], (int)args["HitEffect"], (int)args["HitAudio"], (MotionManager)args["SenderMotion"], (float)args["UpSpeed"]);
                SetHitMove((Vector3)args["MoveDirect"], (float)args["MoveTime"], (bool)args["IsBorder"]);
                break;
            case MotionOpt.Catch:
                _MotionManager.TryEnterState(_MotionManager._StateCatch, args);
                break;
            case MotionOpt.Anim_Event:
                DispatchFlyEvent(args["FuncName"] as string, args["Param"]);
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
    private float _FlyTime = 0;
    private float _StayTime = 0;

    private bool IsFlyEnd = false;

    private void DispatchFlyEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.KEY_FRAME:
                if (_FlyTime > 0)
                {
                    _MotionManager.PauseAnimation();
                }
                break;
            case AnimEventManager.ANIMATION_END:
                FlyEnd();
                break;
        }
    }

    public void SetHitMove(Vector3 moveDirect, float moveTime, bool isBorder)
    {
        if (moveTime <= 0)
            return;

        _MotionManager.SetMove(moveDirect, moveTime, isBorder);
    }

    private void FlyEnd()
    {
        IsFlyEnd = true;
    }

    public void MotionFly(float flyTime, int effectID, int audioID, MotionManager impactSender, float upSpeed)
    {
        //Debug.Log("MotionFly");
        _UpSpeed = upSpeed;
        if (upSpeed == 0)
        {
            _UpSpeed = _ConstUpSpeed;
        }
        if (_UpSpeed < 0)
        {
            _DownSpeed = -_UpSpeed;
        }
        _MotionManager.PlayHitEffect(impactSender, effectID);
        if (audioID > 0)
        {
            _MotionManager.PlayAudio(ResourcePool.Instance._CommonAudio[audioID]);
        }

        _MotionManager.SetLookAt(impactSender.transform.position);
        _FlyTime = flyTime * (GameDataValue.ConfigIntToFloat(_MotionManager.RoleAttrManager.GetBaseAttr(RoleAttrEnum.FlyGravity)));
        if (_MotionManager.IsInAir())
        {
            //_FlyHeight += _MotionManager.JumpBody.localPosition.y;
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
                _DownSpeed = 0;
            }
        }
        else if (_FlyTime > 0)
        {
            _FlyBody.localPosition += _UpSpeed * Time.fixedDeltaTime * Vector3.up;

            //if (_FlyBody.localPosition.y > _FlyHeight)
            //{
            //    _FlyBody.localPosition = new Vector3(0, _FlyHeight, 0);
            //    _FlyHeight = 0;
                _DownSpeed = 0;
            //}

            _FlyTime -= Time.fixedDeltaTime;
        }
        else if (_FlyBody.localPosition.y > 0.0f)
        {
            _DownSpeed += _ConstDownSpeed * Time.fixedDeltaTime;
            _FlyBody.localPosition -= _DownSpeed * Time.fixedDeltaTime * Vector3.up;

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
                _MotionManager.TryEnterState(_MotionManager._StateDie, null);
            }
            else
            {
                _MotionManager.TryEnterState(_MotionManager._StateLie, null);
            }
        }
    }

    #endregion
}
