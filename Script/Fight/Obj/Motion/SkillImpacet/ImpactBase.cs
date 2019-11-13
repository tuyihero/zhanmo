using UnityEngine;
using System.Collections;

public class ImpactBase : MonoBehaviour
{
    protected ObjMotionSkillBase _SkillMotion;
    public ObjMotionSkillBase SkillMotion
    {
        get
        {
            return _SkillMotion;
        }  
    }

    protected MotionManager _SenderMotion;
    public MotionManager SenderMotion
    {
        get
        {
            return _SenderMotion;
        }
    }

    protected MotionManager _ReciveMotion;
    public MotionManager ReciveMotion
    {
        get
        {
            return _ReciveMotion;
        }
    }

    protected SelectBase _Selector;
    public SelectBase Selector
    {
        get
        {
            return _Selector;
        }
    }

    protected bool _IsActingImpact = false;

    public virtual void Init(ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        _IsActingImpact = false;
        _SkillMotion = skillMotion;
        _Selector = selector;
    }

    public virtual void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        _SenderMotion = senderManager;
        _ReciveMotion = reciverManager;
        _IsActingImpact = true;
    }

    public virtual void FinishImpact(MotionManager reciverManager)
    {
        _IsActingImpact = false;
    }

    public virtual void StopImpact()
    {

    }

}
