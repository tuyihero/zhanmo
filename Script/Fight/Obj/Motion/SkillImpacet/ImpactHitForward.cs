using UnityEngine;
using System.Collections;

public class ImpactHitForward : ImpactHit
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        //base.ActImpact(senderManager, reciverManager);

        //HitMotion(senderManager, reciverManager);
        //Debug.Log("ActImpact ImpactHitForward");
        InitActImpact(senderManager, reciverManager);

        Vector3 destMove = senderManager.GetMotionForward().normalized * _Speed * _Time;

        //reciverManager.SetMove(destMove, _Time / SkillMotion.SkillBaseSpeed);
        HitMotion(senderManager, reciverManager, destMove, _Time);

        ProcessDamge(senderManager, reciverManager);
    }

}
