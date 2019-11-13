using UnityEngine;
using System.Collections;

public class AI_SummonSkill : AI_Base
{
    public float _ModelSizeFixed = 0.7f;

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _AISkills.Count; ++i)
        {
            InitSkillStarSpeed(_AISkills[i].SkillBase);
        }

        InitSummon();
    }

    protected override void AIUpdate()
    {
        //base.AIUpdate();

        UpdateSkill();
    }

    #region 

    public float _SummonPosZ = -1;

    private static float _ShowDelayStatic = 0.15f;
    private static float _SkillDelayStatic = 0.15f;
    private static float _DispearTimeStatic = 0.2f;
    private float _ShowDelay = 0.15f;
    private float _SkillDelay = 0.15f;
    private float _DispearTime = 1.0f;

    private float _SkillStartTime = -1;
    private float _SkillFinishTime = -1;

    private void InitSummon()
    {
        var showSpeed = 1 + GameDataValue.ConfigIntToFloat(_SelfMotion.RoleAttrManager.GetBaseAttr(RoleAttrEnum.RiseHandSpeed));

        _ShowDelay = _ShowDelayStatic / showSpeed;
        _SkillDelay = _SkillDelayStatic / showSpeed;
        _DispearTime = _DispearTimeStatic;
    }

    public void UseSkill(int idx)
    {
        if (_AISkills.Count <= idx)
            return;

        PlayShow();
        if (_SelfMotion.ActingSkill != null)
        {
            _SkillStartTime = -1;
            _SelfMotion.ActingSkill.FinishSkill();
        }
        StartCoroutine(UseSkillDelay(idx));
    }

    public IEnumerator UseSkillDelay(int idx)
    {
        transform.position = transform.position + new Vector3(0, -10000, 0);
        yield return new WaitForSeconds(_ShowDelay);
        transform.position = transform.position + new Vector3(0, 10000, 0);
        yield return new WaitForSeconds(_SkillDelay);

        _SelfMotion.ActSkill(_AISkills[idx].SkillBase);
        _SkillStartTime = Time.time;
    }

    private void UpdateSkill()
    {
        if (_SkillStartTime <= 0)
            return;

        if (_SelfMotion._ActionState is StateSkill)
            return;

        if (_SkillFinishTime < 0)
        {
            _SkillFinishTime = Time.time;
        }

        if (Time.time - _SkillFinishTime < _DispearTime)
            return;

        PlayHide();
        SummonSkill.Instance.HideSummonMotion(this);
        _SkillStartTime = -1;
        
        //ResourcePool.Instance.RecvIldeMotion(_SelfMotion);
    }

    #endregion

    #region summon effect

    public static string _SummonShowEffectPath = "Effect/Common/Effect_Common_Summon_Show_FixP2";
    public static string _SummonHideEffectPath = "Effect/Common/Effect_Common_Summon_Hide_FixP2";

    private static EffectSingle _SummonEffect;
    private static EffectSingle _DesummonEffect;

    public void InitEffect()
    {
        if (_SummonEffect == null)
        {
            ResourcePool.Instance.LoadConfig(_SummonShowEffectPath, (resName, resGO, hash) =>
            {
                var showEffectGO = resGO;
                _SummonEffect = showEffectGO.GetComponent<EffectSingle>();
            }, null);
        }

        if (_DesummonEffect == null)
        {
            ResourcePool.Instance.LoadConfig(_SummonShowEffectPath, (resName, resGO, hash) =>
            {
                var hideEffectGO = resGO;
                _DesummonEffect = hideEffectGO.GetComponent<EffectSingle>();
            }, null);
            
        }
    }

    public void PlayShow()
    {
        InitEffect();
        
        _SummonEffect.transform.position = _SelfMotion.transform.position;
        _SummonEffect.PlayEffect();
    }

    public void PlayHide()
    {
        InitEffect();

        _DesummonEffect.transform.position = _SelfMotion.transform.position;
        _DesummonEffect.PlayEffect();
    }

    #endregion

    #region skill start speed

    private float _CurAnimSpeed;

    protected void InitSkillStarSpeed(ObjMotionSkillBase objMotionSkill)
    {
        if (objMotionSkill._NextAnim.Count == 0)
            return;

        float attackConlliderTime = _SelfMotion.AnimationEvent.GetAnimFirstColliderEventTime(objMotionSkill._NextAnim[0], objMotionSkill._SuperArmorColliderID);
        if (attackConlliderTime < 0)
        {
            attackConlliderTime = objMotionSkill._NextAnim[0].length;
        }

        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], 0, AttackStartForSpeed);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], attackConlliderTime + 0.05f, AttackColliderForSpeed);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], objMotionSkill._NextAnim[0].length, AttackColliderForSpeed);
    }

    private void AttackStartForSpeed()
    {
        //var animState = _SelfMotion.GetCurAnim();
        //if (animState == null)
        //    return;

        //_CurAnimSpeed = animState.speed;

        //var speed = _CurAnimSpeed * (1 + GameDataValue.ConfigIntToFloat( _SelfMotion.RoleAttrManager.GetBaseAttr(RoleAttrEnum.RiseHandSpeed)));
        //animState.speed = speed;

    }

    private void AttackColliderForSpeed()
    {
        //if (_CurAnimSpeed < 0)
        //    return;

        //var animState = _SelfMotion.GetCurAnim();
        //if (animState == null)
        //    return;

        //animState.speed = _CurAnimSpeed;
    }
    #endregion
}
