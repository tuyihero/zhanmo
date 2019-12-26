using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletLine2D : BulletBase
{
    public float _LifeTime = 2.0f;
    public float _Speed = 10;
    public int _HitTimes = 1;

    protected float _AwakeTime = 0;
    protected int _AlreadyHitTimes = 0;
    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _AlreadyHitTimes = 0;
        _AwakeTime = Time.time;
        if (_HittedMotions != null)
        {
            _HittedMotions.Clear();
        }
    }

    private IEnumerator FinishDelay()
    {
        yield return new WaitForSeconds(_LifeTime);

        BulletFinish();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (_SkillMotion._ActionPause)
            return;

        MoveUpdate();
    }

    protected void MoveUpdate()
    {
        var motionForward = MotionManager.GetForward2D(transform.rotation.eulerAngles);
        transform.position += motionForward * _Speed * Time.fixedDeltaTime;

        RefreshSpriteRot();

        if (Time.time - _AwakeTime > _LifeTime)
        {
            BulletFinish();
        }
    }

    void OnTriggerStay(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        TriggetMotion(targetMotion);
    }

    protected void TriggetMotion(MotionManager targetMotion)
    {
        if (!_IsBulletHitLie && (targetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR || targetMotion.MotionPrior == BaseMotionManager.RISE_PRIOR))
            return;

        if (_HittedMotions != null)
        {
            if (_HittedMotions.Contains(targetMotion))
            {
                return;
            }
            else
            {
                _HittedMotions.Add(targetMotion);
            }
        }

        BulletHit(targetMotion);
        ++_AlreadyHitTimes;

        if (_HitTimes > 0 && _AlreadyHitTimes >= _HitTimes)
        {
            BulletFinish();
        }
    }

    #region hit motion once

    public List<MotionManager> _HittedMotions;

    #endregion
}
