using UnityEngine;
using System.Collections;

public class BulletEmitterXiShi : BulletEmitterBase
{
    public int _BulletCnt = 8;
    public float _RotSpeed = 60.0f;
    public float _BulletMaxTime = 10.0f;
    public Transform _EmitterTrans;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        gameObject.SetActive(true);
        EmitBullet();
    }

    private void EmitBullet()
    {
        if (_EmitterTrans == null)
        {
            var emitterGO = new GameObject("XishiBulletEmitter");
            _EmitterTrans = emitterGO.transform;
            _EmitterTrans.position = transform.position;
        }
        for (int i = 0; i < _BulletCnt; ++i)
        {
            SendBullet(i);
        }
    }

    private void SendBullet(int idx)
    {
        var bullet = InitBulletGO<BulletBase>();
        float angle = idx * (360 / _BulletCnt) - 360 * 0.5f;

        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, _SenderManager.transform.rotation.eulerAngles.y + angle, 0));
        bullet.transform.SetParent(_EmitterTrans);
    }

    private void FixedUpdate()
    {
        _EmitterTrans.rotation = Quaternion.Euler(new Vector3(0, _EmitterTrans.rotation.eulerAngles.y + Time.fixedDeltaTime * _RotSpeed, 0));
        _EmitterTrans.position = _SenderManager.transform.position;
    }
}
