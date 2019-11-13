using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class AI_Base : MonoBehaviour
{
    public MotionManager _TargetMotion;

    bool _Init = false;

    public MotionManager _SelfMotion;
    protected bool _AIAwake = false;
    public bool AIWake
    {
        get
        {
            return _AIAwake;
        }
        set
        {
            _AIAwake = value;
        }
    }
    public int GroupID;

    void Start()
    {
        StartCoroutine(InitDelay());
        AIManager.Instance.RegistAI(this);

        //StartCoroutine(AIFixedUpdate());
    }

    void OnDisable()
    {
        AIManager.Instance.RemoveAI(this);
    }

    //private IEnumerator AIFixedUpdate()
    //{
    //    yield return new WaitForSeconds(0.1f);

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.1f);

    //        if (!_Init)
    //            continue;

    //        if (_SelfMotion == null || _SelfMotion.IsMotionDie)
    //        {
    //            yield break;
    //        }

    //        AIUpdate();
    //    }
    //}

    void FixedUpdate()
    {

        if (!_Init)
            return;

        if (_SelfMotion == null || _SelfMotion.IsMotionDie)
        {
            return;
        }

        AIUpdate();

    }

    private IEnumerator InitDelay()
    {
        yield return new WaitForFixedUpdate();
        Init();
    }

    protected virtual void Init()
    {
        _Init = true;
        _SelfMotion = GetComponent<MotionManager>();

        if (_TargetMotion == null)
        {
            var mainPlayer = SelectTargetCommon.GetMainPlayer();
            if (mainPlayer != null)
            {
                _TargetMotion = mainPlayer;
            }
        }

        ModifyInitSkill();
        InitSkillInfos();

        _BornPos = transform.position;

        InitReleaseSkillTimes();
    }

    protected virtual void AIUpdate()
    {
        HpItemUpdate();
    }

    public virtual void OnStateChange(StateBase orgState, StateBase newState)
    {
        MoveState(orgState, newState);
        HitProtectStateChange(newState);
    }

    public virtual void OnBeHit(ImpactHit impactHit)
    {
        OnHitProtect(impactHit);
    }

    #region combatLevel

    protected class CombatInfo
    {
        public float AfterSkillWait;
        public float MoveWait;
    }

    protected static Dictionary<int, CombatInfo> _CombatInfos = new Dictionary<int, CombatInfo>()
    {
        { 1, new CombatInfo(){ AfterSkillWait=1, MoveWait=1} },
        { 2, new CombatInfo(){ AfterSkillWait=2.0f, MoveWait=2.0f} },
        { 3, new CombatInfo(){ AfterSkillWait=3.0f, MoveWait=3.0f} },
        { 4, new CombatInfo(){ AfterSkillWait=4.0f, MoveWait=4.0f} },
    };

    protected int _CombatLevel = 1;

    public void SetCombatLevel(int level)
    {
        _CombatLevel = level;

        if (_Init)
        {
            ModifyInitSkill();
        }
    }

    protected virtual void ModifyInitSkill()
    {
        if (_AISkills.Count == 0)
            return;

        foreach (var aiskill in _AISkills)
        {
            if (aiskill.MonDamageRate != 1)
            {
                var skillDamages = aiskill.SkillBase.GetComponentsInChildren<ImpactDamage>(true);
                foreach (var damage in skillDamages)
                {
                    damage._DamageRate *= aiskill.MonDamageRate;
                }

                var bulletDamages = aiskill.SkillBase.GetComponentsInChildren<BulletEmitterBase>(true);
                foreach (var bulletEmitter in bulletDamages)
                {
                    bulletEmitter._Damage *= aiskill.MonDamageRate;
                }

                var subImpactDamages = aiskill.SkillBase.GetComponentsInChildren<ImpactBuffSub>(true);
                foreach (var subImpact in subImpactDamages)
                {
                    subImpact._DamageRate *= aiskill.MonDamageRate;
                }
            }
        }
        
        if (_CombatLevel > 1)
        {
            if (_CombatInfos.ContainsKey(_CombatLevel))
            {
                var combatInfo = _CombatInfos[_CombatLevel];
                foreach (var aiskill in _AISkills)
                {
                    aiskill.AfterSkillWait = aiskill.AfterSkillWait * combatInfo.AfterSkillWait;
                    var skillDamages = aiskill.SkillBase.GetComponentsInChildren<ImpactDamage>(true);
                    foreach (var damage in skillDamages)
                    {
                        if (damage._IsCharSkillDamage)
                        {
                            damage._DamageRate *= aiskill.MonDamageRate;
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region skill

    [System.Serializable]
    public class AI_Skill_Info
    {
        public ObjMotionSkillBase SkillBase;
        public float SkillRange;
        public float SkillInterval;
        public float ReadyTime = 0;
        public float StartCD = -1;
        public float AfterSkillWait = 1;
        public float MonDamageRate = 1;

        public float FirstHitTime { get; set; }
        public float LastUseSkillTime { get; set; }

        public bool IsSkillCD()
        {
            return (Time.time - LastUseSkillTime > SkillInterval);
        }
    }
    public List<AI_Skill_Info> _AISkills;

    public void InitSkillDamageRate(float damageRate)
    {
        foreach (var aiSkill in _AISkills)
        {
            aiSkill.MonDamageRate *= damageRate;
        }
    }

    public void InitMonsterDamageRate()
    {
        for (int i = 0; i < _AISkills.Count; ++i)
        {
            if (i > 0)
            {
                _AISkills[i].MonDamageRate *= 2;
            }
        }
    }

    private float _ComondSkillCD = 0;
    private float _LastUseSkillTime = 0;

    public void InitSkillGoes(MotionManager mainMotion)
    {
        GameObject motionObj = new GameObject("Motion");
        motionObj.transform.SetParent(mainMotion.transform);
        motionObj.transform.localPosition = Vector3.zero;
        motionObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        for(int i = 0; i< _AISkills.Count; ++i)
        {
            var skillInfo = _AISkills[i];
            var skillBase = GameObject.Instantiate(skillInfo.SkillBase);
            skillBase.transform.SetParent(motionObj.transform);
            skillBase.gameObject.SetActive(true);
            skillInfo.SkillBase = skillBase;
            if (i == 0)
            {
                skillInfo.LastUseSkillTime = Time.time - skillInfo.SkillInterval;
            }
            else
            {
                if (skillInfo.StartCD >= 0)
                {
                    skillInfo.LastUseSkillTime = Time.time - skillInfo.SkillInterval + skillInfo.StartCD;
                }
                else
                {
                    skillInfo.LastUseSkillTime = Time.time - skillInfo.SkillInterval * 0.4f;

                }
            }
        }
        
    }

    protected void InitSkillInfos()
    {

    }

    protected void SetSkillCD(AI_Skill_Info skillInfo, float cdTime)
    {
        skillInfo.LastUseSkillTime = Time.time;
        _LastUseSkillTime = Time.time;
    }

    protected bool IsCommonCD()
    {
        //if (Time.time - _LastUseSkillTime < _ComondSkillCD)
        //{
        //    return true;
        //}
        //return false;
        return true;
    }

    protected virtual void StartSkill(AI_Skill_Info skillInfo, bool isIgnoreCD = false)
    {
        if (!skillInfo.SkillBase.IsCanActSkill())
            return;

        _SelfMotion.transform.LookAt(_TargetMotion.transform.position);
        _SelfMotion.ActSkill(skillInfo.SkillBase);
        SetSkillCD(skillInfo, skillInfo.SkillInterval);
    }

    protected virtual bool StartSkill()
    {
        if (!IsRandomActSkill())
            return false;

        float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        for (int i = _AISkills.Count - 1; i >= 0; --i)
        {
            if (!_AISkills[i].IsSkillCD())
                continue;

            //if (!IsCommonCD())
            //    continue;

            if (_AISkills[i].SkillRange < dis)
                continue;

            StartSkill(_AISkills[i]);
            return true;

        }

        return false;
    }

    int _ActValue = -1;
    float _AtkRate = -1;
    float _LastRandomTime = 0;
    float _AttackInterval = 0.5f;
    protected bool IsRandomActSkill()
    {

        if (Time.time - _LastRandomTime < _AttackInterval)
        {
            return false;
        }
        _LastRandomTime = Time.time;

        var actRandom = Random.Range(0, 10000);

        if (_ActValue < 0)
        {
            float diffModify = 1 + ActData.Instance.GetNormalDiff() * 0.2f;
            diffModify = Mathf.Clamp(diffModify, 1, 2);
            if (_SelfMotion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.Normal)
            {
                _ActValue = (int)(500 * diffModify);
                _AttackInterval = 1;
            }
            else if (_SelfMotion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.Elite)
            {
                _ActValue = (int)(2000 * diffModify);
                _AttackInterval = 1;
            }
            else if (_SelfMotion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.ExElite)
            {
                _ActValue = (int)(3000 * diffModify);
                _AttackInterval = 1;
            }
            else if (_SelfMotion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.Hero)
            {
                _ActValue = 10000;
                _AttackInterval = 1.0f - ActData.Instance.GetNormalDiff() * 0.2f;
                _AttackInterval = Mathf.Max(_AttackInterval, 0.2f);
            }
        }

        if (_ActValue >= actRandom)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Move radius

    private float _MoveRadius = 0;
    private float _HitRadius = 0.1f;

    protected void MoveState(StateBase orgState, StateBase newState)
    {

        if (_HitRadius <= 0)
            return;

        if (_MoveRadius == _HitRadius)
            return;

        if (_SelfMotion == null)
            return;

    }

    #endregion

    #region move

    public float _AlertRange = 7;
    public float _CloseRange = 2;
    public float _HuntRange = 10;
    public float _ReHuntRange = 5;

    protected float _CloseInterval = 0.5f;
    protected Vector3 _BornPos;
    protected bool _IsReturnToBornPos = false;
    private float _MoveActTime = 0;
    private float _MoveInterval = -1;
    private float _CloseWait;

    public bool IsActMove()
    {

        if (_SelfMotion._ActionState != _SelfMotion._StateIdle
            && _SelfMotion._ActionState != _SelfMotion._StateMove)
        {
            _IsReturnToBornPos = false;
        }

        if (!(_SelfMotion._ActionState is StateIdle || _SelfMotion._ActionState is StateMove))
        {
            return false;
        }

        


        float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        float bornDis = Vector3.Distance(transform.position, _BornPos);
        //var pathToTarget = GetPath(transform.position, _TargetMotion.transform.position);
        //var pathToBorn = GetPath(transform.position, _BornPos);
        //if (pathToTarget == null || pathToBorn == null)
        //    return false;

        //float distance = GetPathLength(pathToTarget);
        //float bornDis = GetPathLength(pathToBorn);

        if (_CloseWait > 0)
        {
            _CloseWait -= Time.deltaTime;
            return false;
        }

        //alert hunt
        if (!_IsReturnToBornPos)
        {
            //too far
            if (bornDis > _HuntRange)
            {
                _IsReturnToBornPos = true;
                SetMove(_BornPos);
                return true;
            }
            //close enough
            else if (distance < _CloseRange)
            {
                _SelfMotion.StopMoveState();
                _CloseWait = _CloseInterval;
                return false;
            }
            else //hunt
            {
                SetMove(_TargetMotion.transform.position);
                return true;
            }
        }
        else
        {
            //rehunt
            if (distance < _ReHuntRange)
            {
                //rehunt in back
                if (bornDis < _HuntRange * 0.5f)
                {
                    _IsReturnToBornPos = false;
                    SetMove(_TargetMotion.transform.position);
                    return true;
                }
                else
                {
                    SetMove(_BornPos);
                    return true;
                }
            }
            else if (bornDis < 1.0f) //back close
            {
                _IsReturnToBornPos = false;
                _CloseWait = _CloseInterval;
                _SelfMotion.StopMoveState();
                _AIAwake = false;
                return false;
            }
            else
            {
                SetMove(_BornPos);
                return true;
            }
        }   
    }

    public void SetMove(Vector3 position)
    {
        if (_MoveInterval < 0)
        {
            switch (_SelfMotion.MotionType)
            {
                case Tables.MOTION_TYPE.Normal:
                    _MoveInterval = 0.2f;
                    break;
                case Tables.MOTION_TYPE.Elite:
                    _MoveInterval = 0.1f;
                    break;
                case Tables.MOTION_TYPE.ExElite:
                    _MoveInterval = 0.05f;
                    break;
                case Tables.MOTION_TYPE.Hero:
                    _MoveInterval = 0f;
                    break;
            }
        }

        if (Time.time - _MoveActTime < _MoveInterval)
        {
            return;
        }
        _MoveActTime = Time.time;

        _SelfMotion.StartMoveState(position);
    }

    public static NavMeshPath GetPath(Vector3 fromPos, Vector3 toPos)
    {
        NavMeshPath path = new NavMeshPath();

        if (NavMesh.CalculatePath(fromPos, toPos, NavMesh.AllAreas, path) == false)
            return null;

        return path;
    }

    public static float GetPathLength(NavMeshPath path)
    {
        float lng = 0.0f;

        if (path == null)
            return 999999;

        if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
        {
            for (int i = 1; i < path.corners.Length; ++i)
            {
                lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }

        return lng;
    }

    public static float GetPathLength(Vector3 fromPos, Vector3 toPos)
    {
        var navMeshPath = GetPath(fromPos, toPos);
        if (navMeshPath == null)
            return 9999999;
        return GetPathLength(navMeshPath);
    }
    #endregion

    #region show hp item

    protected bool _IsShowHP = false;

    protected virtual void HpItemUpdate()
    {
        if (_IsShowHP)
            return;

        //if (_SelfMotion._ActionState != _SelfMotion._StateIdle
        //    && _SelfMotion._ActionState != _SelfMotion._StateMove)
        {
            _IsShowHP = true;
            UIHPPanel.ShowHPItem(_SelfMotion);
        }
    }

    #endregion

    #region hit protect

    public class HitSkillInfo
    {
        public ObjMotionSkillBase SkillBase;
        public int SkillActTimes;
        public int HitTimes;
    }

    protected ImpactBuff _HitProtectedPrefab;
    public ImpactBuff HitProtectedPrefab
    {
        get
        {
            if (_HitProtectedPrefab == null)
            {
                //var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/HitProtectedBuff");
                //_HitProtectedPrefab = buffGO.GetComponent<ImpactBuff>();
                _HitProtectedPrefab = ResourcePool.Instance.GetConfig<ImpactBuff>(ResourcePool.ConfigEnum.HitProtectedBuff);
                _HitProtectedPrefab.Init(null, null);
            }
            return _HitProtectedPrefab;
        }
    }

    public Dictionary<string, int> _ProtectTimes = new Dictionary<string, int>();
    public void InitProtectTimes(int protectTimes)
    {
        if (protectTimes <= 0)
            return;

        _ProtectTimes.Add("1", protectTimes);
        _ProtectTimes.Add("2", protectTimes);
        _ProtectTimes.Add("3", protectTimes);
    }
    protected bool _ProtectTimesDirty = false;
    public bool ProtectTimesDirty
    {
        get
        {
            return _ProtectTimesDirty;
        }
        set
        {
            _ProtectTimesDirty = value;
        }
    }

    protected Dictionary<string, HitSkillInfo> _HitDict = new Dictionary<string, HitSkillInfo>();
    protected static int _ReleaseSkillTimes = -1;
    protected static int _ReleaseAttackTimes = 8;
    protected static int _ReleaseBuffTimes = 8;
    protected ObjMotionSkillBase _HittingSkill;
    protected bool _ProtectMode = false;

    private void InitReleaseSkillTimes()
    {
        //if (_ReleaseSkillTimes > 0)
        //    return;

        if (FightSkillManager.Instance.ReuseSkillConfig.Equals("0"))
        {
            _ReleaseSkillTimes = 1;
            _ReleaseBuffTimes = 4;
            _ReleaseAttackTimes = 3;
        }
        else
        {
            _ReleaseSkillTimes = 2;
            _ReleaseBuffTimes = 7;
            _ReleaseAttackTimes = 4;
            Debug.Log("_ReleaseSkillTimes:" + _ReleaseSkillTimes);
        }

        //if (RoleData.SelectRole.Profession == Tables.PROFESSION.GIRL_DEFENCE
        //        || RoleData.SelectRole.Profession == Tables.PROFESSION.GIRL_DOUGE)
        //{
        //    _ReleaseAttackTimes += 1;
        //}
    }

    private void OnHitProtect(ImpactHit impactHit)
    {
        if (impactHit == null)
            return;

        //if (impactHit.SkillMotion is ObjMotionSkillAttack)
        //    return;

        if (impactHit.SkillMotion == null)
            return;

        if (!impactHit._IsCharSkillDamage)
            return;

        if (!_HitDict.ContainsKey(impactHit.SkillMotion._ActInput))
        {
            _HitDict.Add(impactHit.SkillMotion._ActInput, new HitSkillInfo() { SkillBase = impactHit.SkillMotion , SkillActTimes= -1, HitTimes=0});
        }

            //protect times
        if (_HittingSkill != null 
            && _HittingSkill != impactHit.SkillMotion 
            && !(_HittingSkill is ObjMotionSkillBuff)
            && !(_HittingSkill is ObjMotionSkillAttack))
        {
            if (_ProtectTimes.Count > 0)
            {
                ReleaseHit();
                UIMessageTip.ShowMessageTip(2300083);
                return;
            }
        }
        //else if (_HittingSkill != null)
        //{
        //    if (_ProtectTimes.Count > 0)
        //    {
        //        return;
        //    }
        //}

        if (_HitDict[impactHit.SkillMotion._ActInput].SkillActTimes != impactHit.SkillMotion._SkillActTimes
            || _HittingSkill != impactHit.SkillMotion)
        {
            if (_ProtectMode)
            {
                ReleaseHit();
                return;
            }

            _HittingSkill = impactHit.SkillMotion;

            if (_HittingSkill != null
                && _HittingSkill._ActInput.Equals("e"))
            {
                Debug.Log("ExAttackRelease");
                return;
            }

            _HitDict[impactHit.SkillMotion._ActInput].SkillActTimes = impactHit.SkillMotion._SkillActTimes;

            if (_ProtectTimes.ContainsKey(impactHit.SkillMotion._ActInput))
            {
                --_ProtectTimes[impactHit.SkillMotion._ActInput];
                if (_ProtectTimes[impactHit.SkillMotion._ActInput] == 0)
                {
                    _ProtectTimes.Remove(impactHit.SkillMotion._ActInput);
                }
                ProtectTimesDirty = true;
                _ProtectMode = true;
                UIMessageTip.ShowMessageTip(2300083);
                return;
            }

            ++_HitDict[impactHit.SkillMotion._ActInput].HitTimes;
            if (impactHit.SkillMotion is ObjMotionSkillBuff)
            {
                //if (impactHit.SkillMotion._ActInput == "5")
                {
                    if (_HitDict[impactHit.SkillMotion._ActInput].HitTimes > _ReleaseBuffTimes)
                    {
                        ReleaseHit();
                    }
                }
            }
            else if (impactHit.SkillMotion is ObjMotionSkillAttack)
            {
                Debug.Log("Attack hit time:" + _HitDict[impactHit.SkillMotion._ActInput].HitTimes);
                if (_HitDict[impactHit.SkillMotion._ActInput].HitTimes > _ReleaseAttackTimes)
                {
                    ReleaseHit(); 
                }
            }
            else
            {
                if (_HitDict[impactHit.SkillMotion._ActInput].HitTimes > _ReleaseSkillTimes)
                {
                    ReleaseHit();
                }
            }
        }
        _HittingSkill = impactHit.SkillMotion;
    }

    private void ReleaseHit()
    {
        Debug.Log("ReleaseHit ");
        ImpactFlyAway impact = new ImpactFlyAway();
        impact._FlyHeight = 1;
        impact._Time = 0.5f;
        impact._Speed = 10;
        impact._DamageRate = 0;
        impact.ActImpact(_SelfMotion, _SelfMotion);
        HitProtectedPrefab.ActBuffInstance(_SelfMotion, _SelfMotion);
        _ProtectMode = false;
    }

    private void HitProtectStateChange(StateBase newState)
    {
        if (newState is StateFly
            || newState is StateCatch
            || newState is StateHit
            || newState is StateLie
            || newState is StateDie)
            return;

        _HitDict.Clear();
        _HittingSkill = null;
        _ProtectMode = false;
    }

    #endregion
    
}


