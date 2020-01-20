using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Car2D : AI_Base2D
{
    

    protected override void Init()
    {
        base.Init();

        
    }

    protected override void AIUpdate()
    {
        //base.AIUpdate();

        if (_TargetMotion == null)
            return;

        if (_SelfMotion._ActionPause)
            return;

        CarUpdate();
    }



    #region car move

    public float _CarLength;

    private void CarUpdate()
    {
        if (_SelfMotion._ActionState == _SelfMotion._StateIdle
            || _SelfMotion._ActionState == _SelfMotion._StateMove)
        {
            if (_SelfMotion.GetMotionForward().x < 0)
            {
                if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMax + 0.5f - _CarLength * 0.1f && transform.position.x < FightManager.Instance._CameraFollow.MainMovePosXMax + 0.5f - _CarLength * 0.5f)
                {
                    _SelfMotion.StopMoveState();
                    StartSkill();
                }
                else if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMin - _CarLength - 0.5f)
                {
                    var targetPos = new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMin - _CarLength - 1f, transform.position.y, transform.position.z);
                    SetMove(targetPos);
                }
                else if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMin - _CarLength - 1f)
                {
                    _SelfMotion.SetLookAt(_TargetMotion.transform.position);
                    var targetPos = new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMin - 0.5f + _CarLength * 0.5f, transform.position.y, transform.position.z);
                    SetMove(targetPos);
                }
                else
                {
                    StartSkill();
                }
            }
            else if (_SelfMotion.GetMotionForward().x > 0)
            {
                if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMin - 0.5f + _CarLength * 0.1f && transform.position.x < FightManager.Instance._CameraFollow.MainMovePosXMin - 0.5f + _CarLength*0.5f)
                {
                    _SelfMotion.StopMoveState();
                    StartSkill();
                }
                else if (transform.position.x < FightManager.Instance._CameraFollow.MainMovePosXMax + _CarLength + 0.5f)
                {
                    var targetPos = new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMax + _CarLength + 1f, transform.position.y, transform.position.z);
                    SetMove(targetPos);
                }
                else if (transform.position.x > FightManager.Instance._CameraFollow.MainMovePosXMax + _CarLength + 0.5f )
                {
                    _SelfMotion.SetLookAt(_TargetMotion.transform.position);
                    var targetPos = new Vector3(FightManager.Instance._CameraFollow.MainMovePosXMax + 0.5f - _CarLength * 0.5f, transform.position.y, transform.position.z);
                    SetMove(targetPos);
                }
                else
                {
                    StartSkill();
                }
            }
        }
        
    }

    protected override void StartSkill(AI_Skill_Info skillInfo, bool isIgnoreCD = false)
    {
        if (!skillInfo.SkillBase.IsCanActSkill())
            return;

        //_SelfMotion.SetLookAt(_TargetMotion.transform.position);
        _SelfMotion.ActSkill(skillInfo.SkillBase);
        SetSkillCD(skillInfo, skillInfo.SkillInterval);
    }

    #endregion

}
