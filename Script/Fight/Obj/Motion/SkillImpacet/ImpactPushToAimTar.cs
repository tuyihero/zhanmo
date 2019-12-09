using UnityEngine;
using System.Collections;

public class ImpactPushToAimTar : ImpactPushToTarget
{

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {

        if (AimTarget.Instance != null && AimTarget.Instance.LockTarget != null)
        {
            base.ActImpact(senderManager, AimTarget.Instance.LockTarget);
        }
        else
        {
            Vector3 destMove = senderManager.GetMotionForward().normalized * _Speed * _Time;
            reciverManager.SetMove(destMove, _Time / senderManager.RoleAttrManager.AttackSpeed);
        }
    }

}
