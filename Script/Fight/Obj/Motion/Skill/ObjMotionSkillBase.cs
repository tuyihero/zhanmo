using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBase : MonoBehaviour
{
    public virtual void Init()
    {
        _MotionManager = gameObject.GetComponentInParent<MotionManager>();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        //_SkillAttr = _MotionManager.RoleAttrManager.GetSkillAttr(_ActInput);
        InitCollider();
        //if (_SkillAttr != null)
        //{
        //    SetEffectSize(1 + _SkillAttr.RangeAdd);
        //}

        foreach (var anim in _NextAnim)
        {
            if (anim != null)
            {
                _MotionManager.InitAnimation(anim);
                _MotionManager.AddAnimationEndEvent(anim);
            }
        }

        RegisterColliderEvent();

        this.enabled = false;
    }

    protected int _CurStep = -1;
    public int CurStep
    {
        get
        {
            return _CurStep;
        }
    }
    public List<AnimationClip> _NextAnim;
    public List<EffectController> _NextEffect;
    public List<AudioClip> _NextAudio;
    public string _ActInput;
    public int _SkillMotionPrior = 100;
    public bool _SkillMotionPriorSelf = false;
    public int _SuperArmorColliderID = -1;
    public bool _ActSkillInJump = false;
    public bool _ActStayInJump = true;

    public float _SkillBaseSpeed = 1;
    public float SkillSpeed
    {
        get
        {
            return (SkillActSpeed) * _SkillBaseSpeed;
        }
    }

    protected bool _CanNextInput = false;
    public bool CanNextInput
    {
        get
        {
            return _CanNextInput;
        }
    }

    protected MotionManager _MotionManager;
    public MotionManager MotionManager
    {
        get
        {
            return _MotionManager;
        }
    }

    public AnimationClip GetCurAnim()
    {
        if (_CurStep < 0 || _CurStep >= _NextAnim.Count)
            return null;

        return _NextAnim[_CurStep];
    }

    public virtual bool IsCanActSkill()
    {
        if (_MotionManager._ActionState == _MotionManager._StateIdle)
            return true;

        if (_MotionManager._ActionState == _MotionManager._StateMove)
            return true;

        if ((_MotionManager._ActionState == _MotionManager._StateJump || _MotionManager._ActionState == _MotionManager._StateJumpIdle) && _ActSkillInJump)
        {
            if (_ActStayInJump)
            {
                _MotionManager.SetJumpStay();
            }
            return true;
        }

        if (_MotionManager._ActionState == _MotionManager._StateSkill)
        {
            if (_MotionManager.ActingSkill == null)
                return true;
            if (_MotionManager.ActingSkill._SkillMotionPrior < _SkillMotionPrior)
            {
                return true;
            }
            if (_SkillMotionPriorSelf && CanNextInput)
                return true;
        }

        return false;
    }

    public bool StartSkill(Hashtable exHash = null)
    {
        if (!IsCanActSkill())
            return false;

        return ActSkill(exHash);
    }


    public virtual void PauseSkill()
    { }

    public virtual void ResumeSkill()
    { }

    public virtual void FinishSkill()
    {
        _MotionManager.FinishSkill(this);
    }

    public virtual void FinishSkillImmediately()
    {
        foreach (var effect in _NextEffect)
        {
            _MotionManager.StopSkillEffect(effect);
        }

        this.enabled = false;
        _MotionManager.ResetMove();
        ColliderStop();

        MotionManager.RoleAttrManager.AccumulateDamageRate = 0;
    }

    public virtual void AnimEvent(string function, object param)
    {
        switch (function)
        {
            case AnimEventManager.ANIMATION_END:
                PlayerNextAnim();
                break;
            case AnimEventManager.COLLIDER_START:
                ColliderStart(param);
                break;
            case AnimEventManager.COLLIDER_END:
                ColliderEnd(param);
                break;
            case AnimEventManager.NEXT_INPUT_START:
                FightSkillManager.Instance.SkillNextInput(this);
                _CanNextInput = true;
                break;
        }
    }

    public int _SkillActTimes = 0;
    public virtual bool ActSkill(Hashtable exhash)
    {
        this.enabled = true;

        _CurStep = -1;
        PlayerNextAnim();

        MotionManager.BuffAttack();

        _LastUseTime = Time.time;
        ++_SkillActTimes;

        _CanNextInput = false;

        return true;
    }

    public void PlayerNextAnim()
    {
        if (_CurStep + 1 == _NextAnim.Count)
        {
            FinishSkill();
        }
        else
        {
            ++_CurStep;
            PlayAnimation(_NextAnim[_CurStep]);

            if (_CurStep - 1 >= 0 && _NextEffect[_CurStep - 1] != null)
            {
                StopSkillEffect(_NextEffect[_CurStep - 1]);
            }

            if (_NextEffect.Count > _CurStep && _NextEffect[_CurStep] != null)
            {
                PlaySkillEffect(_NextEffect[_CurStep]);
            }

            if (_NextAudio.Count > _CurStep && _NextAudio[_CurStep] != null)
            {
                PlayAudio(_NextAudio[_CurStep]);
            }
            

        }
    }

    #region performance

    protected float _SkillAddSpeed = 0;
    public float SkillAddSpeed
    {
        get
        {
            return _SkillAddSpeed;
        }
        set
        {
            _SkillAddSpeed = value;
        }
    }

    protected float _SkillActSpeed = -1;
    public float SkillActSpeed
    {
        get
        {
            //if (_SkillActSpeed < 0)
            {
                if (_MotionManager.RoleAttrManager == null)
                {
                    _SkillActSpeed = 0;
                }
                else
                {
                    _SkillActSpeed = _MotionManager.RoleAttrManager.AttackSpeed;
                }
                _SkillActSpeed += _SkillAddSpeed;
            }
            return _SkillActSpeed;
        }
    }

    public virtual void SetEffectSize(float size)
    {
        foreach (var effect in _NextEffect)
        {
            if (effect == null)
                continue;

            effect._EffectSizeRate = (size);
        }
    }

    protected void PlayAnimation(AnimationClip anim)
    {
        _MotionManager.RePlayAnimation(anim, (SkillActSpeed) * _SkillBaseSpeed);
    }

    protected void PlaySkillEffect(EffectController effect)
    {
        _MotionManager.PlaySkillEffect(effect, (SkillActSpeed) * _SkillBaseSpeed);
    }

    protected void PlayAudio(AudioClip audioClip)
    {
        _MotionManager.PlayAudio(audioClip);
    }

    protected void StopSkillEffect(EffectController effect)
    {
        _MotionManager.StopSkillEffect(effect);
    }

    public virtual float GetTotalAnimLength()
    {
        float totleTime = 0;
        foreach (var anim in _NextAnim)
        {
            totleTime += anim.length / SkillActSpeed;
        }
        return totleTime;
    }

    protected float GetAnimNextInputLength(AnimationClip anim)
    {
        foreach (var animEvent in anim.events)
        {
            if (animEvent.functionName == AnimEventManager.NEXT_INPUT_START)
            {
                return animEvent.time / SkillActSpeed;
            }
        }
        return anim.length;
    }



    #endregion

    #region collider 

    public List<MotionManager> _SkillHitMotions = new List<MotionManager>();

    private Dictionary<int, List<SelectBase>> _ColliderControl = new Dictionary<int, List<SelectBase>>();

    protected void InitCollider()
    {
        var collidercontrollers = gameObject.GetComponentsInChildren<SelectBase>(true);
        foreach (var collider in collidercontrollers)
        {
            collider.Init();
            if (_ColliderControl.ContainsKey(collider._ColliderID))
            {
                _ColliderControl[collider._ColliderID].Add(collider);
            }
            else
            {
                _ColliderControl.Add(collider._ColliderID, new List<SelectBase>());
                _ColliderControl[collider._ColliderID].Add(collider);
            }
        }
    }

    public void ModifyColliderRange(float rangeModify)
    {
        foreach (var colliderKeyValue in _ColliderControl)
        {
            foreach (var collider in colliderKeyValue.Value)
            {
                collider.ModifyColliderRange(rangeModify);
            }
        }
    }

    protected void RegisterColliderEvent()
    {
        foreach (var colliderKeyValue in _ColliderControl)
        {
            foreach(var collider in colliderKeyValue.Value)
                collider.RegisterEvent();
        }
    }

    protected virtual void ColliderStart(object param)
    {
        int index = (int)param;
        //Debug.Log("ColliderStart:" + index);
        if (_ColliderControl != null && _ColliderControl.ContainsKey(index))
        {
            for (int i = 0; i< _ColliderControl[index].Count; ++i)
            {
                _ColliderControl[index][i].ColliderStart();
            }
        }

    }

    protected virtual void ColliderEnd(object param)
    {
        int index = (int)param;
        if (_ColliderControl != null && _ColliderControl.ContainsKey(index))
        {
            for (int i = 0; i < _ColliderControl[index].Count; ++i)
            {
                _ColliderControl[index][i].ColliderFinish();
            }
            
        }
    }

    protected virtual void ColliderStop()
    {
        foreach(var colliderPair in _ColliderControl)
        {
            for (int i = 0; i < colliderPair.Value.Count; ++i)
            {
                colliderPair.Value[i].ColliderStop();
            }

        }
    }

    #endregion

    #region cd for skill bar

    public float _SkillCD = 0;
    protected float _LastUseTime;
    public float LastUseTime
    {
        get
        {
            return _LastUseTime;
        }
        set
        {
            _LastUseTime = value;
        }
    }

    public int _StoreUseTimes = 0;
    protected float _LastStoreTime = 0;
    protected int _LastUseTimes = 0;
    public int LastUseTimes
    {
        get
        {
            return _LastUseTimes;
        }
    }

    #endregion



}
