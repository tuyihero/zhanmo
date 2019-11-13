using UnityEngine;
using System.Collections;

public class BulletEmitterRandomLast : BulletEmitterRandomSplit
{
    public int _RoundCount = 1;
    public float _RoundInterval = 0.1f;

    private int _RountIdx;
    private Vector3 _StartPosition = Vector3.zero;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        gameObject.SetActive(true);
        base.ActImpact(senderManager, reciverManager);

        _RountIdx = 0;
        StartCoroutine(EmitBullet());
    }

    private IEnumerator EmitBullet()
    {
        ++_RountIdx;
        if (_RountIdx >= _RoundCount)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(_RoundInterval);
            StartCoroutine(EmitBullet());
        }

        var bulletPoses = GameRandom.GetIndependentRandoms(0, _SplitCnt * _SplitCnt * 4, _BulletCnt);
        for (int i = 0; i < bulletPoses.Count; ++i)
        {
            EmitBullet(bulletPoses[i]);
        }

        
    }
}
