using UnityEngine;
using System.Collections;

public class ObjMotionSkillAttack : ObjMotionSkillBase
{
    void Update()
    {
        if (_CanNextInput)
        {
            if (InputManager.Instance.IsKeyDown(_ActInput))
            {
                ContinueAttack();
            }
        }
    }

    #region override

    public override void Init()
    {
        base.Init();

        foreach (var anim in _NextAnim)
        {
            if (anim != null)
            {
                _MotionManager.InitAnimation(anim);
                _MotionManager.AddAnimationEndEvent(anim);
            }
        }
    }

    public override void SetEffectSize(float size)
    {
        foreach (var effect in _NextEffect)
        {
            effect._EffectSizeRate = (size);
        }
    }

    public override void AnimEvent(string function, object param)
    {
        //base.AnimEvent(function, param);

        switch (function)
        {
            case AnimEventManager.NEXT_INPUT_START:
                _CanNextInput = true;
                NextInputPress();
                break;
            case AnimEventManager.NEXT_INPUT_END:
                _CanNextInput = false;
                break;
            case AnimEventManager.ANIMATION_END:
                //PlayerNextAnim();
                FinishSkill();
                break;
            case AnimEventManager.COLLIDER_START:
                ColliderStart(param);
                break;
            case AnimEventManager.COLLIDER_END:
                ColliderEnd(param);
                break;
        }
    }

    public override void FinishSkillImmediately()
    {
        base.FinishSkillImmediately();

        _CurStep = -1;
        foreach (var effect in _NextEffect)
        {
            if (effect != null)
            {
                _MotionManager.StopSkillEffect(effect);
            }
        }

        if (InputManager.Instance.IsKeyHold(_ActInput))
        {
            StartCoroutine(ReStartSkill());
            //ActSkill();
        }

    }

    private IEnumerator ReStartSkill()
    {
        yield return new WaitForFixedUpdate();
        _MotionManager.ActSkill(this);
    }

    #endregion

    public SelectBase[] _Collider;

    public override bool ActSkill(Hashtable exhash)
    {
        bool isActSkill = base.ActSkill(exhash);
        if (!isActSkill)
            return false;

        _CanNextInput = false;
        _CurStep = -1;
        ContinueAttack();
        ++_SkillActTimes;
        FightSkillManager.Instance.ResetReuseSkill();
        FightSkillManager.Instance.ResetLastUseSkill();

        return true;
    }

    public void NextInputPress()
    {
        if (InputManager.Instance.IsKeyHold(_ActInput))
        {
            ContinueAttack();
        }

    }

    public void ContinueAttack()
    {
        if (_CurStep + 1 < _NextAnim.Count)
        {
            ++_CurStep;
            //if (InputManager.Instance._Axis != Vector2.zero)
            //{
            //    _MotionManager.SetRotate(new Vector3(InputManager.Instance._Axis.x, 0, InputManager.Instance._Axis.y));
            //}
            //InputManager.Instance.SetRotate();
            PlayAnimation(_NextAnim[_CurStep]);
            if (_NextEffect.Count > _CurStep && _NextEffect[_CurStep] != null)
            {
                PlaySkillEffect(_NextEffect[_CurStep]);
            }
            if (_NextAudio.Count > _CurStep && _NextAudio[_CurStep] != null)
            {
                PlayAudio(_NextAudio[_CurStep]);
            }

            if (_CurStep - 1 >= 0 && _NextEffect.Count > _CurStep  && _NextEffect[_CurStep - 1] != null)
            {
                StopSkillEffect(_NextEffect[_CurStep - 1]);
            }

            _CanNextInput = false;
        }
    }

    public void DushAttack()
    {
        _CanNextInput = false;
        _CurStep = 0;
        ContinueAttack();
    }

}
