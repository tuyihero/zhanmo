using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBase : MonoBehaviour
{
    public int _ColliderID;
    public bool _IsColliderFinish = false;
    public bool _IsRemindSelected = false;
    public AnimationClip _EventAnim;
    public List<int> _EventFrame;
    public int _AudioID = -1;
    
    protected MotionManager _ObjMotion;
    protected ObjMotionSkillBase _SkillMotion;
    protected ImpactBase[] _ImpactList;

    public virtual void Init()
    {
        if (_ColliderID < 1000)
        {
            gameObject.SetActive(false);
        }
        _SkillMotion = gameObject.GetComponentInParent<ObjMotionSkillBase>();
        _ObjMotion = gameObject.GetComponentInParent<MotionManager>();
        _ImpactList = gameObject.GetComponents<ImpactBase>();
        foreach (var impactBase in _ImpactList)
        {
            impactBase.Init(_SkillMotion, this);
        }
    }

    public virtual void ModifyColliderRange(float rangeModify)
    {

    }

    public virtual void RegisterEvent()
    {
        for (int i = 0; i < _EventFrame.Count; ++i)
        {
            var anim = _ObjMotion.GetAnimClip(_EventAnim.name);
            if (anim != null)
            {
                _ObjMotion.AnimationEvent.AddSelectorEvent(anim, _EventFrame[i], _ColliderID);
            }
        }
    }

    public virtual void ResetSelector()
    {
        _ObjMotion.AnimationEvent.RemoveSelectorEvent(_EventAnim, _ColliderID);
    }
    

    public virtual void ColliderStart()
    {
        gameObject.SetActive(true);

        
        if (!_IsColliderFinish)
        {
            StartCoroutine(AutoFinish());
        }

        if (_AudioID > 0)
        {
            _ObjMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_AudioID]);
        }
    }

    public IEnumerator AutoFinish()
    {
        yield return new WaitForSeconds(0.1f);

        ColliderFinish();
    }

    public virtual void ColliderFinish()
    {
        //gameObject.SetActive(false);
    }

    public virtual void ColliderStop()
    {
        foreach (var impact in _ImpactList)
        {
            impact.StopImpact();
        }
    }

}
