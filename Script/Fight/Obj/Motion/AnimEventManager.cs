using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimEventManager : MonoBehaviour
{
    #region skill event

    public const string NEXT_INPUT_START = "NextInputStart";
    public const string NEXT_INPUT_END = "NextInputEnd";
    public const string COLLIDER_START = "ColliderStart";
    public const string COLLIDER_END = "ColliderEnd";
    public const string KEY_FRAME = "KeyFrame";
    public const string ANIMATION_END = "AnimationEnd";

    public void Init()
    {
        _MotionManager = gameObject.GetComponentInParent<MotionManager>(); 
    }

    public void NextInputStart()
    {
        NotifyAnimEvent(NEXT_INPUT_START);
    }

    public void NextInputEnd()
    {
        NotifyAnimEvent(NEXT_INPUT_END);
    }

    public void ColliderStart(int idx)
    {
        NotifyAnimEvent(COLLIDER_START, idx);
    }

    public void CollidertEnd(int idx)
    {
        NotifyAnimEvent(COLLIDER_END, idx);
    }

    public void KeyFrame()
    {
        NotifyAnimEvent(KEY_FRAME);
    }

    public void AnimationEnd()
    {
        NotifyAnimEvent(ANIMATION_END);
    }

    private MotionManager _MotionManager;
    private void NotifyAnimEvent(string function, object param = null)
    {
        _MotionManager.NotifyAnimEvent(function, param);
    }

    #endregion

    #region specil event

    private Dictionary<string, Action> _AnimCallBack = new Dictionary<string, Action>();

    public void AddEvent(AnimationClip animClip, float animTime, Action callBack)
    {
        string funcName = callBack.Method.ToString();
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = animTime;
        animEvent.functionName = "SpecilEventCallBack";
        animEvent.stringParameter = funcName;

        bool animContainsEvent = false;
        foreach (var selectorEvent in animClip.events)
        {
            if (selectorEvent.time == animEvent.time && funcName == selectorEvent.stringParameter)
            {
                animContainsEvent = true;
                break;
            }
        }

        if (!animContainsEvent)
        {
            animClip.AddEvent(animEvent);
        }

        if (!_AnimCallBack.ContainsKey(funcName))
            _AnimCallBack.Add(funcName, callBack);
    }

    public void AddSelectorEvent(AnimationClip animClip, int frame, int selectorID)
    {
        
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = (float)Math.Round( frame / animClip.frameRate, 2);
        animEvent.functionName = "ColliderStart";
        animEvent.intParameter = selectorID;

        foreach (var selectorEvent in animClip.events)
        {
            if (selectorEvent.time == animEvent.time && selectorID == selectorEvent.intParameter)
                return;
        }

        animClip.AddEvent(animEvent);
    }

    public void AddSelectorFinishEvent(AnimationClip animClip, int frame, int selectorID)
    {
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = frame / animClip.frameRate;
        animEvent.functionName = "CollidertEnd";
        animEvent.intParameter = selectorID;

        foreach (var selectorEvent in animClip.events)
        {
            if (selectorEvent.time == animEvent.time && selectorID == selectorEvent.intParameter)
                return;
        }

        animClip.AddEvent(animEvent);
    }

    public void AddSelectorFinishEvent(AnimationClip animClip, float time, int selectorID)
    {
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = time;
        animEvent.functionName = "CollidertEnd";
        animEvent.intParameter = selectorID;

        foreach (var selectorEvent in animClip.events)
        {
            if (selectorEvent.time == animEvent.time && selectorID == selectorEvent.intParameter)
                return;
        }

        animClip.AddEvent(animEvent);
    }

    public void RemoveSelectorEvent(AnimationClip animClip, int selectorID)
    {
        List<AnimationEvent> removeEvents = new List<AnimationEvent>();
        foreach (var animEvent in animClip.events)
        {
            if (animEvent.functionName == "ColliderStart"
                && selectorID == animEvent.intParameter)
                animEvent.functionName = ""; ;
        }

    }

    public float GetAnimFirstColliderEventTime(AnimationClip animClip, int colliderID)
    {
        foreach (var animEvent in animClip.events)
        {
            if (animEvent.functionName == "ColliderStart")
            {
                if (colliderID < 0)
                {
                    return animEvent.time;
                }
                else if(colliderID == animEvent.intParameter)
                {
                    return animEvent.time;
                }
            }
        }

        return -1;
    }

    public void SpecilEventCallBack(string strParam)
    {
        if (_AnimCallBack.ContainsKey(strParam))
        {
            _AnimCallBack[strParam].Invoke();
        }
    }

    #endregion
}
