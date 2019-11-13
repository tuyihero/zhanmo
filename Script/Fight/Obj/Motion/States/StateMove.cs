using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : StateBase
{
    protected override string GetAnimName()
    {
        return "run";
    }

    public override bool CanStartState(params object[] args)
    {
        if (args[0] is Vector2)
        {
            if ((Vector2)args[0] == Vector2.zero)
                return false;
        }

        return true;
    }

    public override void StartState(params object[] args)
    {
        base.StartState(args);

        if (args[0] is Vector2)
        {
            _MotionManager.MoveDirect((Vector2)args[0]);
        }
        else if(args[0] is Vector3)
        {
            _MotionManager.MoveTarget((Vector3)args[0]);
        }
    }

    public override void StateOpt(MotionOpt opt, params object[] args)
    {
        switch (opt)
        {
            case MotionOpt.Input_Direct:
                Vector2 moveDirect = Vector2.zero;
                if (args[0] is Vector2)
                {
                    moveDirect = (Vector2)args[0];
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
                _MotionManager.MoveTarget((Vector3)args[0]);
                break;
            case MotionOpt.Stop_Move:
                _MotionManager.TryEnterState(_MotionManager._StateIdle, args);
                break;
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

    #region 

    #endregion
}
