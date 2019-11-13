using UnityEngine;
using System.Collections;

public class AI_KeepDistance : AI_CloseAttack
{
    public float _AwayRange = 5;
    public float _AwayDistance = 3;

    private float _CloseWait;
    private float _AwayWait;

    protected override void Init()
    {
        base.Init();
    }

    protected override void AIUpdate()
    {
        //base.AIUpdate();

        if (_TargetMotion == null)
            return;

        if (!_AIAwake)
        {
            //float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
            float distance = GetPathLength(transform.position, _TargetMotion.transform.position);
            if (distance > _AlertRange)
                return;

            _AIAwake = true;
            AIManager.Instance.GroupAwake(GroupID);
        }

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (_SelfMotion.ActingSkill != null)
            return;

        //specil:do not attack when target lie on floor
        if (_TargetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.RISE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.FLY_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.HIT_PRIOR)
            return;

        if (StartSkill())
            return;

        float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        if (distance > _CloseRange)
        {
            if (_CloseWait > 0)
            {
                _CloseWait -= Time.fixedDeltaTime;
                return;
            }
            _SelfMotion.StartMoveState(_TargetMotion.transform.position);
        }
        else if (distance < _AwayRange)
        {
            if (_AwayWait > 0)
            {
                _AwayWait -= Time.fixedDeltaTime;
                return;
            }
            var back = _SelfMotion.transform.position - _TargetMotion.transform.position;
            var targetPos = back.normalized* _AwayRange;
            _SelfMotion.StartMoveState(targetPos);
            _AwayWait = _AwayDistance;
        }
        else
        {
            _SelfMotion.StopMoveState();

            StartSkill();
            _CloseWait = _CloseInterval;
        }
    }

}
