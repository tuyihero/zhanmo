﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.AI;
using UnityEngine.Profiling;

public class MotionManager : MonoBehaviour
{
    public void InitMotion()
    {
        gameObject.SetActive(true);

        InitRoleAttr();

        var bodyGO = transform.Find("Body/Body");
        _Animaton = bodyGO.GetComponent<Animator>();
        var bodyEffect = transform.Find("Body/Body/Effect");
        _EffectAnimator = bodyEffect.GetComponent<Animator>();
        var weapon = transform.Find("Body/Weapon/Weapon");
        _WeaponAnimator = weapon.GetComponent<Animator>();
        var weaponEffect = transform.Find("Body/Weapon/Weapon/Effect");
        _WeaponEffectAnimator = weaponEffect.GetComponent<Animator>();

        _AnimationEvent = _Animaton.gameObject.GetComponent<AnimEventManager>();
        if (_AnimationEvent != null)
        {
            GameObject.DestroyImmediate(_AnimationEvent);
        }
        _AnimationEvent = _Animaton.gameObject.AddComponent<AnimEventManager>();
        _AnimationEvent.Init();

        if (TriggerCollider != null)
        {
            TriggerCollider.enabled = true;
        }
        _CanBeSelectByEnemy = true;

        InitAudio();

        InitState();
    }

    void FixedUpdate()
    {
        UpdateMove();

        if (_ActionState != null)
        {
            _ActionState.StateUpdate();
        }
    }

    public void Reset()
    {
        _IsMotionDie = false;
    }

    #region motion

    public bool _IsRoleHit = false;
    public float _RoleHitTime = 0.01f;

    private int _MotionPrior;
    public int MotionPrior
    {
        get
        {
            return _MotionPrior;
        }
        set
        {
            _MotionPrior = value;
        }
    }

    //private BaseMotionManager _BaseMotionManager;
    //public BaseMotionManager BaseMotionManager
    //{
    //    get
    //    {
    //        return _BaseMotionManager;
    //    }
    //}

    //public void NotifyAnimEvent(string function, object param)
    //{
    //    if (ActingSkill != null)
    //    {
    //        ActingSkill.AnimEvent(function, param);
    //    }
    //    else
    //    {
    //        _BaseMotionManager.DispatchAnimEvent(function, param);
    //    }
    //}
    #endregion

    #region Animation

    public Animator _Animaton;
    public Animator Animation
    {
        get
        {
            return _Animaton;
        }  
    }
    public Animator _EffectAnimator;
    public Animator _WeaponAnimator;
    public Animator _WeaponEffectAnimator;

    private AnimEventManager _AnimationEvent;
    public AnimEventManager AnimationEvent
    {
        get
        {
            return _AnimationEvent;
        }
    }

    public void InitAnimation(AnimationClip animClip)
    {
        //List<AnimationEvent> eveneList = new List<UnityEngine.AnimationEvent>();
        //foreach (var animEvent in animClip.events)
        //{
        //    if (animEvent.functionName == "ColliderStart" && animEvent.intParameter >= 1000 && animEvent.intParameter < 10000)
        //    { }
        //    else if (animEvent.functionName == "CollidertEnd" && animEvent.intParameter >= 1000 && animEvent.intParameter < 10000)
        //    { }
        //    else
        //    {
        //        eveneList.Add(animEvent);
        //    }
        //}
        //animClip.events = eveneList.ToArray();

        //_Animaton.AddClip(animClip, animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = RoleAttrManager.AttackSpeed;
        Debug.Log("PlayAnimation:" + animClip.name);
        _Animaton.Play(animClip.name);
        if (_EffectAnimator != null)
        {
            _EffectAnimator.Play(animClip.name);
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.Play(animClip.name);
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.Play(animClip.name);
        }
    }

    public void PlayAnimation(AnimationClip animClip, float speed)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = speed;
        _Animaton.Play(animClip.name);

        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = speed;
            _EffectAnimator.Play(animClip.name);
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = speed;
            _WeaponAnimator.Play(animClip.name);
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = speed;
            _WeaponEffectAnimator.Play(animClip.name);
        }
    }

    public void RePlayAnimation(AnimationClip animClip, float speed)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = speed;
        //_Animaton.Stop();
        _Animaton.Play(animClip.name);

        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = speed;
            _EffectAnimator.Play(animClip.name);
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = speed;
            _WeaponAnimator.Play(animClip.name);
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = speed;
            _WeaponEffectAnimator.Play(animClip.name);
        }
    }

    public void RePlayAnimation(AnimationClip animClip)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = RoleAttrManager.AttackSpeed;
        _Animaton.Play(animClip.name);

        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = _Animaton.speed;
            _EffectAnimator.Play(animClip.name);
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = _Animaton.speed;
            _WeaponAnimator.Play(animClip.name);
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = _Animaton.speed;
            _WeaponEffectAnimator.Play(animClip.name);
        }
    }

    float _OrgSpeed = 1;
    float _OrgEffectSpeed = 1;
    int _PauseCnt = 0;
    public void PauseAnimation(AnimationClip animClip, float lastTime)
    {
        if (_Animaton.speed == 0)
        {
            ++_PauseCnt;
            if (lastTime > 0)
                StartCoroutine(ResumeAnimationLater(animClip, lastTime));
            return;
        }
        else
        {
            _PauseCnt = 1;
        }

        _OrgSpeed = _Animaton.speed;
        _Animaton.speed = 0;
        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = _Animaton.speed;
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = _Animaton.speed;
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = _Animaton.speed;
        }

        if (_PlayingEffect != null)
        {
            _OrgEffectSpeed = _PlayingEffect.LastPlaySpeed;
            _PlayingEffect.SetEffectSpeed(0);
        }

        if(lastTime > 0)
            StartCoroutine(ResumeAnimationLater(animClip, lastTime));
    }

    public void PauseAnimation()
    {
        _Animaton.StopPlayback();
        if (_EffectAnimator != null)
        {
            _EffectAnimator.StopPlayback();
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.StopPlayback();
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.StopPlayback();
        }
        //foreach (AnimationState state in _Animaton)
        //{
        //    if (_Animaton.IsPlaying(state.name))
        //    {
        //        PauseAnimation(state.clip, -1);
        //    }
        //}

    }

    public AnimationState GetCurAnim()
    {
        //foreach (AnimationState state in _Animaton)
        //{
        //    if (_Animaton.IsPlaying(state.name))
        //    {
        //        return state;
        //    }
        //}

        return null;
    }

    public void ResumeAnimation(AnimationClip animClip)
    {
        --_PauseCnt;
        if (_PauseCnt > 0)
            return;

        //if (_Animaton.IsPlaying(animClip.name))
        {
            _Animaton.speed = _OrgSpeed;
        }

        if (_PlayingEffect != null)
        {
            _PlayingEffect.SetEffectSpeed(_OrgEffectSpeed);
        }
    }

    public void ResumeAnimation()
    {
        //foreach (AnimationState state in _Animaton)
        {
            //if (_Animaton.IsPlaying(state.name))
            {
                ResumeAnimation(null);
            }
        }
    }

    private IEnumerator ResumeAnimationLater(AnimationClip animClip, float lasterTime)
    {
        yield return new WaitForSeconds(lasterTime);
        if (animClip == null)
        {
            ResumeAnimation();
        }
        else
        {
            ResumeAnimation(animClip);
        }
    }

    public void AddAnimationEndEvent(AnimationClip animClip)
    {
        if (animClip == null)
            return;

        UnityEngine.AnimationEvent animEvent = new UnityEngine.AnimationEvent();
        animEvent.time = animClip.length;
        animEvent.functionName = "AnimationEnd";

        foreach (var selectorEvent in animClip.events)
        {
            if (selectorEvent.time == animEvent.time && animEvent.functionName == selectorEvent.functionName)
                return;
        }

        animClip.AddEvent(animEvent);
    }

    #endregion

    #region skill

    private float _SkillProcessing;
    public float SkillProcessing
    {
        get
        {
            return _SkillProcessing;
        }
        set
        {
            _SkillProcessing = value;
        }
    }

    #endregion

    #region event

    private EventController _EventController;
    public EventController EventController
    {
        get
        {
            return _EventController;
        }
    }

    #endregion

    #region roleAttr

    private Tables.MonsterBaseRecord _MonsterBase;
    private Tables.MOTION_TYPE _MotionType = Tables.MOTION_TYPE.Normal;

    public Tables.MonsterBaseRecord MonsterBase
    {
        get
        {
            return _MonsterBase;
        }
    }

    public Tables.MOTION_TYPE MotionType
    {
        get
        {
            return _MotionType;
        }
    }

    private SummonMotionData _SummonMotionData;
    public bool IsSummonMotion
    {
        get
        {
            return _SummonMotionData != null;
        }
    }

    private bool _IsMotionDie = false;
    public bool IsMotionDie
    {
        get
        {
            return _IsMotionDie;
        }
    }

    private RoleAttrManager _RoleAttrManager;
    public RoleAttrManager RoleAttrManager
    {
        get
        {
            return _RoleAttrManager;
        }
        set
        {
            _RoleAttrManager = value;
        }
    }

    public void InitRoleAttr(Tables.MonsterBaseRecord monsterBase, Tables.MOTION_TYPE motonType)
    {
        _MonsterBase = monsterBase;
        _MotionType = motonType;
    }

    public void InitRoleAttr(SummonMotionData summonData)
    {
        _SummonMotionData = summonData;
    }

    public void InitRoleAttr()
    {
        _IsMotionDie = false;
        _RoleAttrManager = GetComponent<RoleAttrManager>();
        if (_RoleAttrManager == null)
        {
            _RoleAttrManager = gameObject.AddComponent<RoleAttrManager>();
        }
        _RoleAttrManager._MotionManager = this;

        if (_RoleAttrManager.MotionType == Tables.MOTION_TYPE.MainChar)
        {
            _RoleAttrManager.InitMainRoleAttr();
            if(RoleData.SelectRole != null)
                _MotionAnimPath = RoleData.SelectRole.MotionFold;
        }
        else if (_MonsterBase != null)
        {
            _RoleAttrManager.InitEnemyAttr(_MonsterBase, FightManager.Instance._FightLevel, MotionType);
            _MotionAnimPath = _MonsterBase.AnimPath;
        }
        else if (_SummonMotionData != null)
        {
            _RoleAttrManager.InitSummonAttr(_SummonMotionData);
            _MotionAnimPath = _SummonMotionData.SummonRecord.MonsterBase.AnimPath;
        }
        else
        {
            _RoleAttrManager.InitTestAttr();
            Debug.LogError("MonsterBase is Null");
        }
    }

    public void MotionDie()
    {
        Profiler.BeginSample("MotionDie");
        if (IsBuffCanDie())
        {
            _IsMotionDie = true;
            RemoveAllBuff();
            //Hashtable hash = new Hashtable();
            //hash.Add("HitEffect", -1);

            //RecvAllEffects();
            FightManager.Instance.OnObjDie(this);
            FlyEvent(0.1f, -1, -1, this, null, Vector3.zero, 0);
            //_EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, this, new Hashtable());
            //_BaseMotionManager.FlyEvent(0.1f, -1, this, null);
        }
        else
        {
            _RoleAttrManager.AddHP(1);
        }
        Profiler.EndSample();
    }

    public void MotionCorpse()
    {
        TriggerCollider.enabled = false;
        _CanBeSelectByEnemy = false;
        FightManager.Instance.ObjCorpse(this);

        if (RoleAttrManager.MotionType != Tables.MOTION_TYPE.MainChar)
        {
            MonsterDrop.MonsterDropItems(this);
        }
    }

    public void MotionDisappear()
    {
        if (FightManager.Instance != null && RoleAttrManager.MotionType != Tables.MOTION_TYPE.MainChar)
        {
            FightManager.Instance.ObjDisapear(this);
        }
    }

    #endregion

    #region buff

    private List<ImpactBuff> _ImpactBuffs = new List<ImpactBuff>();
    private GameObject _BuffBindPos;
    public GameObject BuffBindPos
    {
        get
        {
            if (_BuffBindPos == null)
            {
                _BuffBindPos = new GameObject("BuffBind");
                _BuffBindPos.transform.SetParent(transform);
                _BuffBindPos.transform.localPosition = Vector3.zero;
                _BuffBindPos.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            return _BuffBindPos;
        }
    }
    private List<ImpactBuff> _RemoveTemp = new List<ImpactBuff>();

    public ImpactBuff AddBuff(ImpactBuff buff, float lastTime = -1)
    {
        if (!IsCanAddBuff(buff))
            return null;

        var buffGO = new GameObject("Buff-" + buff.ToString());
        buffGO.transform.SetParent(BuffBindPos.transform);
        buffGO.transform.localPosition = Vector3.zero;
        buffGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var newBuff = buffGO.AddComponent(buff.GetType()) as ImpactBuff;
        CopyComponent(buff, newBuff);
        if (lastTime > 0)
        {
            newBuff._LastTime = lastTime;
        }
        _ImpactBuffs.Add(newBuff);
        newBuff.ActBuff(this);

        return newBuff;
    }

    public bool IsContainsBuff(Type buffType)
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            if (_ImpactBuffs[i].GetType() == buffType)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsCanAddBuff(ImpactBuff newBuff)
    {
        if (IsMotionDie)
            return false;

        foreach (var buff in _ImpactBuffs)
        {
            if (!buff.IsCanAddBuff(newBuff))
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveBuff(ImpactBuff buff)
    {
        if (_ImpactBuffs.Contains(buff))
        {
            buff.RemoveBuff(this);
            _ImpactBuffs.Remove(buff);
            GameObject.Destroy(buff.gameObject);
        }
    }

    public void RemoveBuff(Type buffType)
    {
        _RemoveTemp.Clear();
        foreach (var buff in _ImpactBuffs)
        {
            if (buff.GetType() == buffType)
            {
                _RemoveTemp.Add(buff);
            }
        }

        foreach (var buff in _RemoveTemp)
        {
            RemoveBuff(buff);
        }
    }

    public void RemoveAllBuff()
    {
        foreach (var buff in _ImpactBuffs)
        {
            buff.RemoveBuff(this);
            GameObject.Destroy(buff.gameObject);
        }

        _ImpactBuffs.Clear();
    }

    void CopyComponent(object original, object destination)
    {
        System.Type type = original.GetType();
        System.Reflection.FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(destination, field.GetValue(original));
        }
    }

    public void ForeachBuffModify(ImpactBuff.BuffModifyType type, Hashtable result, params object[] args)
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            _ImpactBuffs[i].BuffModify(type, args);
        }
    }

    public bool IsBuffCanHit(MotionManager impactSender, ImpactHit impactHit)
    {
        if (IsMotionDie)
            return true;

        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            if (!_ImpactBuffs[i].IsBuffCanHit(impactSender, impactHit))
                return false;
        }
        return true;
    }

    public bool BuffBeHit(MotionManager impactSender, ImpactHit impactHit)
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            _ImpactBuffs[i].BeHit(impactSender, impactHit);
        }
        return true;
    }

    public void BuffAttack()
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            _ImpactBuffs[i].Attack();
        }
    }

    public void BuffHitEnemy()
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            _ImpactBuffs[i].HitEnemy();
        }
    }

    public void BuffHitEnemy(ImpactHit hitImpact, List<MotionManager> hittedMotions)
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            _ImpactBuffs[i].HitEnemy(hitImpact, hittedMotions);
        }
    }

    public bool IsBuffCanCatch(MotionManager impactSender, ImpactCatch impactCatch)
    {
        if (IsMotionDie)
            return true;

        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            if (!_ImpactBuffs[i].IsBuffCanCatch(impactCatch))
                return false;
        }
        return true;
    }

    public bool IsBuffCanDie()
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            if (!_ImpactBuffs[i].IsBuffCanDie())
                return false;
        }
        return true;
    }

    public void BuffModifyDamage(RoleAttrManager.DamageClass damageValue, ImpactBase impactBase)
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            _ImpactBuffs[i].DamageModify(damageValue, impactBase);
        }
    }

    public void BuffCastDamage(RoleAttrManager.DamageClass damageValue, ImpactBase impactBase)
    {
        for (int i = 0; i < _ImpactBuffs.Count; ++i)
        {
            _ImpactBuffs[i].CastDamage(damageValue, impactBase);
        }
    }

    #region sp buff names

    public List<string> _SpBuffNames = new List<string>();
    public bool _IsBuffNameDirty = false;

    public void AddBuffName(string buffName)
    {
        _SpBuffNames.Add(buffName);
        _IsBuffNameDirty = true;
    }

    #endregion

    #endregion

    #region effect

    private Dictionary<string, EffectController> _SkillEffects = new Dictionary<string, EffectController>();
    private Dictionary<string, Transform> _BindTransform = new Dictionary<string, Transform>();
    private EffectController _PlayingEffect;
    public EffectController PlayingEffect
    {
        get
        {
            return _PlayingEffect;
        }
    }
    public Dictionary<int, EffectController> _DynamicEffects = new Dictionary<int, EffectController>();
    private int _DynamicEffectID = 0;
    public int GetDynamicEffectID()
    {
        ++_DynamicEffectID;
        return _DynamicEffectID;
    }

    public ElementType _SkillElement = ElementType.Physic;

    public void RecvAllEffects()
    {
        foreach (var effect in _SkillEffects.Values)
        {
            ResourcePool.Instance.RecvIldeEffect(effect);
        }

        foreach (var effect in _DynamicEffects.Values)
        {
            ResourcePool.Instance.RecvIldeEffect(effect);
        }
    }

    public EffectController PlaySkillEffect(EffectController effect, float speed = -1, ElementType elementType = ElementType.Physic)
    {
        if (!_SkillEffects.ContainsKey(effect.name))
        {
            var idleEffect = GameObject.Instantiate(effect);
            idleEffect.GetComponent<EffectController>().SetEffectSize(effect._EffectSizeRate);
            idleEffect.transform.SetParent(GetBindTransform(effect._BindPos));
            idleEffect.transform.localPosition = Vector3.zero;
            idleEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
            CopyComponent(effect, idleEffect);
            _SkillEffects.Add(effect.name, idleEffect);
        }
        _PlayingEffect = _SkillEffects[effect.name];
        _PlayingEffect.SetEffectColor(_SkillElement);
        if(speed < 0)
            _PlayingEffect.PlayEffect(RoleAttrManager.AttackSpeed);
        else
            _PlayingEffect.PlayEffect(speed);

        return _PlayingEffect;
    }

    public void StopSkillEffect(EffectController effect)
    {
        if (effect == null)
            return;

        if (_SkillEffects.ContainsKey(effect.name))
        {
            _SkillEffects[effect.name].HideEffect();
        }
    }

    public void StopSkillEffect(string effectName)
    {
        if (string.IsNullOrEmpty(effectName))
            return;

        if (_SkillEffects.ContainsKey(effectName))
        {
            _SkillEffects[effectName].HideEffect();
        }
    }

    public void StopSkillEffect()
    {
        StopSkillEffect(_PlayingEffect);
    }

    public void PauseSkillEffect()
    {
        _PlayingEffect.PauseEffect();
        //foreach (var skillEffect in _SkillEffects)
        //{
        //    skillEffect.Value.PauseEffect();
        //}
    }

    public void ResumeSkillEffect()
    {
        _PlayingEffect.ResumeEffect();
        //foreach (var skillEffect in _SkillEffects)
        //{
        //    skillEffect.Value.ResumeEffect();
        //}
    }



    public Transform GetBindTransform(string bindName)
    {
        if (string.IsNullOrEmpty(bindName))
        {
            if (!_BindTransform.ContainsKey(_Animaton.name))
            {
                var bindTran = transform.Find(_Animaton.name);
                _BindTransform.Add(_Animaton.name, bindTran);
            }
        }

        if (!_BindTransform.ContainsKey(bindName))
        {
            var bindTran = transform.Find(_Animaton.name + "/" + bindName);
            _BindTransform.Add(bindName, bindTran);
        }

        return _BindTransform[bindName];
    }

    public int PlayDynamicEffect(string effectName, Hashtable hashParam = null)
    {
        int dynamicEffectID = 0;
        ResourcePool.Instance.LoadEffect(effectName, (resName, effect, callBackHash) =>
        {
            dynamicEffectID = PlayDynamicEffect(effect, callBackHash);
        }, null);

        return dynamicEffectID;
    }

    public int PlayDynamicEffect(EffectController effect, Hashtable hashParam = null)
    {
        var idleEffect = ResourcePool.Instance.GetIdleEffect(effect);
        idleEffect.transform.SetParent(GetBindTransform(idleEffect._BindPos));
        idleEffect.transform.localPosition = Vector3.zero;
        idleEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (hashParam != null)
        {
            if (hashParam.ContainsKey("WorldPos"))
            {
                idleEffect.transform.position = (Vector3)hashParam["WorldPos"];
            }
            if (hashParam.ContainsKey("Rotation"))
            {
                idleEffect.transform.rotation = ((Quaternion)hashParam["Rotation"]);
            }
        }
        //idleEffect._EffectLastTime = effect._EffectLastTime;
        if (hashParam == null)
            idleEffect.PlayEffect();
        else
            idleEffect.PlayEffect(hashParam);

        int dynamicEffectID = GetDynamicEffectID();
        _DynamicEffects.Add(dynamicEffectID, idleEffect);
        if (idleEffect._EffectLastTime > 0)
        {
            StartCoroutine(StopDynamicEffect(dynamicEffectID));
        }

        return dynamicEffectID;
    }

    public IEnumerator StopDynamicEffect(int effctID)
    {
        if (!_DynamicEffects.ContainsKey(effctID))
            yield break;

        var effct = _DynamicEffects[effctID];
        yield return new WaitForSeconds( effct._EffectLastTime);
        
        StopDynamicEffectImmediately(effctID);
    }

    public void StopDynamicEffectImmediately(int effctID)
    {
        if (!_DynamicEffects.ContainsKey(effctID))
            return;

        var effct = _DynamicEffects[effctID];
        if (ResourcePool.Instance.IsEffectInRecvl(effct))
            return;

        effct.HideEffect();
        ResourcePool.Instance.RecvIldeEffect(effct);
    }

    public void PlayHitEffect(MotionManager impactSender, int effectIdx)
    {
        if (ResourcePool.Instance._CommonHitEffect.Count > effectIdx && effectIdx >= 0)
        {
            Hashtable hash = new Hashtable();
            if (impactSender != null)
                hash.Add("Rotation", Quaternion.LookRotation(impactSender.transform.position - transform.position, Vector3.zero));

            PlayDynamicEffect(ResourcePool.Instance._CommonHitEffect[effectIdx], hash);
        }
    }

    #endregion

    #region audio

    public AudioSource _AudioSource;

    public void InitAudio()
    {
        _AudioSource = gameObject.GetComponent<AudioSource>();
        if (_AudioSource == null)
        {
            _AudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayAudio(AudioClip audioClip)
    {
        _AudioSource.PlayOneShot(audioClip);
    }

    #endregion

    #region move

    private Vector3 _TargetVec;
    private Vector3 _TargetPos;
    private float _LastTime;
    private float _Speed;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        return;
    }

    public void SetLookRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.LookRotation(rotate);
    }

    public void SetRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.Euler(rotate);
    }

    public void SetLookAt(Vector3 target)
    {
        transform.LookAt(target);
    }

    public void ResetMove()
    {
        _TargetVec = Vector3.zero;
        _LastTime = 0;
    }

    public void SetMove(Vector3 moveVec, float lastTime)
    {
        _TargetVec = moveVec;
        _LastTime = lastTime;
        _Speed = _TargetVec.magnitude / _LastTime;
        _TargetPos = Vector3.zero;
    }

    public void SetMove(Vector3 moveVec, float lastTime, Vector3 targetPos)
    {
        _TargetVec = moveVec;
        _LastTime = lastTime;
        _Speed = _TargetVec.magnitude / _LastTime;
        _TargetPos = targetPos;
    }

    public void UpdateMove()
    {
        if (_TargetVec == Vector3.zero)
            return;

        Vector3 moveVec = _TargetVec.normalized * _Speed * Time.fixedDeltaTime;

        _TargetVec -= moveVec;
        _LastTime -= Time.fixedDeltaTime;
        if (_LastTime < 0)
        {
            _LastTime = 0;
            _TargetVec = Vector3.zero;
        }

        var destPoint = transform.position + moveVec;
        if (_TargetPos != Vector3.zero)
        {
            var delta = transform.position - _TargetPos;
            if (delta.magnitude < moveVec.magnitude)
            {
                destPoint = _TargetPos;
                _LastTime = 0;
            }
        }

        SetPosition(destPoint);
        
    }

    public void MoveDirect(Vector2 direct)
    {

        Vector3 derectV3 = new Vector3(direct.x, 0, 0);

        MoveDirect(derectV3);
    }

    public void MoveDirect(Vector3 derectV3)
    {
        
        Vector3 destPoint = transform.position + derectV3 * RoleAttrManager.MoveSpeed * Time.fixedDeltaTime;
        SetPosition(destPoint);
        if (derectV3.x > 0)
        {
            SetRotate(Vector3.zero);
        }
        else if (derectV3.x < 0)
        {
            SetRotate(new Vector3(0,180,0));
        }


    }

    public void MoveTarget(Vector3 targetPos)
    {

        SetMove(targetPos-transform.position, (targetPos - transform.position).magnitude/ RoleAttrManager.MoveSpeed, targetPos);
    }

    public void StopMove()
    {
        return;
    }

    public void SetCorpsePrior()
    {
        //int corpsePrior = 99;
        //switch (RoleAttrManager.MotionType)
        //{
        //    case MotionType.Normal:
        //        corpsePrior = _NormalCorpsePrior;
        //        break;
        //    case MotionType.Elite:
        //        corpsePrior = _EliteCorpsePrior;
        //        break;
        //    case MotionType.Hero:
        //        corpsePrior = _HeroCorpsePrior;
        //        break;
        //    case MotionType.MainChar:
        //        corpsePrior = _PlayerCorpsePrior;
        //        break;
        //}

        //_NavAgent.avoidancePriority = corpsePrior;
    }

    public void ResumeCorpsePrior()
    {
        //int corpsePrior = 99;
        //switch (RoleAttrManager.MotionType)
        //{
        //    case MotionType.Normal:
        //        corpsePrior = _NormalNavPrior;
        //        break;
        //    case MotionType.Elite:
        //        corpsePrior = _EliteNavPrior;
        //        break;
        //    case MotionType.Hero:
        //        corpsePrior = _HeroNavPrior;
        //        break;
        //    case MotionType.MainChar:
        //        corpsePrior = _PlayerNavPrior;
        //        break;
        //}

        //_NavAgent.avoidancePriority = corpsePrior;
    }

    #endregion

    #region motion pause

    //public void SkillPause()
    //{
    //    if (ActingSkill != null)
    //    {
    //        ActingSkill.PauseSkill();
    //        PauseAnimation();
    //        PauseSkillEffect();
    //    }
    //}

    //public void SkillPause(float time)
    //{
    //    SkillPause();
    //    StartCoroutine(SkillResumeLater(time));
    //}

    //public void SkillResume()
    //{
    //    if (ActingSkill != null)
    //    {
    //        ActingSkill.ResumeSkill();
    //        ResumeAnimation();
    //        ResumeSkillEffect();
    //    }
    //}

    //public IEnumerator SkillResumeLater(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    SkillResume();
    //}

    #endregion

    #region target

    public bool _CanBeSelectByEnemy = true;

    private ObstacleAvoidanceType _OrgAvoidType;
    private int _NoDamageTimes = 0;
    public void SetMotionNoDamage()
    {
        ++_NoDamageTimes;
        if (_NoDamageTimes > 0)
        {
            TriggerCollider.enabled = false;
            _CanBeSelectByEnemy = false;
        }
    }

    public void ResetMotionNoDamage()
    {
        --_NoDamageTimes;
        if (_NoDamageTimes == 0)
        {
            TriggerCollider.enabled = true;
            _CanBeSelectByEnemy = true;
        }
    }

    #endregion

    #region bullet

    private Transform _BulletBindPos = null;
    public Transform BulletBindPos
    {
        get
        {
            if (_BulletBindPos == null)
            {
                var bulletGO = new GameObject("BulletBind");
                _BulletBindPos = bulletGO.transform;
                _BulletBindPos.transform.SetParent(transform);
                _BulletBindPos.transform.localPosition = Vector3.zero;
                _BulletBindPos.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            return _BulletBindPos;
        }
    }

    #endregion

    #region collider

    public Vector3 _ColliderInfo = new Vector3(0.4f, 1.8f, 0);
    public Vector3 _ColliderCenter = new Vector3(0, 0, 0);
    public bool _IsTrigger = true;

    private Collider _TriggerCollider;
    public Collider TriggerCollider
    {
        get
        {
            if (_TriggerCollider == null)
            {
                if (_ColliderInfo.x > 0)
                {
                    _TriggerCollider = gameObject.AddComponent<CapsuleCollider>();
                    //_TriggerCollider = AnimationEvent.GetComponentInChildren<Collider>();
                    //if (_TriggerCollider == null)
                    //{
                    //    var sole = AnimationEvent.transform.Find("center/sole");
                    //    if (sole == null)
                    //    {
                    //        sole = AnimationEvent.transform.Find("Bip001/sole");
                    //    }
                    //    var collider = sole.gameObject.AddComponent<CapsuleCollider>();
                    //    collider.radius = _ColliderInfo.x;
                    //    collider.height = _ColliderInfo.y;
                    //    collider.direction = 2;
                    //    if (!_IsTrigger)
                    //    {
                    //        collider.center = new Vector3(0, 0, 3000);
                    //    }
                    //    else
                    //    {
                    //        if (_ColliderCenter == Vector3.zero)
                    //        {
                    //            collider.center = _ColliderCenter;
                    //        }
                    //        else
                    //        {
                    //            collider.center = new Vector3(0, 0, collider.height * 0.5f);
                    //        }
                    //    }
                    //    collider.isTrigger = true;
                    //    var rigidbody = sole.gameObject.AddComponent<Rigidbody>();
                    //    rigidbody.isKinematic = true;
                    //    rigidbody.useGravity = false;
                    //    _TriggerCollider = collider;
                    //}
                }
            }
            return _TriggerCollider;
        }
    }

    #endregion

    #region state mechine

    public string _MotionAnimPath = "";

    public StateIdle _StateIdle;
    public StateMove _StateMove;
    public StateHit _StateHit;
    public StateFly _StateFly;
    public StateCatch _StateCatch;
    public StateRise _StateRise;
    public StateLie _StateLie;
    public StateDie _StateDie;
    public StateSkill _StateSkill;

    public StateBase _ActionState;

    private List<AI_Base> _AIBases;

    private void InitState()
    {
        _StateIdle = new StateIdle();
        _StateIdle.InitAnimation(this);

        _StateMove = new StateMove();
        _StateMove.InitAnimation(this);

        _StateHit = new StateHit();
        _StateHit.InitAnimation(this);

        _StateFly = new StateFly();
        _StateFly.InitAnimation(this);

        _StateCatch = new StateCatch();
        _StateCatch.InitAnimation(this);

        _StateRise = new StateRise();
        _StateRise.InitAnimation(this);

        _StateLie = new StateLie();
        _StateLie.InitAnimation(this);

        _StateDie = new StateDie();
        _StateDie.InitAnimation(this);

        _StateSkill = new StateSkill();
        _StateSkill.InitAnimation(this);

        _AIBases = new List<AI_Base>( gameObject.GetComponents<AI_Base>());

        TryEnterState(_StateIdle, null);
    }

    public void TryEnterState(StateBase state, params object[] args)
    {
        if (!state.CanStartState(args))
            return;

        var orgState = _ActionState;
        if (_ActionState != null)
        {
            _ActionState.FinishState();
        }

        state.StartState(args);
        _ActionState = state;

        foreach (var aiBase in _AIBases)
        {
            aiBase.OnStateChange(orgState, _ActionState);
        }
    }

    public void StateOpt(StateBase.MotionOpt opt, params object[] args)
    {
        if (_ActionState == null)
            return;

        _ActionState.StateOpt(opt, args);
    }

    #endregion

    #region state opt

    public ObjMotionSkillBase ActingSkill
    {
        get
        {
            if (_ActionState == _StateSkill)
            {
                return _StateSkill.ActingSkill;
            }
            return null;
        }
    }

    public void InputDirect(Vector2 direct)
    {
        StateOpt(StateBase.MotionOpt.Input_Direct, direct);
    }

    public void ActSkill(ObjMotionSkillBase skillMotion, Hashtable hash = null)
    {
        StateOpt(StateBase.MotionOpt.Act_Skill, skillMotion, hash);
    }

    public void InputSkill(string key)
    {
        StateOpt(StateBase.MotionOpt.Input_Skill, key);
    }

    public void FinishSkill(ObjMotionSkillBase skillMotion)
    {
        StateOpt(StateBase.MotionOpt.Stop_Skill, skillMotion);

        //if (InputManager.Instance._InputMotion == this)
        //{
        //    InputManager.Instance.SkillFinish(skillMotion);
        //}
    }

    public void ActionPause(float time)
    {
        StateOpt(StateBase.MotionOpt.Pause_State, time);
    }

    public void ActionResume()
    {
        StateOpt(StateBase.MotionOpt.Resume_State);
    }

    public void NotifyAnimEvent(string function, object param)
    {
        StateOpt(StateBase.MotionOpt.Anim_Event, function, param);
    }

    public void HitEvent(float hitTime, int hitEffect, int hitAudio, MotionManager impactSender, ImpactHit hitImpact, Vector3 moveDirect, float moveTime, bool isPauseFly = false)
    {
        if (!IsBuffCanHit(impactSender, hitImpact))
        {
            return;
        }
        BuffBeHit(impactSender, hitImpact);
        foreach (var aiBase in _AIBases)
        {
            aiBase.OnBeHit(hitImpact);
        }
        StateOpt(StateBase.MotionOpt.Hit, hitTime, hitEffect, impactSender, hitImpact, moveDirect, moveTime, hitAudio, isPauseFly);
    }

    public void FrozenEvent()
    {

    }

    public void FlyEvent(float flyHeight, int hitEffect, int hitAudio, MotionManager impactSender, ImpactHit hitImpact, Vector3 moveDirect, float moveTime)
    {
        //Debug.Log("FlyEvent");
        if (!IsBuffCanHit(impactSender, hitImpact))
        {
            return;
        }
        BuffBeHit(impactSender, hitImpact);
        foreach (var aiBase in _AIBases)
        {
            aiBase.OnBeHit(hitImpact);
        }
        StateOpt(StateBase.MotionOpt.Fly, flyHeight, hitEffect, impactSender, hitImpact, moveDirect, moveTime, hitAudio);
    }

    public void CatchEvent(float catchTime, int hitEffect, int hitAudio, MotionManager impactSender, ImpactHit hitImpact, Vector3 moveDirect, float moveTime)
    {
        if (!IsBuffCanCatch(impactSender, hitImpact as ImpactCatch))
        {
            return;
        }
        BuffBeHit(impactSender, hitImpact);
        foreach (var aiBase in _AIBases)
        {
            aiBase.OnBeHit(hitImpact);
        }
        StateOpt(StateBase.MotionOpt.Catch, catchTime, hitEffect, impactSender, hitImpact, moveDirect, moveTime, hitAudio);
    }

    public void StopCatch()
    {
        //Debug.Log("StopCatch");
        StateOpt(StateBase.MotionOpt.Stop_Catch);
    }

    public void StartMoveState(Vector3 target)
    {
        StateOpt(StateBase.MotionOpt.Move_Target, target);
    }

    public void StopMoveState()
    {
        StateOpt(StateBase.MotionOpt.Stop_Move);
    }

    #endregion

    #region audio

    public AudioClip _BehitAudio;
    public AudioClip _DeadAudio;

    #endregion
}