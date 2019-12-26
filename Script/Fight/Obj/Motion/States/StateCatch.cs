using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCatch : StateBase
{
    protected override string GetAnimName()
    {
        return "Act_Hit_01";
    }

    public override void InitState(MotionManager motionManager)
    {
        base.InitState(motionManager);

        _MotionManager.AddAnimationEndEvent(_Animation);
    }
    public override void StartState(Hashtable args)
    {
        //base.StartState(args);
        MotionHit((float)args["CatchTime"], (int)args["HitEffect"], (int)args["HitAudio"], (MotionManager)args["SenderMotion"]);
        SetHitMove((Vector3)args["MoveDirect"], (float)args["MoveTime"]);

        if (_MotionManager._BehitAudio != null)
        {
            _MotionManager.PlayAudio(_MotionManager._BehitAudio);
        }
    }


    public override void StateOpt(MotionOpt opt, Hashtable args)
    {
        switch (opt)
        {
            case MotionOpt.Anim_Event:
                DispatchHitEvent(args["FuncName"] as string, args["Param"]);
                break;
            case MotionOpt.Catch:
                MotionHit((float)args["CatchTime"], (int)args["HitEffect"], (int)args["HitAudio"], (MotionManager)args["SenderMotion"]);
                SetHitMove((Vector3)args["MoveDirect"], (float)args["MoveTime"]);
                break;
            case MotionOpt.Stop_Catch:
                //_MotionManager.FlyEvent(0.1f, -1, -1, _MotionManager, null, Vector3.zero, 0);
                StopCatch();
                break;
            default:
                break;
        }
    }

    #region MyRegion

    private float _StopKeyFrameTime = 0.0f;
    private void DispatchHitEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.KEY_FRAME:
                HitKeyframe(param);
                break;
            case AnimEventManager.ANIMATION_END:
                StopCatch();
                break;
        }
    }

    public void StopCatch()
    {
        _MotionManager.FlyEvent(0.1f, 0, -1, _MotionManager, null, new Vector3(0, 0, 0), 0.0f, 0.0f,true);
    }

    public void HitKeyframe(object param)
    {
        if (_StopKeyFrameTime > 0)
        {
            _MotionManager.PauseAnimation(_Animation, -1);
            _MotionManager.StartCoroutine(ComsumeAnim());
        }
    }

    public void SetHitMove(Vector3 moveDirect, float moveTime)
    {
        if (moveTime <= 0)
            return;

        _MotionManager.SetMove(moveDirect, moveTime);
    }

    public void MotionHit(float hitTime, int hitEffect, int audioID, MotionManager impactSender)
    {
        _MotionManager.PlayHitEffect(impactSender, hitEffect);
        if (audioID > 0)
        {
            _MotionManager.PlayAudio(ResourcePool.Instance._CommonAudio[audioID]);
        }
        if (hitTime <= 0)
            return;

        if (hitTime > _Animation.length)
        {
            _StopKeyFrameTime = hitTime - _Animation.length;
        }
        else
        {
            _StopKeyFrameTime = 0;
        }

        _MotionManager.StopCoroutine("ComsumeAnim");
        _MotionManager.RePlayAnimation(_Animation, 1);
        //_MotionManager.SetLookAt(impactSender.transform.position);
    }

    public IEnumerator ComsumeAnim()
    {
        yield return new WaitForSeconds(_StopKeyFrameTime);

        _MotionManager.ResumeAnimation(_Animation);
        StopCatch();
    }

    #endregion
}
