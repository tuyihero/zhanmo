using UnityEngine;
using System.Collections;

public class ImpactCatch : ImpactHit
{

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        //base.ActImpact(senderManager, reciverManager);

        CatchMotion(senderManager, reciverManager);

        ProcessDamge(senderManager, reciverManager);
    }

    protected virtual void CatchMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        reciverManager.CatchEvent(_HitTime, _HitEffect, _HitAudio, senderManager, this, Vector3.zero, 0);
        //reciverManager.BaseMotionManager.CatchEvent(_HitTime, _HitEffect, senderManager, this);
    }

    protected virtual void CatchMotion(MotionManager senderManager, MotionManager reciverManager, Vector3 moveDirect, float moveTime)
    {
        //reciverManager.BaseMotionManager.CatchEvent(_HitTime, _HitEffect, senderManager, this);
        reciverManager.CatchEvent(_HitTime, _HitEffect, _HitAudio, senderManager, this, moveDirect, moveTime);
    }

    #region catch

    #endregion

}
