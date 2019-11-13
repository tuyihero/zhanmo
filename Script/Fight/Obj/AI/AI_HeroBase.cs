using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeroBase : AI_Base
{
    protected override void Init()
    {
        base.Init();
        InitRise();

        InitPassiveSkills();

        //BuffStar();
        BuffBlockSummon();
        //IsCancelNormalAttack = false;
    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        RiseUpdate();
        CrazyUpdate();
        //UpdateCriticalAI();
    }

    #region use skill

    protected float _AfterSkillWait = 0;
    protected float _AfterSkillTime = 0;

    protected override bool StartSkill()
    {
        if (!IsCancelNormalAttack || (IsCancelNormalAttack && _SelfMotion.ActingSkill != _AISkills[0].SkillBase))
        {
            if (!IsRandomActSkill())
                return false;
        }

        if (Time.time - _AfterSkillTime < _AfterSkillWait)
            return false;

        float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        for (int i = _AISkills.Count - 1; i >= 0; --i)
        {
            if (!_AISkills[i].IsSkillCD())
                continue;

            if (_AISkills[i].SkillRange < dis)
                continue;

            StartSkill(_AISkills[i]);
            _AfterSkillWait = _AISkills[i].AfterSkillWait;
            return true;

        }

        return false;
    }

    public override void OnStateChange(StateBase orgState, StateBase newState)
    {
        base.OnStateChange(orgState, newState);

        //after skill
        if (orgState is StateSkill && newState is StateIdle)
        {
            _AfterSkillTime = Time.time;
        }

        //cancel attack
        if (newState is StateSkill)
        {
            if (_SelfMotion.ActingSkill == _AISkills[0].SkillBase)
            {
                int cancelSkillIdx = -1;
                for (int i = 1; i < _AISkills.Count; ++i)
                {
                    float skillCD = _AISkills[i].SkillInterval  - (Time.time - _AISkills[i].LastUseSkillTime);
                    float lastCD = _AISkills[0].FirstHitTime / _SelfMotion.RoleAttrManager.AttackSpeed + 0.1f;
                    if (skillCD < lastCD)
                    {
                        _AISkills[i].LastUseSkillTime = Time.time - _AISkills[i].SkillInterval + lastCD;
                    }
                    else if (cancelSkillIdx < 0 && skillCD < _AISkills[i].SkillInterval * 0.25f)
                    {
                        cancelSkillIdx = i;
                        _AISkills[i].LastUseSkillTime = Time.time - _AISkills[i].SkillInterval + lastCD;
                        _AfterSkillWait = lastCD;
                    }
                }
            }
        }
    }

    private bool _IsCancelNormalAttack = false;
    public bool IsCancelNormalAttack
    {
        get
        {
            return _IsCancelNormalAttack;
        }
        set
        {
            _IsCancelNormalAttack = value;
            if (_IsCancelNormalAttack)
            {
                _AISkills[0].SkillBase._SkillMotionPrior = 50;
            }
        }
    }

    #endregion

    #region rise

    public bool _IsRiseBoom = false;
    private ImpactBase _RiseBoom;

    private float _RiseTime = 1f;
    private bool _IsRiseEvent = false;

    private void InitRise()
    {
        if (!_IsRiseBoom)
            return;

        ResourcePool.Instance.LoadConfig("SkillMotion/CommonImpact/RiseBoomSkill", (resName, resGO, hash) =>
        {
            var riseBoom = resGO;
            var motionTrans = transform.Find("Motion");
            riseBoom.transform.SetParent(motionTrans);
            riseBoom.transform.localPosition = Vector3.zero;
            riseBoom.transform.localRotation = Quaternion.Euler(Vector3.zero);

            _RiseBoom = riseBoom.GetComponent<ImpactBase>();
        }, null);
        
    }

    private void RiseUpdate()
    {
        if (!_IsRiseBoom)
            return;

        if (_SelfMotion._ActionState == _SelfMotion._StateRise)
        {
            if (!_IsRiseEvent)
            {
                _IsRiseEvent = true;
                _RiseBoom.ActImpact(_SelfMotion, _SelfMotion);
            }
        }
        else
        {
            if (_IsRiseEvent)
            {
                _IsRiseEvent = false;
            }
        }
    }

    #endregion

    #region super armor

    private ImpactBuff _SuperArmorPrefab;
    public ImpactBuff SuperArmorPrefab
    {
        get
        {
            if (_SuperArmorPrefab == null)
            {
                //var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/SuperArmor");
                //_SuperArmorPrefab = buffGO.GetComponent<ImpactBuff>();
                _SuperArmorPrefab = ResourcePool.Instance.GetConfig<ImpactBuff>(ResourcePool.ConfigEnum.SuperArmor);
            }
            return _SuperArmorPrefab;
        }
    }

    private ImpactBuff _SuperArmorBlockPrefab;
    public ImpactBuff SuperArmorBlockPrefab
    {
        get
        {
            if (_SuperArmorBlockPrefab == null)
            {
                //var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/SuperArmorBlock");
                //_SuperArmorBlockPrefab = buffGO.GetComponent<ImpactBuff>();
                _SuperArmorBlockPrefab = ResourcePool.Instance.GetConfig<ImpactBuff>(ResourcePool.ConfigEnum.SuperArmorBlock);
            }
            return _SuperArmorBlockPrefab;
        }
    }

    private ImpactBuffSuperArmor _BuffInstance;

    public bool _IsContainsNormalAtk = false;

    protected void InitSkills()
    {
        int startIdx = 0;
        if (!_IsContainsNormalAtk)
        {
            startIdx = 1;
        }
        for (int i = startIdx; i < _AISkills.Count; ++i)
        {
            var firstHitTime = InitSuperArmorSkill(_AISkills[i].SkillBase);
            _AISkills[i].FirstHitTime = firstHitTime;
            InitReadySkillSpeed(_AISkills[i]);
        }
    }

    protected float InitSuperArmorSkill(ObjMotionSkillBase objMotionSkill)
    {
        if (objMotionSkill._NextAnim.Count == 0)
            return 0;

        float attackConlliderTime = _SelfMotion.AnimationEvent.GetAnimFirstColliderEventTime(objMotionSkill._NextAnim[0], objMotionSkill._SuperArmorColliderID);
        if (attackConlliderTime < 0)
        {
            attackConlliderTime = objMotionSkill._NextAnim[0].length - 0.05f;
        }

        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], 0, AttackStart);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], attackConlliderTime + 0.05f, AttackCollider);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], objMotionSkill._NextAnim[0].length, AttackCollider);
        return attackConlliderTime;
    }

    private void AttackStart()
    {
        _BuffInstance = SuperArmorBlockPrefab.ActBuffInstance(_SelfMotion, _SelfMotion) as ImpactBuffSuperArmor;

    }

    private void AttackCollider()
    {
        if (_BuffInstance != null)
        {
            _SelfMotion.RemoveBuff(_BuffInstance);
        }
    }

    #endregion

    #region 

    #region ready skill speed

    private static float _ReadySkillSpeedTime = 0.8f;
    private float _CurAnimSpeed;
    private float _CurEffectSpeed;
    private Dictionary<ObjMotionSkillBase, float> _SkillColliderTime = new Dictionary<ObjMotionSkillBase, float>();

    public void InitReadySkillSpeed(AI_Skill_Info aiSkillInfo)
    {
        if (aiSkillInfo.ReadyTime == 0)
            return;

        if (_ReadySkillSpeedTime == 0)
            return;

        _SkillColliderTime.Add(aiSkillInfo.SkillBase, aiSkillInfo.ReadyTime);
        _SelfMotion.AnimationEvent.AddEvent(aiSkillInfo.SkillBase._NextAnim[0], 0, AttackStartForSpeed);
        _SelfMotion.AnimationEvent.AddEvent(aiSkillInfo.SkillBase._NextAnim[0], aiSkillInfo.ReadyTime - 0.05f, AttackColliderForSpeed);
        _SelfMotion.AnimationEvent.AddEvent(aiSkillInfo.SkillBase._NextAnim[0], aiSkillInfo.SkillBase._NextAnim[0].length, AttackColliderForSpeed);
    }

    private void AttackStartForSpeed()
    {
        //_CurAnimSpeed = -1;
        //var animState = _SelfMotion.GetCurAnim();
        //if (animState == null)
        //    return;

        //if (!_SkillColliderTime.ContainsKey(_SelfMotion._StateSkill.ActingSkill))
        //    return;

        //var readyTime = _SkillColliderTime[_SelfMotion._StateSkill.ActingSkill];
        //if (readyTime > _ReadySkillSpeedTime)
        //    return;

        //var speed = readyTime * animState.speed / _ReadySkillSpeedTime;
        //_CurAnimSpeed = animState.speed;
        //animState.speed = speed;

        //if (_SelfMotion.PlayingEffect != null)
        //{
        //    var effectSpeed = readyTime * _SelfMotion.PlayingEffect.LastPlaySpeed / _ReadySkillSpeedTime;
        //    _CurEffectSpeed = _SelfMotion.PlayingEffect.LastPlaySpeed;
        //    _SelfMotion.PlayingEffect.SetEffectSpeed(effectSpeed);
        //}
    }

    private void AttackColliderForSpeed()
    {
        //if (_CurAnimSpeed < 0)
        //    return;

        //var animState = _SelfMotion.GetCurAnim();
        //if (animState == null)
        //    return;

        //if (!_SkillColliderTime.ContainsKey(_SelfMotion._StateSkill.ActingSkill))
        //    return;

        //animState.speed = _CurAnimSpeed;

        //if (_SelfMotion.PlayingEffect != null)
        //{
        //    _SelfMotion.PlayingEffect.SetEffectSpeed(_CurEffectSpeed);
        //}
    }

    #endregion

    #endregion

    #region critical AI

    public float _CriticalSkillRate = 0;

    //if target start use skill, AI use skill
    protected void UpdateCriticalAI()
    {
        if (_SelfMotion._ActionState != _SelfMotion._StateIdle)
            return;

        if (_TargetMotion.ActingSkill == null)
            return;

        if (_CriticalSkillRate <= 0)
            return;

        var random = Random.Range(0, 1);
        if (_CriticalSkillRate < random)
            return;

        AI_Skill_Info aiSkill = _AISkills[0];
        for (int i = _AISkills.Count - 1; i >= 0; --i)
        {
            if (!_AISkills[i].IsSkillCD())
            {
                aiSkill = _AISkills[i];
                break;
            }
        }
        StartSkill(aiSkill);
    }

    #endregion

    #region Passive skills

    public Transform _PassiveGO;

    public void InitPassiveSkills()
    {
        if (_PassiveGO == null)
            return;

        List<ImpactBase> passiveImpacts = new List<ImpactBase>();
        for (int i = 0; i < _PassiveGO.childCount; ++i)
        {
            var passiveImpact = _PassiveGO.GetChild(i).GetComponents<ImpactBase>();
            passiveImpacts.AddRange(passiveImpact);
        }

        foreach (var buff in passiveImpacts)
        {
            buff.ActImpact(_SelfMotion, _SelfMotion);
        }
    }

    #endregion

    #region stage 2 /crizy buff

    public float Stage2SuperTime = 20;
    public List<float> _StageBuffHpPersent;

    protected bool _Stage2Started = false;

    protected ImpactBuff[] _Strtage2Buff;
    public ImpactBuff[] StrStage2Buff
    {
        get
        {
            if (_Strtage2Buff == null)
            {
                InitCrazyBuff();
                for (int i = 0; i < _Strtage2Buff.Length; ++i)
                {
                    _Strtage2Buff[i].Init(null, null);
                }
            }
            return _Strtage2Buff;
        }
    }

    protected virtual void InitCrazyBuff()
    {

    }

    protected virtual void CrazyUpdate()
    {
        if (_StageBuffHpPersent.Count > 0)
        {
            if (_SelfMotion.RoleAttrManager.HPPersent < _StageBuffHpPersent[0])
            {
                StartStage2();
                _StageBuffHpPersent.RemoveAt(0);
            }
        }
    }

    protected virtual void StartStage2()
    {
        for (int i = 0; i < StrStage2Buff.Length; ++i)
        {
            float buffLoasTime = StrStage2Buff[i]._LastTime;
            if (StrStage2Buff[i]._LastTime <= 0)
            {
                buffLoasTime = Stage2SuperTime;
            }
            StrStage2Buff[i].ActBuffInstance(_SelfMotion, _SelfMotion, buffLoasTime);
        }
    }

    #endregion

    #region block bullet

    private ImpactBuff _BulletBlockBuffPrefab;
    public ImpactBuff BulletBlockBuffPrefab
    {
        get
        {
            if (_BulletBlockBuffPrefab == null)
            {
                //var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/BlockBullet");
                //_BulletBlockBuffPrefab = buffGO.GetComponent<ImpactBuff>();
                _BulletBlockBuffPrefab = ResourcePool.Instance.GetConfig<ImpactBuff>(ResourcePool.ConfigEnum.BlockBullet);
            }
            return _BulletBlockBuffPrefab;
        }
    }

    private void BuffStar()
    {
        BulletBlockBuffPrefab.ActBuffInstance(_SelfMotion, _SelfMotion);

    }

    #endregion

    #region block summon

    private ImpactBuff _SummonBlockBuffPrefab;
    public ImpactBuff SummonBlockBuffPrefab
    {
        get
        {
            if (_SummonBlockBuffPrefab == null)
            {
                //var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/BlockBullet");
                //_BulletBlockBuffPrefab = buffGO.GetComponent<ImpactBuff>();
                _SummonBlockBuffPrefab = ResourcePool.Instance.GetConfig<ImpactBuff>(ResourcePool.ConfigEnum.BlockSummon);
            }
            return _SummonBlockBuffPrefab;
        }
    }

    private void BuffBlockSummon()
    {
        SummonBlockBuffPrefab.ActBuffInstance(_SelfMotion, _SelfMotion);

    }

    #endregion

}
