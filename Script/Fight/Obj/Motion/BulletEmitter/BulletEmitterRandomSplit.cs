using UnityEngine;
using System.Collections;

public class BulletEmitterRandomSplit : BulletEmitterBase
{
    public int _BulletCnt = 5;
    public int _MaxRange = 8;
    public float _SplitRange = 0.5f;

    protected int _SplitCnt;
    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        _SplitCnt = (int)(_MaxRange / _SplitRange);
        var bulletPoses = GameRandom.GetIndependentRandoms(0, _SplitCnt * _SplitCnt * 4, _BulletCnt);
        for (int i = 0; i < bulletPoses.Count; ++i)
        {
            EmitBullet(bulletPoses[i]);
        }
    }

    protected void EmitBullet(int bulletPos)
    {
        var bullet = InitBulletGO<BulletBase>();

        var randomX = bulletPos / (_SplitCnt * 2) - _SplitCnt;
        var randomY = bulletPos % (_SplitCnt * 2) - _SplitCnt;

        Vector3 destPos = new Vector3(randomX * _SplitRange, 0, randomY * _SplitRange);
        bullet.transform.position = _SenderManager.transform.position + destPos;
    }
}
