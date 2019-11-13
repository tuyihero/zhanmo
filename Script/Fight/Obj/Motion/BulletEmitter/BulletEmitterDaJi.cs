using UnityEngine;
using System.Collections;

public class BulletEmitterDaJi : BulletEmitterBase
{
    public int _BulletCnt = 4;
    public Vector2 _StartSpeed = new Vector2(10,18);

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var bullets = InitBulletGO<BulletDaiJiHeart>(_BulletCnt);

        for (int i = 0; i < _BulletCnt; ++i)
        {
            float angle = i * (360.0f / _BulletCnt) / Mathf.Rad2Deg;
            Vector3 direct = new Vector3(Mathf.Sin(angle), 1, Mathf.Cos(angle));
            float speed = Random.Range(_StartSpeed.x, _StartSpeed.y);
            bullets[i].SetInitSpeed(direct * speed);
        }
    }
}
