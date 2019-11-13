using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBuff : ObjMotionSkillBase
{

    void Update()
    {
        if (MotionManager == null)
            return;

        if (_CanNextInput && _IsCanActAfterBuff)
        {
            if (InputManager.Instance.IsKeyHold("k") || InputManager.Instance.IsKeyHold(_ActSkillInput))
            {
                _SkillProcess = 1.1f;
                InputManager.Instance.SetRotate();
                MotionManager.FinishSkill(this);
                MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions[_ActSkillInput]);
            }
        }

        _SkillProcess += Time.deltaTime;
        //MotionManager.SkillProcessing = _SkillProcess / GetTotalAnimLength();
    }

    private bool _IsCanActAfterBuff = false;
    public bool IsCanActAfterBuff
    {
        get
        {
            return _IsCanActAfterBuff;
        }
        set
        {
            _IsCanActAfterBuff = value;
        }
    }
    private float _SkillProcess = 0;

    private int _AttackStep = 0;
    private string _ActSkillInput = "";

    public override void AnimEvent(string function, object param)
    {
        base.AnimEvent(function, param);

        switch (function)
        {
            case AnimEventManager.NEXT_INPUT_START:
                _CanNextInput = true;
                StartNextInput();
                break;
            case AnimEventManager.NEXT_INPUT_END:
                _CanNextInput = false;
                break;
        }
    }

    public override bool IsCanActSkill()
    {
        bool baseAct = base.IsCanActSkill();

        if (!baseAct && _MotionManager.ActingSkill != null)
        {
            if (_MotionManager.ActingSkill._ActInput == "1"
                || _MotionManager.ActingSkill._ActInput == "2"
                || _MotionManager.ActingSkill._ActInput == "3"
                || _MotionManager.ActingSkill._ActInput == "e")
            {
                if (_MotionManager.ActingSkill.CanNextInput)
                {
                    return true;
                }
            }
        }

        return baseAct;
    }

    public override bool ActSkill(Hashtable exHash = null)
    {
        base.ActSkill(exHash);

        _AttackStep = 0;
        _ActSkillInput = "";
        _CanNextInput = false;
        if (exHash != null && exHash.ContainsKey("AttackStep"))
        {
            _AttackStep = (int)exHash["AttackStep"];
            _ActSkillInput = _AttackStep.ToString();
        }
        _SkillProcess = 0;
        ++_SkillActTimes;
        return true;
    }

    public void StartNextInput()
    {
        if (IsCanActAfterBuff)
        {
            InputManager.Instance.SetReuseSkill(_ActSkillInput);
        }
    }
}
