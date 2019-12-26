using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : StateBase
{
    protected override string GetAnimName()
    {
        return "jump";
    }

    public override bool CanStartState(Hashtable args)
    {
        return true;
    }

    public override void StartState(Hashtable args)
    {
        base.StartState(args);

        _MotionManager.Jump(1);
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
                _MotionManager.JumpMove(moveDirect);
                break;
            case MotionOpt.Pause_State:
                _MotionManager.PauseAnimation(_Animation, (float)args["PauseTime"]);
                break;
            case MotionOpt.Resume_State:
                _MotionManager.ResumeAnimation(_Animation);
                break;
            case MotionOpt.Jump:
                if (args.ContainsKey("JumpToPosIdx"))
                {
                    _MotionManager.TryEnterState(_MotionManager._StateJumpZ, args);
                }
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
