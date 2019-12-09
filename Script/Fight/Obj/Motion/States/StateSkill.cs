using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSkill : StateBase
{
    private int _SkillPrior;

    public override void InitState(MotionManager motionManager)
    {
        base.InitState(motionManager);

        _ActingSkill = null;
        InitSkills();
    }

    public override bool CanStartState(params object[] args)
    {
        bool isCanActSkill = true;
        ObjMotionSkillBase skillBase = args[0] as ObjMotionSkillBase;
        if (skillBase == null)
        {
            string inputKey = args[0] as string;
            if (_SkillMotions.ContainsKey(inputKey))
            {
                skillBase = _SkillMotions[inputKey];
            }

        }

        if (skillBase == null)
            return false;


        isCanActSkill &= skillBase.IsCanActSkill();

        if (FightManager.Instance != null && _MotionManager == FightManager.Instance.MainChatMotion)
        {
            isCanActSkill &= !FightSkillManager.Instance.IsSkillInCD(skillBase);
        }

        isCanActSkill &= skillBase.IsMPEnough();

        return isCanActSkill;
    }

    public override void StartState(params object[] args)
    {
        ActSkill(args);
    }

    public override void FinishState()
    {
        base.FinishState();

        if (ActingSkill != null)
        {
            FinishSkill(ActingSkill);
        }
    }

    public override void StateOpt(MotionOpt opt, object[] args)
    {
        switch (opt)
        {
            case MotionOpt.Act_Skill:
                if (ActingSkill is ObjMotionSkillPre)
                {
                    ((ObjMotionSkillPre)ActingSkill).ResumeSkill();
                }
                ActSkill(args);
                break;
            case MotionOpt.Input_Skill:
                if (ActingSkill is ObjMotionSkillPre)
                {
                    ((ObjMotionSkillPre)ActingSkill).ResumeSkill();
                }
                string key = args[0] as string;
                if (ActingSkill._ActInput == key)
                {

                }
                else
                {
                    ActSkill(args);
                }
                break;
            case MotionOpt.Stop_Skill:
                if (_MotionManager.IsInAir())
                {
                    _MotionManager.TryEnterState(_MotionManager._StateJumpIdle, args);
                    if (_MotionManager.JumpStay)
                    {
                        _MotionManager.JumpFall();
                    }
                }
                else
                {
                    _MotionManager.TryEnterState(_MotionManager._StateIdle, args);
                }
                break;
            case MotionOpt.Anim_Event:
                _ActingSkill.AnimEvent(args[0] as string, args[1]);
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
            case MotionOpt.Pause_State:
                var curAnim = _ActingSkill.GetCurAnim();
                if (curAnim != null)
                {
                    _MotionManager.PauseAnimation(curAnim, (float)args[0]);
                }
                break;
            case MotionOpt.Resume_State:
                var resumeAnim = _ActingSkill.GetCurAnim();
                if (resumeAnim != null)
                {
                    _MotionManager.ResumeAnimation(resumeAnim);
                }
                break;
            default:
                break;
        }
    }

    private void ActSkill(params object[] args)
    {
        ObjMotionSkillBase skillBase = null;
        if (args[0] is ObjMotionSkillBase)
        {
            skillBase = args[0] as ObjMotionSkillBase;
        }
        else if (args[0] is string)
        {
            string inputKey = args[0] as string;
            if (_SkillMotions.ContainsKey(inputKey))
            {
                skillBase = _SkillMotions[inputKey];
            }
        }
        if (skillBase == null)
            return;

        Hashtable hash = null;
        if (args.Length > 1)
        {
            hash = args[1] as Hashtable;
        }

        if (!skillBase.IsCanActSkill())
            return;

        skillBase.CostMP();
        ActSkill(skillBase, hash);
    }

    #region skill base

    public Dictionary<string, ObjMotionSkillBase> _SkillMotions = new Dictionary<string, ObjMotionSkillBase>();

    private ObjMotionSkillBase _ActingSkill;
    public ObjMotionSkillBase ActingSkill
    {
        get
        {
            return _ActingSkill;
        }
    }

    private void InitSkills()
    {
        var skillList = _MotionManager.GetComponentsInChildren<ObjMotionSkillBase>();
        foreach (var skill in skillList)
        {
            skill.gameObject.SetActive(true);
            //Debug.Log("skill._ActInput:" + skill._ActInput);
            _SkillMotions.Add(skill._ActInput, skill);
            //skill.SetImpactElement(ElementType.Cold);
        }

        foreach (var attrImpact in _MotionManager.RoleAttrManager.GetAttrImpacts())
        {
            attrImpact.ModifySkillBeforeInit(_MotionManager);
        }

        foreach (var skill in _SkillMotions.Values)
        {
            skill.Init();
        }

        foreach (var attrImpact in _MotionManager.RoleAttrManager.GetAttrImpacts())
        {
            attrImpact.ModifySkillAfterInit(_MotionManager);
        }
    }

    public void ActSkill(ObjMotionSkillBase skillMotion, Hashtable exHash = null)
    {
        if (!skillMotion.IsCanActSkill())
            return;

        if (_ActingSkill != null)
        {
            FinishSkill(_ActingSkill);
        }

        skillMotion.StartSkill(exHash);
        _ActingSkill = skillMotion;
        _SkillPrior = _ActingSkill._SkillMotionPrior;
    }

    public void FinishSkill(ObjMotionSkillBase skillMotion)
    {
        if (FightManager.Instance != null)
        {
            FightSkillManager.Instance.SetSkillCD(_ActingSkill);
        }
        _ActingSkill = null;
        skillMotion.FinishSkillImmediately();

    }
    

    #endregion
}
