using UnityEngine;
using System.Collections;

public class ImpactPushToPos2D : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    private Vector3 _TargetPos = Vector3.zero;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var baseAI = SenderMotion.GetComponent<AI_Hero2DLiRu>();
        var targetPos = baseAI.GetTargetPos();
        if (targetPos == Vector3.zero)
            return;

        Vector3 direct = targetPos - senderManager.transform.position;
        senderManager.SetMove(direct, _Time / senderManager.RoleAttrManager.MoveSpeedRate);
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

            aiBase.MoveActionInterval = 2;

        }
        else if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMax + 0.5f
            || (aiBase._TargetMotion.transform.position.x < transform.position.x && Mathf.Abs(aiBase._TargetMotion.transform.position.x - FightManager.Instance._CameraFollow.MainMovePosXMax) < aiBase._AttackDistanceX.x) && Mathf.Abs(ZDistance) < 0.3f)
        {
            _TargetPos = new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMax, aiBase._TargetMotion.transform.position.y, aiBase.GetOtherZ() + randomPosZ);
            
            aiBase.MoveActionInterval = 2;
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

        return _TargetPos;
    }

}
