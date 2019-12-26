using UnityEngine;
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

        _JumpBody = transform.Find("AnimTrans");
        _ShadowTrans = transform.Find("Shadow");
        var bodyGO = transform.Find("AnimTrans/Body/Body");
        _Animaton = bodyGO.GetComponent<Animator>();
        var bodyEffect = transform.Find("AnimTrans/Body/Body/Effect/Effect");
        if (bodyEffect != null)
        {
            _EffectAnimator = bodyEffect.GetComponent<Animator>();
        }
        var weapon = transform.Find("AnimTrans/Body/Body/Weapon/Weapon");
        if (weapon != null)
        {
            _WeaponAnimator = weapon.GetComponent<Animator>();
            var weaponEffect = transform.Find("AnimTrans/Body/Weapon/Weapon/Effect");
            if (weaponEffect != null)
            {
                _WeaponEffectAnimator = weaponEffect.GetComponent<Animator>();
            }
        }
        InitEffectAnimator();

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
        if (_ActionPause)
        {
            if (_ActionPauseTime > 0)
            {
                if (Time.time - _ActionPauseStart > _ActionPauseTime)
                {
                    ActionResume();
                }
            }
            return;
        }

        UpdateMove();
        UpdateJump();
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
    List<string> _BodyEffects;
    List<string> _WeaponEffects;

    private void InitEffectAnimator()
    {
        if (_BodyEffects != null)
            return;

        _BodyEffects = new List<string>();
        _WeaponEffects = new List<string>();
        if (_EffectAnimator != null && _EffectAnimator.runtimeAnimatorController != null)
        {
            foreach (var anim in _EffectAnimator.runtimeAnimatorController.animationClips)
            {
                _BodyEffects.Add(anim.name);
            }
        }

        if (_WeaponEffectAnimator != null)
        {
            foreach (var anim in _WeaponEffectAnimator.runtimeAnimatorController.animationClips)
            {
                _WeaponEffects.Add(anim.name);
            }
        }
    }

    private AnimEventManager _AnimationEvent;
    public AnimEventManager AnimationEvent
    {
        get
        {
            return _AnimationEvent;
        }
    }

    public void InitAnimation(AnimationClip animClipTemp)
    {
        var animClip = GetAnimClip(animClipTemp.name);
        List<AnimationEvent> eveneList = new List<UnityEngine.AnimationEvent>();
        foreach (var animEvent in animClip.events)
        {
            if (animEvent.functionName == "ColliderStart" && animEvent.intParameter >= 1000 && animEvent.intParameter < 10000)
            { }
            else if (animEvent.functionName == "CollidertEnd" && animEvent.intParameter >= 1000 && animEvent.intParameter < 10000)
            { }
            else
            {
                eveneList.Add(animEvent);
            }
        }
        animClip.events = eveneList.ToArray();

        //_Animaton.AddClip(animClip, animClip.name);
    }

    public AnimationClip GetAnimClip(string animName)
    {
        foreach (var anim in Animation.runtimeAnimatorController.animationClips)
        {
            if (anim.name == animName)
                return anim;
        }

        return null;
    }

    public void PlayAnimation(AnimationClip animClip)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = RoleAttrManager.AttackSpeed;
        _Animaton.Play(animClip.name, 0, 0);
        if (_EffectAnimator != null)
        {
            if (_BodyEffects.Contains(animClip.name))
            {
                _EffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _EffectAnimator.Play("None", 0, 0);
            }
            
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.Play(animClip.name, 0, 0);
        }
        if (_WeaponEffectAnimator != null)
        {
            if (_WeaponEffects.Contains(animClip.name))
            {
                _WeaponEffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _WeaponEffectAnimator.Play("None", 0, 0);
            }
        }
    }

    public void PlayAnimation(AnimationClip animClip, float speed)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = speed;
        _Animaton.Play(animClip.name, 0, 0);

        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = speed;
            if (_BodyEffects.Contains(animClip.name))
            {
                _EffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _EffectAnimator.Play("None", 0, 0);
            }
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = speed;
            _WeaponAnimator.Play(animClip.name, 0, 0);
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = speed;
            if (_WeaponEffects.Contains(animClip.name))
            {
                _WeaponEffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _WeaponEffectAnimator.Play("None", 0, 0);
            }
        }
    }

    public void RePlayAnimation(AnimationClip animClip, float speed)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = speed;
        //_Animaton.Stop();
        _Animaton.Play(animClip.name, 0, 0);

        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = speed;
            if (_BodyEffects.Contains(animClip.name))
            {
                _EffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _EffectAnimator.Play("None", 0, 0);
            }
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = speed;
            _WeaponAnimator.Play(animClip.name, 0, 0);
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = speed;
            if (_WeaponEffects.Contains(animClip.name))
            {
                _WeaponEffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _WeaponEffectAnimator.Play("None", 0, 0);
            }
        }
    }

    public void RePlayAnimation(AnimationClip animClip)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton.speed = RoleAttrManager.AttackSpeed;
        _Animaton.Play(animClip.name, 0, 0);

        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = _Animaton.speed;
            if (_BodyEffects.Contains(animClip.name))
            {
                _EffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _EffectAnimator.Play("None", 0, 0);
            }
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = _Animaton.speed;
            _WeaponAnimator.Play(animClip.name, 0, 0);
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = _Animaton.speed;
            if (_WeaponEffects.Contains(animClip.name))
            {
                _WeaponEffectAnimator.Play(animClip.name, 0, 0);
            }
            else
            {
                _WeaponEffectAnimator.Play("None", 0, 0);
            }
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
        _OrgSpeed = _Animaton.speed;
        _Animaton.speed = 0;
        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = 0;
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = 0;
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = 0;
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
        if (_EffectAnimator != null)
        {
            _EffectAnimator.speed = _OrgSpeed;
        }
        if (_WeaponAnimator != null)
        {
            _WeaponAnimator.speed = _OrgSpeed;
        }
        if (_WeaponEffectAnimator != null)
        {
            _WeaponEffectAnimator.speed = _OrgSpeed;
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

        if (MotionType == Tables.MOTION_TYPE.MainChar)
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
            FlyEvent(0.1f, -1, -1, this, null, Vector3.zero, 0, 5);
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
                var bindTran = _Animaton.transform.Find(_Animaton.name);
                _BindTransform.Add(_Animaton.name, bindTran);
            }
        }

        if (!_BindTransform.ContainsKey(bindName))
        {
            var bindTran = _Animaton.transform.Find(bindName);
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

    private Transform _ShadowTrans;
    private Vector3 _TargetVec;
    private Vector3 _TargetPos;
    private float _LastTime;
    private float _Speed;

    private bool _IsMoveBorder;

    public void SetPosition(Vector3 position, bool border = false)
    {
        float xLimit = position.x;
        if ((RoleAttrManager != null && RoleAttrManager.MotionType == Tables.MOTION_TYPE.MainChar))
        {
            xLimit = Mathf.Clamp(position.x, FightManager.Instance._CameraFollow.MainMovePosXMin, FightManager.Instance._CameraFollow.MainMovePosXMax);
        }
        else if (border)
        {
            xLimit = Mathf.Clamp(position.x, FightManager.Instance._CameraFollow.MainMovePosXMin - 0.5f, FightManager.Instance._CameraFollow.MainMovePosXMax + 0.5f);
        }

        float zLimit = position.z;
        //if (RoleAttrManager != null && RoleAttrManager.MotionType == Tables.MOTION_TYPE.MainChar)
        {
            zLimit = Mathf.Clamp(position.z, FightManager.Instance._CameraFollow._SceneAnimController.SceneZMin, FightManager.Instance._CameraFollow._SceneAnimController.SceneZMax);
        }

        Vector3 destPos = new Vector3(xLimit, FightManager.Instance._CameraFollow._SceneAnimController.SceneY, zLimit);
        transform.position = destPos;

        return;
    }

    public void SetLookRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.LookRotation(rotate);
    }

    public void SetRotate(Vector3 rotate)
    {
        if (_Animaton == null)
            return;

        transform.rotation = Quaternion.Euler(rotate);

        if (rotate.y == 0)
        {
            _Animaton.transform.parent.localRotation = Quaternion.Euler(45, 0, 0);
            _ShadowTrans.transform.localRotation = Quaternion.Euler(45, 0, 0);

            if(_EffectAnimator != null)
                _EffectAnimator.transform.parent.localPosition = new Vector3(_EffectAnimator.transform.parent.localPosition.x, _EffectAnimator.transform.parent.localPosition.y, -0.03f);
            if (_WeaponAnimator != null)
                _WeaponAnimator.transform.parent.localPosition = new Vector3(_WeaponAnimator.transform.parent.localPosition.x, _WeaponAnimator.transform.parent.localPosition.y, -0.01f);
            if (_WeaponEffectAnimator != null)
                _WeaponEffectAnimator.transform.localPosition = new Vector3(_WeaponEffectAnimator.transform.parent.localPosition.x, _WeaponEffectAnimator.transform.parent.localPosition.y, -0.02f);
        }
        else
        {
            _Animaton.transform.parent.localRotation = Quaternion.Euler(-45, 0, 0);
            _ShadowTrans.transform.localRotation = Quaternion.Euler(-45, 0, 0);

            if (_EffectAnimator != null)
                _EffectAnimator.transform.parent.localPosition = new Vector3(_EffectAnimator.transform.parent.localPosition.x, _EffectAnimator.transform.parent.localPosition.y, 0.03f);
            if (_WeaponAnimator != null)
                _WeaponAnimator.transform.parent.localPosition = new Vector3(_WeaponAnimator.transform.parent.localPosition.x, _WeaponAnimator.transform.parent.localPosition.y, 0.01f);
            if (_WeaponEffectAnimator != null)
                _WeaponEffectAnimator.transform.localPosition = new Vector3(_WeaponEffectAnimator.transform.parent.localPosition.x, _WeaponEffectAnimator.transform.parent.localPosition.y, 0.02f);
        }
        
    }

    public void SetLookAt(Vector3 target)
    {
        if (target.x > transform.position.x)
        {
            SetRotate(Vector3.zero);
        }
        else if (target.x < transform.position.x)
        {
            SetRotate(new Vector3(0, 180, 0));
        }
    }

    public void ResetMove()
    {
        _TargetVec = Vector3.zero;
        _LastTime = 0;
    }

    public void SetMove(Vector3 moveVec, float lastTime, bool border = true)
    {
        _TargetVec = moveVec;
        _LastTime = lastTime;
        _Speed = _TargetVec.magnitude / _LastTime;
        _TargetPos = Vector3.zero;
        _IsMoveBorder = border;
    }

    public void SetMove(Vector3 moveVec, float lastTime, Vector3 targetPos)
    {
        _TargetVec = moveVec;
        _LastTime = lastTime;
        _Speed = _TargetVec.magnitude / _LastTime;
        _TargetPos = targetPos;
        _IsMoveBorder = false;
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

        SetPosition(destPoint, _IsMoveBorder);
        
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

    public void MoveTarget(Vector3 targetPos,float speed)
    {

        SetMove(targetPos-transform.position, (targetPos - transform.position).magnitude/ (RoleAttrManager.MoveSpeed * speed), targetPos);
    }

    public void StopMove()
    {
        _TargetVec = Vector3.zero;
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

    public Vector3 GetMotionForward()
    {
        if (transform.transform.rotation.eulerAngles.y == 0)
        {
            return new Vector3(1, 0, 0);
        }
        else
        {
            return new Vector3(-1, 0, 0);
        }
    }

    public Vector3 GetMotionAwayDirect(Vector3 position)
    {
        if (position.x >= transform.position.x)
        {
            return new Vector3(1, 0, 0);
        }
        else
        {
            return new Vector3(-1, 0, 0);
        }
    }

    public static Vector3 GetForward2D(Vector3 eulerAngles)
    {
        if (eulerAngles.y == 0)
        {
            return new Vector3(1, 0, 0);
        }
        else
        {
            return new Vector3(-1, 0, 0);
        }
    }

    #endregion

    #region jump

    public float _JumpSpeed = 14;
    public float _JumpTime = 0.2f;
    private float _SwitchZSpeed = 5.0f;

    private float _JumpHeight;
    private Transform _JumpBody;
    public Transform JumpBody
    {
        get
        {
            return _JumpBody;
        }
    }
    private float _CurJumpTime = 0.5f;
    private float _CurJumpSpeed = 3f;
    public float _Gravity = -40f;
    private float _JumpMoveSpeedRate = 0.6f;
    private Vector2 _JumpMoveDirect = Vector2.zero;
    private bool _JumpStay = false;
    public bool JumpStay
    {
        get
        {
            return _JumpStay;
        }
    }

    private Vector2 _SkillJumpSpeed;
    private float _SkillJumpTime;
    private Action _SkillJumpFunc;

    private int _SwitchToPosIdx = 0;
    private int _CurZInx = 0;

    public bool IsInAir()
    {
        return _JumpBody.localPosition.y > 0;
    }

    public void UpdateJump()
    {
        if (_JumpStay)
            return;

        if (_SkillJumpSpeed != Vector2.zero)
        {
            _JumpBody.localPosition += new Vector3(0, _SkillJumpSpeed.y* Time.fixedDeltaTime, 0);
            SetPosition(transform.position + new Vector3(_SkillJumpSpeed.x * GetMotionForward().x * Time.fixedDeltaTime, 0, 0));
            _SkillJumpTime -= Time.fixedDeltaTime;

            Debug.Log("Skill jump height:" + _JumpBody.localPosition.y);
            if (_SkillJumpTime <= 0)
            {
                _SkillJumpSpeed = Vector2.zero;
                _SkillJumpTime = 0;
                _JumpHeight = 1;
                _CurJumpSpeed = 0;

                if (_SkillJumpFunc != null)
                {
                    _SkillJumpFunc.Invoke();
                }
            }

            if (_JumpBody.localPosition.y <= 0)
            {
                _SkillJumpSpeed = Vector2.zero;
                _SkillJumpTime = 0;

                _JumpBody.localPosition = Vector3.zero;
                _JumpHeight = 0;
                _JumpMoveDirect = Vector2.zero;
                ResetJumpSkillAct();

                if (_SkillJumpFunc != null)
                {
                    _SkillJumpFunc.Invoke();
                }
            }
        }
        else if (_JumpHeight > 0)
        {
            _CurJumpSpeed += _Gravity * Time.fixedDeltaTime;
            //_CurJumpSpeed = _JumpSpeed;
            //if (_CurJumpTime <= 0)
            //{
            //    _CurJumpSpeed = _Gravity;
            //}
            //else
            //{
            //    _CurJumpTime -= Time.fixedDeltaTime;
            //}
            float jumpstep = _CurJumpSpeed * Time.fixedDeltaTime;
            float jumpMove = _JumpMoveDirect.normalized.x * RoleAttrManager.MoveSpeed * _JumpMoveSpeedRate* Time.fixedDeltaTime;
            _JumpBody.localPosition += new Vector3(0, jumpstep, 0);

            SetPosition(transform.position + new Vector3(jumpMove,0,0));
            if (jumpMove > 0)
            {
                SetRotate(Vector3.zero);
            }
            else if (jumpMove < 0)
            {
                SetRotate(new Vector3(0, 180, 0));
            }

            if (_JumpBody.localPosition.y <= 0)
            {
                _JumpBody.localPosition = Vector3.zero;
                _JumpHeight = 0;
                _JumpMoveDirect = Vector2.zero;
                ResetJumpSkillAct();
                TryEnterState(_StateIdle, null);
            }

            if (_SwitchToPosIdx >= 0)
            {
                float jumpToPos = FightManager.Instance._CameraFollow._SwitchZ[_SwitchToPosIdx];
                float jumpZSpeed = _SwitchZSpeed;
                if (jumpToPos < transform.position.z)
                {
                    jumpZSpeed = -_SwitchZSpeed;
                    SetPosition(transform.position + new Vector3(0, 0, jumpZSpeed * Time.fixedDeltaTime));

                    if (jumpToPos > transform.position.z)
                    {
                        SetPosition(new Vector3(transform.position.x, transform.position.y, jumpToPos));
                        _CurZInx = _SwitchToPosIdx;
                        _SwitchToPosIdx = -1;
                    }
                    
                }
                else
                {
                    SetPosition(transform.position + new Vector3(0, 0, jumpZSpeed * Time.fixedDeltaTime));

                    if (jumpToPos < transform.position.z)
                    {
                        SetPosition(new Vector3(transform.position.x, transform.position.y, jumpToPos));
                        _CurZInx = _SwitchToPosIdx;
                        _SwitchToPosIdx = -1;
                    }
                    
                }



            }
        }
    }

    public void Jump(float height)
    {
        _JumpHeight = height;
        _CurJumpSpeed = _JumpSpeed;
        _JumpStay = false;
        _CurJumpTime = _JumpTime;
        _SwitchToPosIdx = -1;
    }

    public void SetJumpStay()
    {
        _JumpStay = true;
        _SwitchToPosIdx = -1;
    }

    public void JumpFall()
    {
        _JumpStay = false;
        _CurJumpSpeed = 0;
    }

    public void JumpMove(Vector2 direct)
    {
        _JumpMoveDirect = direct;
        _SwitchToPosIdx = -1;
    }

    public void SetSkillJump(Vector2 speed, float time, Action skillJumpFun = null)
    {
        _JumpHeight = 0;
        _CurJumpSpeed = 0;

        _SkillJumpSpeed = speed;
        _SkillJumpTime = time;

        _SkillJumpFunc = skillJumpFun;

        _SwitchToPosIdx = -1;
    }

    public void ResetJump()
    {
        _JumpHeight = 0;
        _CurJumpSpeed = 0;
        _JumpStay = false;

        _SkillJumpSpeed = Vector2.zero;
        _SkillJumpTime = 0;

        _SkillJumpFunc = null;

        _SwitchToPosIdx = -1;
    }

    public void ResetJumpSkillAct()
    {
        foreach (var skillMotion in _StateSkill._SkillMotions.Values)
        {
            skillMotion.ResetActedTimesInJump();
        }
    }

    public void JumpToZPos(int toZPosIdx)
    {
        _SwitchToPosIdx = toZPosIdx;

        if (_JumpBody.localPosition.y < 0.5f)
        {
            _JumpHeight = 0.5f;
            _CurJumpSpeed = _JumpSpeed;
            _JumpStay = false;
        }
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

    public Vector3 _ColliderInfo = new Vector3(0.2f, 1.8f, 1);
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
                    _TriggerCollider = _JumpBody.gameObject.GetComponent<Collider>();
                    if (_TriggerCollider == null)
                    {
                        var sole = _JumpBody;
                        if (sole == null)
                        {
                            sole = _JumpBody;
                        }
                        var collider = sole.gameObject.AddComponent<BoxCollider>();
                        collider.size = _ColliderInfo;
                        if (!_IsTrigger)
                        {
                            collider.center = new Vector3(0, 0, 3000);
                        }
                        else
                        {
                            if (_ColliderCenter == Vector3.zero)
                            {
                                collider.center = new Vector3(0, collider.size.y * 0.5f, 0);
                            }
                            else
                            {
                                collider.center = _ColliderCenter;
                            }
                        }
                        collider.isTrigger = true;
                        var rigidbody = sole.gameObject.AddComponent<Rigidbody>();
                        rigidbody.isKinematic = true;
                        rigidbody.useGravity = false;
                        _TriggerCollider = collider;
                    }
                }
            }
            return _TriggerCollider;
        }
    }

    #endregion

    #region state mechine

    public bool _HasRiseState = true;

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
    public StateJump _StateJump;
    public StateJumpIdle _StateJumpIdle;
    public StateJumpZ _StateJumpZ;

    public StateBase _ActionState;

    private List<AI_Base> _AIBases;

    private void InitState()
    {
        _StateIdle = new StateIdle();
        _StateIdle.InitAnimation(this);

        _StateMove = new StateMove();
        _StateMove.InitAnimation(this);

        _StateJump = new StateJump();
        _StateJump.InitAnimation(this);

        _StateJumpIdle = new StateJumpIdle();
        _StateJumpIdle.InitAnimation(this);

        _StateJumpZ = new StateJumpZ();
        _StateJumpZ.InitAnimation(this);

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

    public void TryEnterState(StateBase state, Hashtable args = null)
    {
        if (args == null)
        {
            args = new Hashtable();
        }

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

        if (_ActionState == _StateIdle
            || _ActionState == _StateLie
            || _ActionState == _StateDie
            || _ActionState == _StateRise)
        {
            ResetCombo();
        }
    }

    public void StateOpt(StateBase.MotionOpt opt, Hashtable args)
    {
        if (_ActionState == null)
            return;

        if (args == null)
        {
            args = new Hashtable();
        }
        _ActionState.StateOpt(opt, args);
    }

    #endregion

    #region state opt

    public bool _ActionPause = false;
    public bool _IsCanFly = true;
    public bool _IsCanHit = true;
    private float _ActionPauseTime = 0;
    private float _ActionPauseStart = 0;

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
        Hashtable args = new Hashtable();
        args.Add("InputDirect", direct);
        StateOpt(StateBase.MotionOpt.Input_Direct, args);
    }

    public void ActSkill(ObjMotionSkillBase skillMotion, Hashtable hash = null)
    {
        Hashtable args = new Hashtable();
        args.Add("SkillMotion", skillMotion);
        args.Add("SkillParam", skillMotion);
        StateOpt(StateBase.MotionOpt.Act_Skill, args);
    }

    public void InputSkill(string key)
    {
        Hashtable args = new Hashtable();
        args.Add("InputSkill", key);
        StateOpt(StateBase.MotionOpt.Input_Skill, args);
    }

    public void FinishSkill(ObjMotionSkillBase skillMotion)
    {
        Hashtable args = new Hashtable();
        args.Add("SkillMotion", skillMotion);
        StateOpt(StateBase.MotionOpt.Stop_Skill, args);

        //if (InputManager.Instance._InputMotion == this)
        //{
        //    InputManager.Instance.SkillFinish(skillMotion);
        //}
    }

    public void ActionPause(float time)
    {
        _ActionPause = true;
        _ActionPauseTime = time;
        _ActionPauseStart = Time.time;
        Hashtable args = new Hashtable();
        args.Add("PauseTime", time);
        StateOpt(StateBase.MotionOpt.Pause_State, args);
    }

    public void ActionResume()
    {
        _ActionPause = false;
        _ActionPauseTime = 0;
        _ActionPauseStart = 0;
        StateOpt(StateBase.MotionOpt.Resume_State, null);
    }

    public void NotifyAnimEvent(string function, object param)
    {
        Hashtable args = new Hashtable();
        args.Add("FuncName", function);
        args.Add("Param", param);
        StateOpt(StateBase.MotionOpt.Anim_Event, args);
    }

    public void HitEvent(float hitTime, int hitEffect, int hitAudio, MotionManager impactSender, ImpactHit hitImpact, Vector3 moveDirect, float moveTime, bool isPauseFly = false, bool isBorder = true)
    {
        if (!_IsCanHit)
        {
            return;
        }

        if (!IsBuffCanHit(impactSender, hitImpact))
        {
            return;
        }

        if (IsInAir() && _ActionState != _StateFly)
        {
            FlyEvent(0.1f, hitEffect, hitAudio, impactSender, hitImpact, moveDirect, moveTime, 5);
            return;
        }

        Hashtable args = new Hashtable();
        args.Add("HitTime", hitTime);
        args.Add("HitEffect", hitEffect);
        args.Add("HitAudio", hitAudio);
        args.Add("SenderMotion", impactSender);
        args.Add("HitImpact", hitImpact);
        args.Add("MoveDirect", moveDirect);
        args.Add("MoveTime", moveTime);
        args.Add("IsPauseFly", isPauseFly);
        args.Add("IsBorder", isBorder);

        BuffBeHit(impactSender, hitImpact);
        foreach (var aiBase in _AIBases)
        {
            aiBase.OnBeHit(hitImpact);
        }
        StateOpt(StateBase.MotionOpt.Hit, args);

        AddCombo();
    }

    public void FrozenEvent()
    {

    }

    public void FlyEvent(float flyTime, int hitEffect, int hitAudio, MotionManager impactSender, ImpactHit hitImpact, Vector3 moveDirect, float moveTime, float upSpeed, bool isBorder = true)
    {
        if (!_IsCanFly && !IsMotionDie)
        {
            if (_IsCanHit)
            {
                HitEvent(0.6f, hitEffect, hitAudio, impactSender, hitImpact, moveDirect, moveTime, false, isBorder);
            }
            return;
        }
        //Debug.Log("FlyEvent");
        if (!IsBuffCanHit(impactSender, hitImpact))
        {
            return;
        }

        Hashtable args = new Hashtable();
        args.Add("FlyTime", flyTime);
        args.Add("HitEffect", hitEffect);
        args.Add("HitAudio", hitAudio);
        args.Add("SenderMotion", impactSender);
        args.Add("HitImpact", hitImpact);
        args.Add("MoveDirect", moveDirect);
        args.Add("MoveTime", moveTime);
        args.Add("UpSpeed", upSpeed);
        args.Add("IsBorder", isBorder);

        BuffBeHit(impactSender, hitImpact);
        foreach (var aiBase in _AIBases)
        {
            aiBase.OnBeHit(hitImpact);
        }
        StateOpt(StateBase.MotionOpt.Fly, args);

        AddCombo();
    }

    public void CatchEvent(float catchTime, int hitEffect, int hitAudio, MotionManager impactSender, ImpactHit hitImpact, Vector3 moveDirect, float moveTime)
    {
        if (!IsBuffCanCatch(impactSender, hitImpact as ImpactCatch))
        {
            return;
        }

        Hashtable args = new Hashtable();
        args.Add("CatchTime", catchTime);
        args.Add("HitEffect", hitEffect);
        args.Add("HitAudio", hitAudio);
        args.Add("SenderMotion", impactSender);
        args.Add("HitImpact", hitImpact);
        args.Add("MoveDirect", moveDirect);
        args.Add("MoveTime", moveTime);

        BuffBeHit(impactSender, hitImpact);
        foreach (var aiBase in _AIBases)
        {
            aiBase.OnBeHit(hitImpact);
        }
        StateOpt(StateBase.MotionOpt.Catch, args);

        AddCombo();
    }

    public void StopCatch()
    {
        //Debug.Log("StopCatch");
        StateOpt(StateBase.MotionOpt.Stop_Catch, null);
    }

    public void StartMoveState(Vector3 target, Transform lookatTrans = null, float speed = 1)
    {
        Hashtable args = new Hashtable();
        args.Add("MoveTarget", target);
        args.Add("MoveSpeed", speed);
        args.Add("MoveLookatTrans", lookatTrans);

        StateOpt(StateBase.MotionOpt.Move_Target, args);
    }

    public void StopMoveState()
    {
        StateOpt(StateBase.MotionOpt.Stop_Move, null);
    }

    public void JumpState()
    {
        StateOpt(StateBase.MotionOpt.Jump, null);
    }

    public void JumpSwitchZ()
    {
        int jumpToPosIdx = _CurZInx;
        if (jumpToPosIdx + 1 >= FightManager.Instance._CameraFollow._SwitchZ.Count)
        {
            jumpToPosIdx = 0;
        }
        else
        {
            jumpToPosIdx = jumpToPosIdx + 1;
        }

        Hashtable args = new Hashtable();
        args.Add("JumpToPosIdx", jumpToPosIdx);

        StateOpt(StateBase.MotionOpt.Jump, args);
    }

    #endregion

    #region audio

    public AudioClip _BehitAudio;
    public AudioClip _DeadAudio;

    #endregion

    #region combat

    private int _BeHitCombo = 0;
    public int BeHitCombo
    {
        get
        {
            return _BeHitCombo;
        }
    }

    public void ResetCombo()
    {
        _BeHitCombo = 0;
    }

    public void AddCombo()
    {
        ++_BeHitCombo;

        FightManager.Instance.SetCombo(_BeHitCombo);
    }

    #endregion
}
