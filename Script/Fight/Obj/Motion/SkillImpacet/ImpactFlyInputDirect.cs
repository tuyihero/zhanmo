using UnityEngine;
using System.Collections;

public class ImpactFlyInputDirect : ImpactFly
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        InitActImpact(senderManager, reciverManager);

        Vector3 moveDirect = new Vector3(InputManager.Instance.CameraAxis.x, 0, InputManager.Instance.CameraAxis.y);
        if (moveDirect == Vector3.zero)
        {
            moveDirect = senderManager.GetMotionForward().normalized;
        }
        Vector3 destMove = moveDirect * _Speed * _Time;

        HitFlyMotion(senderManager, reciverManager, destMove, _Time);

        ProcessDamge(senderManager, reciverManager);

    }

}
