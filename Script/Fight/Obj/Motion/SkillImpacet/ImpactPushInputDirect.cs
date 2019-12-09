using UnityEngine;
using System.Collections;

public class ImpactPushInputDirect : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    private float _ExSpeed = 0;
    public float ExSpeed
    {
        get
        {
            return _ExSpeed;
        }
        set
        {
            _ExSpeed = value;
        }
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        Vector3 moveDirect = new Vector3(InputManager.Instance.CameraAxis.x, 0, InputManager.Instance.CameraAxis.y);
        if (moveDirect == Vector3.zero)
        {
            moveDirect = senderManager.GetMotionForward().normalized;
        }
        Vector3 destMove = moveDirect * (_Speed + ExSpeed) * _Time;
        reciverManager.SetLookRotate(moveDirect);
        reciverManager.SetMove(destMove, _Time / SkillMotion.SkillSpeed);
    }

}
