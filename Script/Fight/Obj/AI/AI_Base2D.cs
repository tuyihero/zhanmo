using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Base2D : AI_Base
{
    

    protected override void Init()
    {
        base.Init();

        
    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_TargetMotion == null)
            return;

        if (_SelfMotion._ActionPause)
            return;

        ActionUpdate();
    }
    
    public override void OnStateChange(StateBase orgState, StateBase newState)
    {
        base.OnStateChange(orgState, newState);

        MoveState(orgState, newState);
    }

    #region done attack lie

    protected override bool StartSkill()
    {
        if (_TargetMotion == null)
            return false;

        if (_TargetMotion._ActionState is StateCatch
            || _TargetMotion._ActionState is StateFly
            || _TargetMotion._ActionState is StateHit
            || _TargetMotion._ActionState is StateLie)
        {
            return false;
        }

        if (!IsRandomActSkill())
            return false;

        float dis = Mathf.Abs(_SelfMotion.transform.position.x-_TargetMotion.transform.position.x);

        for (int i = _AISkills.Count - 1; i >= 0; --i)
        {
            if (!_AISkills[i].ActOutCamera && !IsInCamera())
                continue;

            if (!_AISkills[i].ActDiffZ && IsDiffZ())
                continue;

            if (!_AISkills[i].IsSkillCD())
                continue;

            //if (!IsCommonCD())
            //    continue;

            if (_AISkills[i].SkillRange.y < dis || _AISkills[i].SkillRange.x > dis)
                continue;

            StartSkill(_AISkills[i]);
            return true;

        }

        return false;
    }

    #endregion

    #region AIActions

    private static float _MoveActionIntervalConst = 0.5f;
    private float _MoveActionInterval = 1f;
    public float MoveActionInterval
    {
        get
        {
            return _MoveActionInterval;
        }
        set
        {
            _MoveActionInterval = value;
        }
    }

    protected float _MoveWaitTime = 0;

    public void ActionUpdate()
    {

        //if (IsInCamera())
        {
            if (StartSkill())
                return;
        }

        if (Time.time - _MoveWaitTime > _MoveActionInterval)
        {
            _MoveActionInterval = _MoveActionIntervalConst;

            if (_SelfMotion.ActingSkill != null)
                return;

            if (ActionMove())
            {
                _MoveWaitTime = Time.time;
            }
        }
    }

    #endregion

    #region AIActionMove

    public Vector2 _AttackDistanceX;
    public bool _MoveZ = true;

    public float GetOtherZ()
    {
        if (Mathf.Abs(_TargetMotion.transform.position.z - FightManager.Instance._CameraFollow._SceneAnimController.SceneZMin) >
            Mathf.Abs(_TargetMotion.transform.position.z - FightManager.Instance._CameraFollow._SceneAnimController.SceneZMax))
        {
            return _TargetMotion.transform.position.z - 2.5f;
        }
        else
        {
            return _TargetMotion.transform.position.z + 2.5f;
        }
    }

    public bool IsDiffZ()
    {
        return Mathf.Abs(_TargetMotion.transform.position.z - transform.position.z) > 0.3f;
    }

    public bool IsInCamera()
    {
        if (transform.position.x > FightManager.Instance._CameraFollow.transform.position.x - FightManager.Instance._CameraFollow.MinPosX
            && transform.position.x < FightManager.Instance._CameraFollow.transform.position.x + FightManager.Instance._CameraFollow.MinPosX)
            return true;
        return false;
    }

    public virtual bool ActionMove()
    {
        if (_MoveZ)
            return ActionMoveZ();
        else
            return ActionMoveX();
    }

    public virtual bool ActionMoveX()
    {
        float randomPosX = Random.Range(_AttackDistanceX.x, _AttackDistanceX.y);
        if (transform.position.x < FightManager.Instance._CameraFollow.MainMovePosXMin)
        {
            _SelfMotion.StartMoveState(new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMin, transform.position.y, transform.position.z), _TargetMotion.transform);
        }
        else if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMax)
        {
            _SelfMotion.StartMoveState(new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMax, transform.position.y, transform.position.z), _TargetMotion.transform);
        }

        if (!IsDiffZ())
        {
            float XDistance = _TargetMotion.transform.position.x - transform.position.x;
            if ((Mathf.Abs(XDistance) > _AttackDistanceX.y || Mathf.Abs(XDistance) < _AttackDistanceX.x))
            {
                if (transform.position.x < _TargetMotion.transform.position.x)
                {
                    if (_TargetMotion.transform.position.x - randomPosX < FightManager.Instance._CameraFollow.MainMovePosXMin)
                    {
                        _SelfMotion.StartMoveState(new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMin, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                    else
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                }
                else
                {
                    if (_TargetMotion.transform.position.x + randomPosX > FightManager.Instance._CameraFollow.MainMovePosXMax)
                    {
                        _SelfMotion.StartMoveState(new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMax, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                    else
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                }
            }
        }
        else
        {
            float XDistance = _TargetMotion.transform.position.x - transform.position.x;
            if ((Mathf.Abs(XDistance) > _AttackDistanceX.y || Mathf.Abs(XDistance) < _AttackDistanceX.x))
            {
                if (transform.position.x < _TargetMotion.transform.position.x)
                {
                    if (_TargetMotion.transform.position.x - randomPosX < FightManager.Instance._CameraFollow.MainMovePosXMin)
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                    else
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                }
                else
                {
                    if (_TargetMotion.transform.position.x + randomPosX > FightManager.Instance._CameraFollow.MainMovePosXMax)
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                    else
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX, transform.position.y, transform.position.z), _TargetMotion.transform);
                    }
                }
            }

        }

        return true;
    }

    public virtual bool ActionMoveZ()
    {
        float randomPosZ = Random.Range(-0.2f, 0.2f);
        float ZDistance = _TargetMotion.transform.position.z - transform.position.z;
        float XDistance = _TargetMotion.transform.position.x - transform.position.x;
        if (transform.position.x < FightManager.Instance._CameraFollow.MainMovePosXMin - 0.5f
            || (_TargetMotion.transform.position.x > transform.position.x && Mathf.Abs(_TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin) < _AttackDistanceX.x) && Mathf.Abs(ZDistance) < 0.3f)
        {
            _SelfMotion.StartMoveState(new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMin, _TargetMotion.transform.position.y, GetOtherZ() + randomPosZ), _TargetMotion.transform);
            _MoveActionInterval = 2;
        }
        else if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMax + 0.5f
            || (_TargetMotion.transform.position.x < transform.position.x && Mathf.Abs(_TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMax) < _AttackDistanceX.x) && Mathf.Abs(ZDistance) < 0.3f)
        {
            _SelfMotion.StartMoveState(new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMax, _TargetMotion.transform.position.y, GetOtherZ() + randomPosZ), _TargetMotion.transform);
            _MoveActionInterval = 2;
        }
        else
        {
            
            float randomPosX = Random.Range(_AttackDistanceX.x, _AttackDistanceX.y);
            if (Mathf.Abs(ZDistance) < 0.3f)
            {
                if (Mathf.Abs(XDistance) > _AttackDistanceX.y)
                {
                    if (XDistance > 0)
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX + 0.1f, _TargetMotion.transform.position.y, _TargetMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                    }
                    else
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX - 0.1f, _TargetMotion.transform.position.y, _TargetMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                    }
                }
                else if (Mathf.Abs(XDistance) < _AttackDistanceX.x)
                {
                    if (XDistance > 0)
                    {
                        if (_TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin > _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX - 0.1f, _TargetMotion.transform.position.y, _TargetMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                        else if (_TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin < _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX + 0.1f, _TargetMotion.transform.position.y, _TargetMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                    }
                    else
                    {
                        if (FightManager.Instance._CameraFollow.MainMovePosXMax - _TargetMotion.transform.position.x > _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX + 0.1f, _TargetMotion.transform.position.y, _TargetMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                        else if (FightManager.Instance._CameraFollow.MainMovePosXMax - _TargetMotion.transform.position.x < _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX - 0.1f, _TargetMotion.transform.position.y, _TargetMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                    }
                }
                else
                {
                    _SelfMotion.StopMoveState();
                    return false;
                }
            }
            else
            {
                if (Mathf.Abs(XDistance) > _AttackDistanceX.y)
                {
                    if (XDistance > 0)
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX + 0.1f, _TargetMotion.transform.position.y, _SelfMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                    }
                    else
                    {
                        _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX - 0.1f, _TargetMotion.transform.position.y, _SelfMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                    }
                }
                else if (Mathf.Abs(XDistance) < _AttackDistanceX.x)
                {
                    if (XDistance > 0)
                    {
                        if (_TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin > _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX - 0.1f, _TargetMotion.transform.position.y, _SelfMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                        else if (_TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin < _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX + 0.1f, _TargetMotion.transform.position.y, _SelfMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                    }
                    else
                    {
                        if (FightManager.Instance._CameraFollow.MainMovePosXMax - _TargetMotion.transform.position.x > _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x + randomPosX + 0.1f, _TargetMotion.transform.position.y, _SelfMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                        else if (FightManager.Instance._CameraFollow.MainMovePosXMax - _TargetMotion.transform.position.x < _AttackDistanceX.x)
                        {
                            _SelfMotion.StartMoveState(new Vector3(_TargetMotion.transform.position.x - randomPosX - 0.1f, _TargetMotion.transform.position.y, _SelfMotion.transform.position.z + randomPosZ), _TargetMotion.transform);
                        }
                    }
                }
                else
                {
                    _SelfMotion.StartMoveState(new Vector3(_SelfMotion.transform.position.x, _TargetMotion.transform.position.y, _TargetMotion.transform.position.z + randomPosZ), _TargetMotion.transform, 1.4f);
                }

            }
        }

        return true;
    }

    #endregion


    

}
