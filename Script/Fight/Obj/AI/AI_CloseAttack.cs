using UnityEngine;
using System.Collections;

public class AI_CloseAttack : AI_Base
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

        if (StartSkill())
            return;

        IsActMove();
        
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

        return base.StartSkill();
    }

    #endregion
    
}
