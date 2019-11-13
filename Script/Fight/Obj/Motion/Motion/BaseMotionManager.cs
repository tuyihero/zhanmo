using UnityEngine;
using System.Collections;

public class BaseMotionManager : MonoBehaviour
{
    public const int IDLE_PRIOR = 0;
    public const int MOVE_PRIOR = 10;
    public const int HIT_PRIOR = 1000;
    public const int FLY_PRIOR = 1001;
    public const int CATCH_PRIOR = 1002;
    public const int RISE_PRIOR = 999;
    public const int LIE_PRIOR = 998;
    public const int DIE_PRIOR = 2000;

    protected MotionManager _MotionManager;

    public bool IsCanHit { get; set; }

    public void Init()
    {
        _MotionManager = gameObject.GetComponent<MotionManager>();
        _NavAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        _MotionManager.AddAnimationEndEvent(_HitAnim);
        _MotionManager.AddAnimationEndEvent(_FlyAnim);
        _MotionManager.AddAnimationEndEvent(_RiseAnim);
        _FlyBody = _MotionManager.AnimationEvent.gameObject;

        _MotionManager.InitAnimation(_IdleAnim);
        _MotionManager.InitAnimation(_MoveAnim);
        _MotionManager.InitAnimation(_HitAnim);
        _MotionManager.InitAnimation(_FlyAnim);
        _MotionManager.InitAnimation(_RiseAnim);
    }

    public void HitEvent(float hitTime, int hitEffect, MotionManager impactSender, ImpactHit hitImpact)
    {
        if (!_MotionManager.IsBuffCanHit(impactSender, hitImpact))
        {
            IsCanHit = false;
            return;
        }
        IsCanHit = true;

        Hashtable eventHash = new Hashtable();
        eventHash.Add("Sender", impactSender);
        if (_MotionManager.MotionPrior == FLY_PRIOR || _MotionManager.MotionPrior == LIE_PRIOR)
        {

            MotionFlyStay(hitTime, hitEffect, impactSender);
        }
        else if(_MotionManager.MotionPrior <= HIT_PRIOR)
        {

            MotionHit(hitTime, hitEffect, impactSender);
        }
         
    }

    public void FlyEvent(float flyHeight, int hitEffect, MotionManager impactSender, ImpactHit hitImpact)
    {
        if (!_MotionManager.IsBuffCanHit(impactSender, hitImpact))
        {
            IsCanHit = false;
            return;
        }
        IsCanHit = true;

        if (_MotionManager.MotionPrior > FLY_PRIOR)
            return;

        Hashtable eventHash = new Hashtable();
        eventHash.Add("Sender", impactSender);

        MotionFly(flyHeight, hitEffect, impactSender);
    }

    public bool IsCanBePush()
    {
        if (!IsCanHit)
            return false;

        if (_MotionManager.MotionPrior == CATCH_PRIOR)
            return false;

        return true;
    }

    public void CatchEvent(float catchTime, int hitEffect, MotionManager impactSender, ImpactCatch impactCatch)
    {
        if (!_MotionManager.IsBuffCanCatch(impactSender, impactCatch))
        {
            return;
        }

        if (_MotionManager.MotionPrior > CATCH_PRIOR)
            return;

        MotionCatch(catchTime, hitEffect, impactSender);
    }

    public void DispatchAnimEvent(string funcName, object param)
    {
        switch (_MotionManager.MotionPrior)
        {
            case HIT_PRIOR:
                DispatchHitEvent(funcName, param);
                break;
            case CATCH_PRIOR:
                DispatchCatchEvent(funcName, param);
                break;
            case RISE_PRIOR:
                DispatchRiseEvent(funcName, param);
                break;
            case FLY_PRIOR:
                DispatchFlyEvent(funcName, param);
                break;
        }
    }

    void FixedUpdate()
    {
        UpdateMove();
        FlyUpdate();
    }

    public void PauseMotion()
    { }

    #region idle

    public AnimationClip _IdleAnim;

    public bool IsMotionIdle()
    {
        if (_MotionManager.MotionPrior == IDLE_PRIOR)
            return true;

        return false;
    }

    public bool CanMotionIdle()
    {
        if (_MotionManager.MotionPrior != MOVE_PRIOR && _MotionManager.MotionPrior > IDLE_PRIOR)
            return false;

        if (_MotionManager.ActingSkill!= null)
            return false;

        return true;
    }

    public void MotionIdle()
    {
        _MotionManager.MotionPrior = IDLE_PRIOR;
        _MotionManager.PlayAnimation(_IdleAnim);
    }

    #endregion

    #region move

    public AnimationClip _MoveAnim;

    private UnityEngine.AI.NavMeshAgent _NavAgent;

    public bool IsMoving()
    {
        return _MotionManager.MotionPrior == MOVE_PRIOR;
    }

    public bool CanMotionMove()
    {
        if (_MotionManager.MotionPrior > MOVE_PRIOR)
            return false;

        if (_MotionManager.ActingSkill!= null)
            return false;

        return true;
    }

    public void MoveDirect(Vector2 direct)
    {

        Vector3 derectV3 = new Vector3(direct.x, 0, direct.y);

        MoveDirect(derectV3);
    }

    public void MoveDirect(Vector3 derectV3)
    {
        _MotionManager.MotionPrior = MOVE_PRIOR;
        Vector3 destPoint = transform.position + derectV3.normalized;

        _MotionManager.PlayAnimation(_MoveAnim, _MotionManager.RoleAttrManager.MoveSpeedRate);
        _MotionManager.transform.rotation = Quaternion.LookRotation(derectV3);
        _NavAgent.speed = _MotionManager.RoleAttrManager.MoveSpeed;
        _NavAgent.SetDestination(destPoint);
        //NavMeshHit navHit = new NavMeshHit();
        //if (!NavMesh.SamplePosition(destPoint, out navHit, 5, NavMesh.AllAreas))
        //{
        //    return;
        //}
        //_NavAgent.Warp(destPoint);
    }

    public void MoveTarget(Vector3 targetPos)
    {
        if (!CanMotionMove())
            return;

        _MotionManager.MotionPrior = MOVE_PRIOR;
        _MotionManager.PlayAnimation(_MoveAnim, _MotionManager.RoleAttrManager.MoveSpeedRate);

        _NavAgent.speed = _MotionManager.RoleAttrManager.MoveSpeed;
        _NavAgent.SetDestination(targetPos);
    }

    public void StopMove()
    {
        _NavAgent.Stop();
        _NavAgent.ResetPath();
        if (CanMotionIdle())
            MotionIdle();
    }

    #endregion

    #region push

    private Vector3 _TargetVec;
    private float _LastTime;
    private float _Speed;

    public void SetRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.LookRotation(rotate);
    }

    public void SetMove(Vector3 moveVec, float lastTime)
    {
        if (_NavAgent == null)
        {
            _NavAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        _TargetVec += moveVec;
        _LastTime = lastTime;
        _Speed = _TargetVec.magnitude / _LastTime;
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

        _NavAgent.Warp(transform.position + moveVec);
    }

    #endregion

    #region hit

    public AnimationClip _HitAnim;

    private float _StopKeyFrameTime = 0.0f;

    private void DispatchHitEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.KEY_FRAME:
                HitKeyframe(param);
                break;
            case AnimEventManager.ANIMATION_END:
                HitEnd();
                break;
        }
    }

    public void MotionHit(float hitTime, int hitEffect, MotionManager impactSender)
    {
        PlayHitEffect(impactSender, hitEffect);
        if (hitTime <= 0)
            return;

        if (_MotionManager.ActingSkill!= null)
            _MotionManager.ActingSkill.FinishSkill();

        if(_MotionManager.MotionPrior == MOVE_PRIOR)
            StopMove();

        if (hitTime > _HitAnim.length)
        {
            _StopKeyFrameTime = hitTime - _HitAnim.length;
        }
        else
        {
            _StopKeyFrameTime = 0;
        }
        _MotionManager.MotionPrior = HIT_PRIOR;
        StopAllCoroutines();
        _MotionManager.RePlayAnimation(_HitAnim, 1);
        //_MotionManager.SetLookAt(impactSender.transform.position);
    }

    public void HitKeyframe(object param)
    {
        if (_StopKeyFrameTime > 0)
        {
            _MotionManager.PauseAnimation(_HitAnim, -1);
            StartCoroutine(ComsumeAnim());
        }
    }

    public IEnumerator ComsumeAnim()
    {
        yield return new WaitForSeconds(_StopKeyFrameTime);

        _MotionManager.ResumeAnimation(_HitAnim);
    }

    public void HitEnd()
    {
        _MotionManager.MotionPrior = IDLE_PRIOR;
    }

    protected void PlayHitEffect(MotionManager impactSender, int effectIdx)
    {
        if (ResourcePool.Instance._CommonHitEffect.Count > effectIdx && effectIdx >= 0)
        {
            Hashtable hash = new Hashtable();
            if(impactSender != null)
                hash.Add("Rotation", Quaternion.LookRotation( impactSender.transform.position - transform.position, Vector3.zero));

            _MotionManager.PlayDynamicEffect(ResourcePool.Instance._CommonHitEffect[effectIdx], hash);
        }
    }

    #endregion

    #region fly

    private const float _UpSpeed = 15;
    private const float _DownSpeed = 10;
    private const float _LieTimeStatic = 0.6f;
    private const float _CorpseTimeStatic = 0.01f;

    public AnimationClip _FlyAnim;

    private GameObject _FlyBody;
    private float _FlyHeight = 0;
    private float _StayTime = 0;
    private float _LieTime = 0;

    private bool IsFlyEnd = false;

    private void DispatchFlyEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.ANIMATION_END:
                FlyEnd();
                break;
        }
    }

    private void FlyEnd()
    {
        IsFlyEnd = true;
    }

    public void MotionFly(float flyHeight, int effectID, MotionManager impactSender)
    {
        Debug.Log("MotionFly");
        PlayHitEffect(impactSender, effectID);

        if (_MotionManager.ActingSkill!= null)
            _MotionManager.ActingSkill.FinishSkill();

        if (_MotionManager.MotionPrior == MOVE_PRIOR)
            StopMove();

        _MotionManager.MotionPrior = FLY_PRIOR;
        _MotionManager.RePlayAnimation(_FlyAnim, 1);

        _FlyHeight = flyHeight;

        IsFlyEnd = false;
        _MotionManager.SetCorpsePrior();
        //if(impactSender != null)
        //    _MotionManager.SetLookAt(impactSender.transform.position);
    }

    public void MotionFlyStay(float time, int effectID, MotionManager impactSender)
    {
        Debug.Log("MotionFlyStay");
        PlayHitEffect(impactSender, effectID);

        _MotionManager.MotionPrior = FLY_PRIOR;
        _MotionManager.RePlayAnimation(_FlyAnim, 1);

        _StayTime = time;
    }

    public void FlyUpdate()
    {
        if (_StayTime > 0)
        {
            _StayTime -= Time.fixedDeltaTime;
        }
        else if (_FlyHeight > 0)
        {
            _FlyBody.transform.localPosition += _UpSpeed * Time.fixedDeltaTime * Vector3.up;

            if (_FlyBody.transform.localPosition.y > _FlyHeight)
            {
                _FlyBody.transform.localPosition = new Vector3(0, _FlyHeight, 0);
                _FlyHeight = 0;
            }
        }
        else if (_FlyBody.transform.localPosition.y > 0)
        {
            _FlyBody.transform.localPosition -= _DownSpeed * Time.fixedDeltaTime * Vector3.up;
            if (_FlyBody.transform.localPosition.y < 0)
            {
                _FlyBody.transform.localPosition = Vector3.zero;
                if (_MotionManager.IsMotionDie)
                {
                    _LieTime = _CorpseTimeStatic;
                }
                else
                {
                    _LieTime = _LieTimeStatic;
                }
            }

        }
        else if (!IsFlyEnd)
        {
            
        }
        else if (_LieTime > 0)
        {
            _MotionManager.MotionPrior = LIE_PRIOR;
            _LieTime -= Time.fixedDeltaTime;
            if (_LieTime <= 0)
            {
                if (_MotionManager.IsMotionDie)
                {
                    Debug.Log("MotionDie:" + Time.time);
                    _MotionManager.MotionPrior = DIE_PRIOR;
                    StartCoroutine(MotionCorpse());
                }
                else
                {
                    Debug.Log("Motion rise");
                    MotionRise();
                }
            }
        }
        
    }

    #endregion

    #region rise

    public AnimationClip _RiseAnim;
    public float _BodyDisappearTime = 1f;
    


    private void MotionRise()
    {
        _MotionManager.MotionPrior = RISE_PRIOR;
        _MotionManager.PlayAnimation(_RiseAnim);

    }

    private IEnumerator MotionCorpse()
    {
        _MotionManager.MotionCorpse();

        yield return new WaitForSeconds(_BodyDisappearTime);

        _MotionManager.MotionDisappear();
    }

    private void MotionFall()
    {
        if (_MotionManager.IsMotionDie)
        {
            _MotionManager.MotionPrior = DIE_PRIOR;
            StartCoroutine(MotionCorpse());
        }
        else if(_MotionManager.MotionPrior == FLY_PRIOR)
        {
            MotionRise();
        }
        
    }

    private void RiseEnd()
    {
        MotionIdle();
        _MotionManager.ResumeCorpsePrior();

    }

    private void DispatchRiseEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.ANIMATION_END:
                RiseEnd();
                break;
        }
    }
    #endregion

    #region catch

    private float _StopCatchTime = 0;

    public void MotionCatch(float hitTime, int hitEffect, MotionManager impactSender)
    {
        PlayHitEffect(impactSender, hitEffect);
        if (hitTime <= 0)
            return;

        if (_MotionManager.ActingSkill!= null)
            _MotionManager.ActingSkill.FinishSkill();

        if (_MotionManager.MotionPrior == MOVE_PRIOR)
            StopMove();

        if (hitTime > _HitAnim.length)
        {
            _StopCatchTime = hitTime - _HitAnim.length;
        }
        else
        {
            _StopCatchTime = 0;
        }
        _MotionManager.ResetMove();
        _MotionManager.MotionPrior = CATCH_PRIOR;
        StopAllCoroutines();
        //StartCoroutine(StopCatch(hitTime));
        _MotionManager.RePlayAnimation(_HitAnim, 1);

    }

    private void DispatchCatchEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.KEY_FRAME:
                CatchKeyframe(param);
                break;
            case AnimEventManager.ANIMATION_END:
                FinishCatch();
                break;
        }
    }

    public void CatchKeyframe(object param)
    {
        if (_StopCatchTime > 0)
        {
            _MotionManager.PauseAnimation(_HitAnim, -1);
            StartCoroutine(StopCatch(_StopCatchTime));
        }
    }

    private IEnumerator StopCatch(float catchTime)
    {
        yield return new WaitForSeconds(catchTime);
        MotionFly(0.1f, 0, null);
    }

    public void FinishCatch()
    {
        if (_MotionManager.MotionPrior == CATCH_PRIOR)
        {
            MotionIdle();
            StopAllCoroutines();
        }
    }

    #endregion
}
