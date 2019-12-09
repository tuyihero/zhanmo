using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCollider : SelectBase
{

    public List<int> _ColliderFinishFrame;

    protected List<MotionManager> _TrigMotions= new List<MotionManager>();
    public List<MotionManager> TrigMotions
    {
        get
        {
            return _TrigMotions;
        }
    }

    protected Collider _Collider;
    public Collider Collider
    {
        get
        {
            return _Collider;
        }
    }

    public bool _SelectLieObj;

    public int _HittedAudio = -1;
    public int _NonHittedAudio = -1;
    private bool _IsPlayHitAudio = false;
    private bool _IsPlayedNonHitAudio = false;
    private bool _HittedFlag = false;

    public override void Init()
    {
        base.Init();
        
    }

    public override void ModifyColliderRange(float rangeModify)
    {
        base.ModifyColliderRange(rangeModify);

        _Collider = gameObject.GetComponent<Collider>();
    }

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        //for (int i = 0; i < _ColliderFinishFrame.Count; ++i)
        //{
        //    var anim = _ObjMotion.Animation.GetClip(_EventAnim.name);
        //    _ObjMotion.AnimationEvent.AddSelectorFinishEvent(anim, _ColliderFinishFrame[i], _ColliderID);
        //}
    }

    public override void ColliderStart()
    {
        if (_Collider == null)
        {
            _Collider = gameObject.GetComponent<Collider>();
        }
        _Collider.enabled = true;
        base.ColliderStart();
        _IsPlayedNonHitAudio = false;
        _IsPlayHitAudio = false;
        _HittedFlag = false;
        //Debug.Log("ColliderStart:" + _ColliderID);
    }

    public override void ColliderFinish()
    {
        base.ColliderFinish();

        if (!_IsPlayedNonHitAudio && _TrigMotions.Count == 0)
        {
            if (_NonHittedAudio > 0)
            {
                _ObjMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_NonHittedAudio]);
                _IsPlayedNonHitAudio = true;
            }
        }
        else if (_TrigMotions.Count != 0)
        {
            _IsPlayedNonHitAudio = true;
            foreach (var trigMotion in _TrigMotions)
            {
                if (trigMotion.IsContainsBuff(typeof(ImpactBuffSuperArmor)))
                {

                    _ObjMotion.PlayAudio(ResourcePool.Instance._CommonAudio[ResourcePool.Instance._HitSuperArmor]);
                }
                else
                {
                    if (_HittedAudio > 0)
                    {
                        _ObjMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_HittedAudio]);
                    }
                }
            }
        }

        ImpactHit hitImpact = null;
        foreach (var impact in _ImpactList)
        {
            if (impact is ImpactHit)
            {
                hitImpact = impact as ImpactHit;
                break;
            }
        }

        if (_TrigMotions.Count != 0)
        {
            _ObjMotion.BuffHitEnemy(hitImpact, _TrigMotions);
        }
        
        

        if (_Collider != null)
            _Collider.enabled = false;
        _TrigMotions.Clear();
    }

    public override void ColliderStop()
    {
        ColliderFinish();
    }

    void OnTriggerStay(Collider other)
    {
        var motion = other.gameObject.GetComponentInParent<MotionManager>();
        if (motion == null)
            return;

        TriggerMotion(motion);

        //Debug.Log("OnTriggerStay:" + _ColliderID);
    }

    protected virtual void TriggerMotion(MotionManager motion)
    {
        if (!_TrigMotions.Contains(motion))
        {
            _TrigMotions.Add(motion);

            if (motion._StateLie == motion._ActionState && !_SelectLieObj)
                return;

            if (!motion._CanBeSelectByEnemy)
                return;
            
            {
                foreach (var impact in _ImpactList)
                {
                    impact.ActImpact(_ObjMotion, motion);
                }

                if (!_HittedFlag)
                {
                    _HittedFlag = true;
                    _ObjMotion.BuffHitEnemy();
                }

                if (_IsRemindSelected && _ObjMotion.ActingSkill != null)
                {
                    if (!_ObjMotion.ActingSkill._SkillHitMotions.Contains(motion))
                        _ObjMotion.ActingSkill._SkillHitMotions.Add(motion);
                }

                if (_ObjMotion._IsRoleHit)
                {
                    GlobalEffect.Instance.Pause(_ObjMotion._RoleHitTime);
                }
            }
        }
    }

}
