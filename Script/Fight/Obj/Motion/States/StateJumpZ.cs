using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJumpZ : StateBase
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

        if (args.ContainsKey("JumpToPosIdx"))
        {
            _MotionManager.JumpToZPos((int)args["JumpToPosIdx"]);
        }
    }

    public override void StateOpt(MotionOpt opt, Hashtable args)
    {
        switch (opt)
        {
            case MotionOpt.Pause_State:
                _MotionManager.PauseAnimation(_Animation, (float)args[0]);
                break;
            case MotionOpt.Resume_State:
                _MotionManager.ResumeAnimation(_Animation);
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
