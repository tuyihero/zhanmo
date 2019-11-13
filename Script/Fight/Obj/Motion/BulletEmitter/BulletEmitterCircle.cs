using UnityEngine;
using System.Collections;

public class BulletEmitterCircle : BulletEmitterElement
{
    public Vector3 _BulletPosOffset;
    public int _BulletCnt = 8;
    public float _Interval = 0.1f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        gameObject.SetActive(true);

        StartCoroutine(EmitBullet());
    }

    private IEnumerator EmitBullet()
    {
        for (int i = 0; i < _BulletCnt; ++i)
        {
            SendBullet(i);
            if (_Interval > 0)
            {
                yield return new WaitForSeconds(_Interval);
            }
        }
    }

    private void SendBullet(int idx)
    {
        var bullet = InitBulletGO<BulletBase>();
        float angle = idx * (360.0f / _BulletCnt);

        bullet.transform.rotation = Quaternion.Euler(0, angle, 0);
        bullet.transform.position += bullet.transform.forward * _BulletPosOffset.x + bullet.transform.up * _BulletPosOffset.y + bullet.transform.right * _BulletPosOffset.z;
    }
}
