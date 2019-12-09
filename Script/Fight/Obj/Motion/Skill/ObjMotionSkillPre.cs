using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillPre : ObjMotionSkillBase
{
    public float _PreWait = 0.3f;

    private float _PreStartTime = 0;

    void Update()
    {
        if (MotionManager == null)
            return;

        if (!_CanNextInput)
            return;

        if (Time.time - _PreStartTime < _PreWait)
            return;

        if (InputManager.Instance.Axis.y > 0
            && Mathf.Abs(InputManager.Instance.Axis.y) >= Mathf.Abs(InputManager.Instance.Axis.x))
        {
            ResumeAction();
            if (MotionManager.IsInAir())
            {
                MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions["p"]);
            }
            else
            {
                MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions["1"]);
            }
        }
        else if (InputManager.Instance.Axis.y < 0
            && Mathf.Abs(InputManager.Instance.Axis.y) >= Mathf.Abs(InputManager.Instance.Axis.x))
        {
            ResumeAction();
            if (MotionManager.IsInAir())
            {
                MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions["n"]);
            }
            else
            {
                MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions["2"]);
            }
        }
        else if (InputManager.Instance.Axis.x != 0)
        {
            ResumeAction();
            if (InputManager.Instance.Axis.x > 0)
            {
                MotionManager.SetRotate(Vector3.zero);
            }
            else if (InputManager.Instance.Axis.x < 0)
            {
                MotionManager.SetRotate(new Vector3(0, 180, 0));
            }
            MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions["3"]);
        }
        else if (InputManager.Instance.IsKeyHold("k"))
        {
            if (MotionManager._StateSkill._SkillMotions.ContainsKey("4"))
            {
                ResumeAction();
                MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions["4"]);
            }
        }
    }

    public override bool ActSkill(Hashtable exhash)
    {
        Debug.Log("SkillPre pauseAction");
        _PreStartTime = Time.time;
        if (FightManager.Instance != null)
        {
            foreach (var monMotion in FightManager.Instance._MonMotion)
            {
                if (monMotion != MotionManager)
                {
                    monMotion.ActionPause(-1);
                }
            }
        }
        TimeManager.Instance.Pause();
        return base.ActSkill(exhash);
    }

    public override void FinishSkill()
    {
        base.FinishSkill();

        ResumeAction();
    }

    private void ResumeAction()
    {
        Debug.Log("SkillPre ResumeAction");
        if (FightManager.Instance != null)
        {
            foreach (var monMotion in FightManager.Instance._MonMotion)
            {
                if (monMotion != MotionManager)
                {
                    monMotion.ActionResume();
                }
            }
        }
        TimeManager.Instance.Resume();

    }

}
