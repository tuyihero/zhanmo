using UnityEngine;
using System.Collections;

public class BulletEmitterRandomForward : BulletEmitterBase
{
    public float _RandomZ = 1;
    public float _RandomY = 0.5f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        EmitBullet();

    }

    private void EmitBullet()
    {
        float randomY = Random.Range(-_RandomY, _RandomY);
        float randomZ = Random.Range(-_RandomZ, _RandomZ);
        var pos = SenderMotion.transform.up * randomY + SenderMotion.transform.right * randomZ;

        var bullet = InitBulletGO<BulletBase>();
        bullet.transform.position += pos;
    }
}
