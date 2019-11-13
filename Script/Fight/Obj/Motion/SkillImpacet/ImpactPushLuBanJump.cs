using UnityEngine;
using System.Collections;

public class ImpactPushLuBanJump : ImpactBase
{
    public float _Distance = -1;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        int randomAngle = Random.Range(90, 270);
        reciverManager.SetRotate(reciverManager.transform.forward + new Vector3(0, randomAngle, 0));
    }

}
