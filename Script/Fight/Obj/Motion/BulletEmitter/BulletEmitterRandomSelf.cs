using UnityEngine;
using System.Collections;

public class BulletEmitterRandomSelf : BulletEmitterBase
{
    public int _BulletCnt = 5;
    public int _MaxRange = 8;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        for (int i = 0; i < _BulletCnt; ++i)
        {
            EmitBullet();
        }
    }

    private void EmitBullet()
    {
        var bullet = InitBulletGO<BulletBase>();
        float angle = Random.Range(0,360);
        angle = angle / Mathf.Rad2Deg;
        Vector3 direct = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        float range = Random.Range(0, _MaxRange);
        //if (i > _BulletCnt * 0.5f)
        //{
        //    sinAngle = -sinAngle;
        //}

        bullet.transform.position += direct * range;
        Debug.Log("bullet position:" + bullet.transform.position);
    }
}
