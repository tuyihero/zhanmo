using UnityEngine;
using System.Collections;

public class ImpactContract : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;
    public float _NearDis = 1.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {

        base.ActImpact(senderManager, reciverManager);

        Vector3 direct = - reciverManager.transform.position + senderManager.transform.position;
        float length = direct.magnitude - _NearDis;
        if (length < 0)
            return;

        float moveTime = length / _Speed;
        moveTime = Mathf.Min(_Time, moveTime);

        Vector3 destMove = direct.normalized * _Speed * moveTime;
        reciverManager.SetMove(destMove, moveTime / senderManager.RoleAttrManager.AttackSpeed);

    }

}
