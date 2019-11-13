using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AI_EliteKeepDistance : AI_EliteBase
{
    public float _DangerAngle = 45;
    public float _WornAngle = 90;
    public float _BackAngleRange = 30;
    public float _MoveInterval = 2.0f;

    protected override void AIUpdate()
    {
        base.AIUpdate();
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
        if (!IsCancelNormalAttack && _SelfMotion.ActingSkill != null)
            return;

        //specil:do not attack when target lie on floor
        if (_TargetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.RISE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.FLY_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.HIT_PRIOR)
            return;

        if (StartSkill())
            return;

        if (!IsActDistanceMove())
        {
            StartSkill();
        }
    }

    private float _LastMoveTime;
    private Vector3 _LastMoeToPos;
    public bool IsActDistanceMove()
    {
        float disToTar = Vector3.Distance(_LastMoeToPos, _SelfMotion.transform.position);
        if (disToTar < 0.5f)
        {
            _SelfMotion.StopMoveState();
            _SelfMotion.transform.LookAt(_TargetMotion.transform.position);
        }

        if (Time.time - _LastMoveTime < _MoveInterval)
            return false;

        _LastMoveTime = Time.time;

        float moveAngle = 0;
        float targetAngle = Vector3.Angle(_SelfMotion.transform.position - _TargetMotion.transform.position, _TargetMotion.transform.forward);
        if (targetAngle > 0)
        {
            moveAngle = targetAngle + _BackAngleRange;
        }
        else
        {
            moveAngle = targetAngle - _BackAngleRange;
        }

        var rot = new Vector3(0, _TargetMotion.transform.rotation.eulerAngles.y + moveAngle, 0);
        _SelfMotion.transform.rotation = Quaternion.Euler(rot);
        Vector3 targetPos = _TargetMotion.transform.position + _SelfMotion.transform.forward * _CloseRange;

        NavMeshHit navmeshHit;
        if (NavMesh.SamplePosition(targetPos, out navmeshHit, 1000, 1))
        {
            _LastMoeToPos = navmeshHit.position;
        }
        _SelfMotion.StartMoveState(_LastMoeToPos);

        return true;
    }


}

