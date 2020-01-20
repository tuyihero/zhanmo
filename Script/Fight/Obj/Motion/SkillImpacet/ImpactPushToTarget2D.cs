using UnityEngine;
using System.Collections;

public class ImpactPushToTarget2D : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;
    public float _NearDis = 1.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        Vector3 targetPos = new Vector3( reciverManager.transform.position.x - _NearDis, reciverManager.transform.position.y, reciverManager.transform.position.z);
        if (senderManager.transform.position.x > reciverManager.transform.position.x)
        {
            targetPos = new Vector3(reciverManager.transform.position.x + _NearDis, reciverManager.transform.position.y, reciverManager.transform.position.z);
        }


        Vector3 direct = targetPos - senderManager.transform.position;
        //float length = direct.magnitude - _NearDis;
        //if (length < 0)
        //    return;

        float moveTime = _Time;

        Vector3 destMove = direct;
        senderManager.SetMove(destMove, moveTime / senderManager.RoleAttrManager.AttackSpeed);
    }

}
