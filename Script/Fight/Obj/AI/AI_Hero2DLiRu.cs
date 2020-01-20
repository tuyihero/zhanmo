using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Hero2DLiRu : AI_HeroBase2D
{
    public int _JumpSkillIdx = -1;

    protected override void Init()
    {
        base.Init();

    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

    }

    private Vector3 _TargetPos;
    public Vector3 GetTargetPos()
    {
        return _TargetPos;
    }

    public override bool ActionMove()
    {
        //return true;
        //return ActionMoveZ();

        var targetPos = GetMovePos(this);
        if (targetPos == Vector3.zero)
            return true;

        StartSkill(_AISkills[_JumpSkillIdx]);
        MoveActionInterval = 1.0f;
        return true;
    }

    public Vector3 GetMovePos(AI_Base2D aiBase)
    {
        _TargetPos = Vector3.zero;
        float ZDistance = aiBase._TargetMotion.transform.position.z - transform.position.z;
        float XDistance = aiBase._TargetMotion.transform.position.x - transform.position.x;
        float randomPosZ = Random.Range(-0.2f, 0.2f);

        if (transform.position.x < FightManager.Instance._CameraFollow.MainMovePosXMin - 0.5f
            || (aiBase._TargetMotion.transform.position.x > transform.position.x && Mathf.Abs(aiBase._TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin) < aiBase._AttackDistanceX.x) && Mathf.Abs(ZDistance) < 0.3f)
        {
            _TargetPos = new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMin, aiBase._TargetMotion.transform.position.y, aiBase.GetOtherZ() + randomPosZ);

        }
        else if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMax + 0.5f
            || (aiBase._TargetMotion.transform.position.x < transform.position.x && Mathf.Abs(aiBase._TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMax) < aiBase._AttackDistanceX.x) && Mathf.Abs(ZDistance) < 0.3f)
        {
            _TargetPos = new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMax, aiBase._TargetMotion.transform.position.y, aiBase.GetOtherZ() + randomPosZ);
        }
        else
        {

            float randomPosX = Random.Range(aiBase._AttackDistanceX.x, aiBase._AttackDistanceX.y);
            if (Mathf.Abs(ZDistance) < 0.3f)
            {
                if (Mathf.Abs(XDistance) > aiBase._AttackDistanceX.y)
                {
                    if (XDistance > 0)
                    {
                        _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x - randomPosX + 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._TargetMotion.transform.position.z + randomPosZ);

                    }
                    else
                    {
                        _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x + randomPosX - 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._TargetMotion.transform.position.z + randomPosZ);

                    }
                }
                else if (Mathf.Abs(XDistance) < aiBase._AttackDistanceX.x)
                {
                    if (XDistance > 0)
                    {
                        if (aiBase._TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin > aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x - randomPosX - 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._TargetMotion.transform.position.z + randomPosZ);

                        }
                        else if (aiBase._TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin < aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x + randomPosX + 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._TargetMotion.transform.position.z + randomPosZ);

                        }
                    }
                    else
                    {
                        if (FightManager.Instance._CameraFollow.MainMovePosXMax - aiBase._TargetMotion.transform.position.x > aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x + randomPosX + 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._TargetMotion.transform.position.z + randomPosZ);

                        }
                        else if (FightManager.Instance._CameraFollow.MainMovePosXMax - aiBase._TargetMotion.transform.position.x < aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x - randomPosX - 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._TargetMotion.transform.position.z + randomPosZ);

                        }
                    }
                }

            }
            else
            {
                if (Mathf.Abs(XDistance) > aiBase._AttackDistanceX.y)
                {
                    if (XDistance > 0)
                    {
                        _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x - randomPosX + 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._SelfMotion.transform.position.z + randomPosZ);

                    }
                    else
                    {
                        _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x + randomPosX - 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._SelfMotion.transform.position.z + randomPosZ);

                    }
                }
                else if (Mathf.Abs(XDistance) < aiBase._AttackDistanceX.x)
                {
                    if (XDistance > 0)
                    {
                        if (aiBase._TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin > aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x - randomPosX - 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._SelfMotion.transform.position.z + randomPosZ);

                        }
                        else if (aiBase._TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMin < aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x + randomPosX + 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._SelfMotion.transform.position.z + randomPosZ);
                        }
                    }
                    else
                    {
                        if (FightManager.Instance._CameraFollow.MainMovePosXMax - aiBase._TargetMotion.transform.position.x > aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x + randomPosX + 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._SelfMotion.transform.position.z + randomPosZ);
                        }
                        else if (FightManager.Instance._CameraFollow.MainMovePosXMax - aiBase._TargetMotion.transform.position.x < aiBase._AttackDistanceX.x)
                        {
                            _TargetPos = new Vector3(aiBase._TargetMotion.transform.position.x - randomPosX - 0.1f, aiBase._TargetMotion.transform.position.y, aiBase._SelfMotion.transform.position.z + randomPosZ);
                        }
                    }
                }
                else
                {
                    _TargetPos = new Vector3(aiBase._SelfMotion.transform.position.x, aiBase._TargetMotion.transform.position.y, aiBase._TargetMotion.transform.position.z + randomPosZ);
                }

            }
        }

        if(_TargetPos != Vector3.zero)
            aiBase.MoveActionInterval = 2;

        return _TargetPos;
    }




}
