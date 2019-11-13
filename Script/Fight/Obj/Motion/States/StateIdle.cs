using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : StateBase
{

    protected override string GetAnimName()
    {
        return "idle";
    }

    public override void StateOpt(MotionOpt opt, params object[] args)
    {
        switch (opt)
        {
            case MotionOpt.Input_Direct:
                _MotionManager.TryEnterState(_MotionManager._StateMove, args);
                break;
            case MotionOpt.Move_Target:
                _MotionManager.TryEnterState(_MotionManager._StateMove, args);
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
}
