using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : StateBase
{
    protected override string GetAnimName()
    {
        return "run";
    }

    public override bool CanStartState(Hashtable args)
    {
        if (args.ContainsKey("InputDirect"))
        {
            if ((Vector2)args["InputDirect"] == Vector2.zero)
                return false;
        }

        return true;
    }

    public override void StartState(Hashtable args)
    {
        base.StartState(args);

        if (args.ContainsKey("InputDirect"))
        {
            _MotionManager.MoveDirect((Vector2)args["InputDirect"]);
        }
        else if(args.ContainsKey("MoveTarget"))
        {
            _MotionManager.MoveTarget((Vector3)args["MoveTarget"], (float)args["MoveSpeed"]);
            PlayAnim((Vector3)args["MoveTarget"], (float)args["MoveSpeed"], (Transform)args["MoveLookatTrans"]);
        }
    }

    public override void StateOpt(MotionOpt opt, Hashtable args)
    {
        switch (opt)
        {
            case MotionOpt.Input_Direct:
                Vector2 moveDirect = Vector2.zero;
                if (args.ContainsKey("InputDirect"))
                {
                    moveDirect = (Vector2)args["InputDirect"];
                }
                if (moveDirect != Vector2.zero)
                {
                    _MotionManager.MoveDirect(moveDirect);
                }
                else
                {
                    _MotionManager.TryEnterState(_MotionManager._StateIdle, args);
                }
                break;
            case MotionOpt.Move_Target:
                _MotionManager.MoveTarget((Vector3)args["MoveTarget"], (float)args["MoveSpeed"]);
                PlayAnim((Vector3)args["MoveTarget"], (float)args["MoveSpeed"], (Transform)args["MoveLookatTrans"]);
                break;
            case MotionOpt.Stop_Move:
                _MotionManager.TryEnterState(_MotionManager._StateIdle, args);
                break;
            case MotionOpt.Jump:
                _MotionManager.TryEnterState(_MotionManager._StateJump, args);
                break;
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
                _MotionManager.TryEnterState(_MotionManager._StateHit, args);
                break;
            case MotionOpt.Fly:
                _MotionManager.TryEnterState(_MotionManager._StateFly, args);
                break;
            case MotionOpt.Catch:
                _MotionManager.TryEnterState(_MotionManager._StateCatch, args);
                break;
            default:
                break;
        }
    }

    public override void FinishState()
    {
        base.FinishState();

        _MotionManager.StopMove();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        UpdateLookAtTrans();
    }

    #region 

    private Vector3 _TargetPos;
    private Transform _LookTransform;
    private float _MoveSpeedRate;

    private Vector3 _LastRote = Vector3.zero;

    private void UpdateLookAtTrans()
    {
        if (_LookTransform == null)
        {
            return;
        }

        if (_LookTransform.position.x > _MotionManager.transform.position.x)
        {
            if (_LastRote != Vector3.zero)
            {
                _LastRote = Vector3.zero;
                RefreshAnim();
                _MotionManager.SetRotate(Vector3.zero);
            }
            
        }
        else if (_LookTransform.position.x < _MotionManager.transform.position.x)
        {
            if (_LastRote.y != 180)
            {
                _LastRote = new Vector3(0, 180, 0);
                RefreshAnim();
                _MotionManager.SetRotate(_LastRote);
            }
            
        }
    }

    private void RefreshAnim()
    {
        //if (_LookTransform != null &&
        //    ((_TargetPos.x > _MotionManager.transform.position.x && _LookTransform.position.x < _MotionManager.transform.position.x)
        //    || (_TargetPos.x < _MotionManager.transform.position.x && _LookTransform.position.x > _MotionManager.transform.position.x)))
        //{
        //    _MotionManager.RePlayAnimation(_Animation, -_MoveSpeedRate);
        //}
        //else
        {
            _MotionManager.RePlayAnimation(_Animation, _MoveSpeedRate);
        }
    }

    private void PlayAnim(Vector3 target, float speed, Transform lookAtTrans)
    {
        _TargetPos = target;
        if (_LookTransform != lookAtTrans || _MoveSpeedRate != speed)
        {
            _LookTransform = lookAtTrans;
            _MoveSpeedRate = speed;
            RefreshAnim();
        }
    }

    #endregion
}
