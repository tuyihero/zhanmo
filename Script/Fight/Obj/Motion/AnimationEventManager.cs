using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationEventManager : MonoBehaviour
{
    private MotionManager _MotionManager;
    private Animation _Animaton;
    public Animation Animation
    {
        get
        {
            return _Animaton;
        }
    }

    public void Init()
    {
        _Animaton = gameObject.GetComponentInChildren<Animation>();
        _MotionManager = gameObject.GetComponentInParent<MotionManager>(); 
    }

    void FixedUpdate()
    {
        AnimUpdate();
    }

    #region add event

    public delegate void AnimEventCallBack(Hashtable param);
    public class AnimEvent
    {
        public float time;
        public AnimEventCallBack animCallBack;
        public Hashtable param;
    }

    private Dictionary<string, List<AnimEvent>> _AnimationEvents = new Dictionary<string, List<AnimEvent>>();

    public void AddAnimEvent(AnimationClip anim, float time, AnimEventCallBack animCallBack, Hashtable param)
    {
        AnimEvent animEvent = new AnimEvent();
        animEvent.time = time;
        animEvent.animCallBack = animCallBack;
        animEvent.param = param;

        if (!_AnimationEvents.ContainsKey(anim.name))
        {
            _AnimationEvents.Add(anim.name, new List<AnimEvent>());
        }
        _AnimationEvents[anim.name].Add(animEvent);
        _AnimationEvents[anim.name].Sort((animEvent1, animEvent2) =>
        {
            if (animEvent1.time > animEvent2.time)
                return 1;
            else if (animEvent1.time < animEvent2.time)
                return -1;
            else
                return 0;
        });
    }

    public void AddAnimStartEvent(AnimationClip anim, AnimEventCallBack animCallBack, Hashtable param)
    {
        AddAnimEvent(anim, 0, animCallBack, param);
    }

    public void AddAnimEndEvent(AnimationClip anim, AnimEventCallBack animCallBack, Hashtable param)
    {
        AddAnimEvent(anim, anim.length, animCallBack, param);
    }

    #endregion

    #region playAnim

    private string _PlayingAnim;
    private float _PlayingSpeed;

    public void InitAnimation(AnimationClip animClip)
    {
        _Animaton.AddClip(animClip, animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip)
    {
        _PlayingSpeed = 1;
        _PlayingAnim = animClip.name;
        StartPlayAnim();
        _Animaton.Play(animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip, float speed)
    {
        _PlayingSpeed = speed;
        _PlayingAnim = animClip.name;
        StartPlayAnim();
        _Animaton[animClip.name].speed = speed;
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip, float speed)
    {
        _PlayingSpeed = speed;
        _PlayingAnim = animClip.name;
        StartPlayAnim();
        _Animaton[animClip.name].speed = speed;
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip)
    {
        _PlayingSpeed = 1;
        _PlayingAnim = animClip.name;
        StartPlayAnim();
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    float _OrgSpeed = 1;
    public void PauseAnimation(AnimationClip animClip)
    {
        _OrgSpeed = _Animaton[animClip.name].speed;
        _Animaton[animClip.name].speed = 0;
    }

    public void PauseAnimation()
    {
        foreach (AnimationState state in _Animaton)
        {
            if (_Animaton.IsPlaying(state.name))
            {
                _OrgSpeed = state.speed;
                state.speed = 0;
            }
        }

    }

    public void ResumeAnimation(AnimationClip animClip)
    {
        if (_Animaton.IsPlaying(animClip.name))
        {
            _Animaton[animClip.name].speed = _OrgSpeed;
        }
    }

    public void ResumeAnimation()
    {
        foreach (AnimationState state in _Animaton)
        {
            if (_Animaton.IsPlaying(state.name))
            {
                state.speed = _OrgSpeed;
            }
        }
    }

    #endregion

    #region call event

    private AnimEvent _NextEvent;
    private float _AnimPlayedTime;

    private void AnimUpdate()
    {
        if (_NextEvent == null)
            return;

        if (!_AnimationEvents.ContainsKey(_PlayingAnim))
            return;

        _AnimPlayedTime += Time.fixedDeltaTime * _PlayingSpeed;
        if (_NextEvent.time < _AnimPlayedTime)
        {
            _NextEvent.animCallBack(_NextEvent.param);
            if (_AnimPlayedTime == 0)
                return;
            int idx = _AnimationEvents[_PlayingAnim].IndexOf(_NextEvent) + 1;
            if (idx < _AnimationEvents[_PlayingAnim].Count)
            {
                _NextEvent = _AnimationEvents[_PlayingAnim][idx + 1];
            }
            else
            {
                _NextEvent = null;
            }

        }
    }

    private void StartPlayAnim()
    {
        _AnimPlayedTime = 0;
        if (_AnimationEvents.ContainsKey(_PlayingAnim))
        {
            _NextEvent = _AnimationEvents[_PlayingAnim][0];
        }
    }

    #endregion

}
