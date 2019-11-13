using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuff : ImpactBase
{
    #region buff critic

    [System.Serializable]
    public enum CRITIC_TYPE
    {
        None = 0,
        Refresh,
    }

    public static int _StaticBuffID = 0;
    public static int GetBuffID()
    {
        return ++_StaticBuffID;
    }

    protected int _BuffID;
    public int BuffID
    {
        get
        {
            return _BuffID;
        }
    }

    public bool _IsBuffCriticID = false;
    public bool _IsBuffCriticClass = false;

    public CRITIC_TYPE _CriticType = CRITIC_TYPE.Refresh;

    #endregion

    #region buff type

    public enum BuffType
    {
        Buff,
        Debuff
    }

    public BuffType _BuffType = BuffType.Buff;

    #endregion

    public float _LastTime;

    protected float _ExLastTime = 0;
    public float ExLastTime
    {
        get
        {
            return _ExLastTime;
        }
        set
        {
            _ExLastTime = value;
        }
    }

    public EffectController _BuffEffect;

    protected MotionManager _BuffSender;
    protected MotionManager _BuffOwner;
    protected int _DynamicEffect;
    protected bool _IsActingBuff = false;

    protected Dictionary<MotionManager, List<ImpactBuff>> _ReciverDict = new Dictionary<MotionManager, List<ImpactBuff>>();

    public void Awake()
    {
        enabled = false;
    }

    public void Update()
    {
        if (_IsActingBuff)
        {
            UpdateBuff();
        }
    }

    public override void Init(ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        base.Init(skillMotion, selector);

        _BuffID = GetBuffID();
    }

    public override sealed void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        _BuffSender = senderManager;
        _BuffOwner = reciverManager;
        var dynamicBuff = reciverManager.AddBuff(this);
    }

    public ImpactBuff ActBuffInstance(MotionManager senderManager, MotionManager reciverManager, float lastTime = 0)
    {
        base.ActImpact(senderManager, reciverManager);

        if (_BuffID == 0)
        {
            _BuffID = GetBuffID();
        }
        _BuffSender = senderManager;
        _BuffOwner = reciverManager;
        var dynamicBuff = reciverManager.AddBuff(this, lastTime + ExLastTime);
        return dynamicBuff;
    }

    public override void FinishImpact(MotionManager reciverManager)
    {
        base.FinishImpact(reciverManager);

        reciverManager.RemoveBuff(GetType());
    }

    public void ActBuff(MotionManager reciverManager)
    {
        if (reciverManager.IsMotionDie)
            return;

        ActBuff(_BuffSender, reciverManager);
    }

    public virtual void ActBuff(MotionManager senderManager, MotionManager ownerManager)
    {
        if (_BuffEffect != null)
        {
            _DynamicEffect = ownerManager.PlayDynamicEffect(_BuffEffect);
        }
        if(_LastTime > 0)
            StartCoroutine(TimeOut(ownerManager));

        this.enabled = true;

        _IsActingBuff = true;
    }

    public virtual bool IsCanAddBuff(ImpactBuff newBuff)
    {
        bool canAdd = true;
        if (_IsBuffCriticID)
        {
            canAdd = canAdd & (BuffID != newBuff.BuffID);
            
        }

        if (_IsBuffCriticClass)
        {
            canAdd = canAdd & (newBuff.GetType() != this.GetType());
        }

        if (!canAdd && _CriticType == CRITIC_TYPE.Refresh)
        {
            _LastTime = newBuff._LastTime;
            RefreshLastTime();
        }

        return canAdd;
    }

    public virtual void RemoveBuff(MotionManager ownerManager)
    {
        if (_DynamicEffect != null)
        {
            ownerManager.StopDynamicEffectImmediately(_DynamicEffect);
        }
    }

    public virtual void UpdateBuff()
    {

    }

    public void RefreshLastTime()
    {
        StopAllCoroutines();
        StartCoroutine(TimeOut(_BuffOwner));
    }

    public IEnumerator TimeOut(MotionManager ownerManager)
    {
        yield return new WaitForSeconds(_LastTime + ExLastTime);
        ownerManager.RemoveBuff(this);
    }

    #region 

    public enum BuffModifyType
    {
        IsCanHit = 1,
        IsCanCatch,
        DamageValue,
    }

    public virtual void BuffModify(BuffModifyType type, params object[] args)
    {

    }

    public virtual bool IsBuffCanHit(MotionManager impactSender, ImpactHit damageImpact)
    {
        return true;
    }

    public virtual void BeHit(MotionManager hitSender, ImpactHit hitImpact)
    {

    }

    public virtual void HitEnemy()
    {

    }

    public virtual void HitEnemy(ImpactHit hitImpact, List<MotionManager> hittedMotions)
    {

    }

    public virtual void Attack()
    {

    }

    public virtual bool IsBuffCanCatch(ImpactCatch damageImpact)
    {
        return true;
    }

    public virtual bool IsBuffCanDie()
    {
        return true;
    }

    public virtual void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        
    }

    public virtual void CastDamage(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {

    }

    #endregion
}
