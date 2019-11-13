using UnityEngine;
using System.Collections;

public class ImpactFlyToPos : ImpactFly
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;
    public Vector2 _Offset = Vector3.zero;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        Vector3 hitPos = senderManager.transform.position + senderManager.transform.forward * _Offset.x + reciverManager.transform.right * _Offset.y;
        Vector3 distance = hitPos - reciverManager.transform.position;

        float moveTime = _Time;
        float targetTime = distance.magnitude / _Speed;
        if (targetTime < _Time)
        {
            moveTime = targetTime;
        }

        if (moveTime > 0.01f)
        {
            Vector3 destMove = (hitPos - reciverManager.transform.position).normalized * _Speed * moveTime;
            HitFlyMotionWithoutSpeed(senderManager, reciverManager, destMove, moveTime);
        }

        ProcessDamge(senderManager, reciverManager);

    }

}
