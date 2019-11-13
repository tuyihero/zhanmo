using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletEmitterArc : BulletEmitterElement
{
    public int _BulletCnt = 8;
    public float _Angle = 60;
    public float _Interval = 0.1f;
    public bool _BulletHitMotionOnce = false;
    private List<MotionManager> _BulletHittedMotion;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        gameObject.SetActive(true);

        if (_BulletHitMotionOnce)
        {
            _BulletHittedMotion = new List<MotionManager>();
        }

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
        float angle = idx * (_Angle / _BulletCnt) - _Angle * 0.5f;

        bullet.transform.rotation = Quaternion.Euler( new Vector3(0, _SenderManager.transform.rotation.eulerAngles.y + angle, 0));

        var lineBullet = bullet as BulletLine;
        if (lineBullet != null)
        {
            if (_BulletHitMotionOnce)
            {
                lineBullet._HittedMotions = _BulletHittedMotion;
            }
            else
            {
                lineBullet._HittedMotions = null;
            }
        }
    }
}
