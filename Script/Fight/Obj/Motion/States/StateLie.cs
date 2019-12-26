using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLie : StateBase
{

    public override void StartState(Hashtable args)
    {
        _LieStartTime = Time.time;
        _RealLieTime = _LieTime * GameDataValue.ConfigIntToFloat(_MotionManager.RoleAttrManager.GetBaseAttr(RoleAttrEnum.RiseUpSpeed));
    }

    public override void StateUpdate()
    {
        LieUpdate();
    }

    public override void StateOpt(MotionOpt opt, Hashtable args)
    {
        switch (opt)
        {
            case MotionOpt.Act_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Input_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Hit:
                args["HitTime"] = 0.01f;
                args.Add("UpSpeed", 1.0f);
                _MotionManager.TryEnterState(_MotionManager._StateFly, args);
                break;
            case MotionOpt.Fly:
                args["FlyTime"] = 0.01f;
                _MotionManager.TryEnterState(_MotionManager._StateFly, args);
                break;
            case MotionOpt.Catch:
                _MotionManager.TryEnterState(_MotionManager._StateCatch, args);
                break;
            default:
                break;
        }
    }

    #region 

    private static float _LieTime = 0.75f;
    private float _LieStartTime = -1;
    private float _RealLieTime = _LieTime;

    private void LieUpdate()
    {
        if (_LieStartTime < 0)
            return;

        float deltaTime = Time.time - _LieStartTime;
        if (deltaTime >= _RealLieTime)
        {
            if (_MotionManager._HasRiseState)
            {
                _MotionManager.TryEnterState(_MotionManager._StateRise);
            }
            else
            {
                _MotionManager.TryEnterState(_MotionManager._StateIdle);
            }
        }
    }

    #endregion
}
