using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class ImpactPushToTargetBackRandom : ImpactBase
{
    public float _Distance = -1;
    public float _Angle = 20;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        int angleCnt = (int)(360 / _Angle);
        List<Vector3> farPos = new List<Vector3>();
        for (int i = 0; i < angleCnt; ++i)
        {
            var rot = new Vector3(0, reciverManager.transform.rotation.eulerAngles.y + _Angle, 0);
            reciverManager.transform.rotation = Quaternion.Euler(rot);
            Vector3 pos = reciverManager.transform.position + reciverManager.transform.forward * _Distance;

            NavMeshHit navmeshHit;
            if (NavMesh.SamplePosition(pos, out navmeshHit, 1000, 1))
            {
                var distance = Vector3.Distance(reciverManager.transform.position, navmeshHit.position);
                if (distance > _Distance * 0.5f)
                {
                    farPos.Add(pos);
                }
            }
        }

        if (farPos.Count == 0)
            return;

        int randomIdx = Random.Range(0, farPos.Count);
        //Vector3 pos = reciverManager.transform.position + reciverManager.transform.forward * _Distance;
        senderManager.SetPosition(farPos[randomIdx]);
        senderManager.SetLookAt(reciverManager.transform.position);
    }

}
