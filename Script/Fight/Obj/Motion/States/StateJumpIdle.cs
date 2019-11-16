﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJumpIdle : StateBase
{
    protected override string GetAnimName()
    {
        return "jumpIdle";
    }

    public override bool CanStartState(params object[] args)
    {
        return true;
    }

    public override void StartState(params object[] args)
    {
        base.StartState(args);

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
                _MotionManager.JumpMove(moveDirect);
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
        }
    }

    public override void FinishState()
    {
        base.FinishState();
    }

    #region 

    #endregion


}