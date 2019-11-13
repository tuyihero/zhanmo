using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterBase : ImpactBase
{
    public BulletBase _BulletPrefab;
    public bool _SenderPos = true;
    public Vector3 _EmitterOffset;
    public float _Damage;
    public ElementType _DamageType = ElementType.Physic;
    public Vector3 _BulletScaleModify = Vector3.one;

    protected MotionManager _SenderManager;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);
        _SenderManager = senderManager;

        HitMotions.Clear();
    }

    protected T InitBulletGO<T>() where T:class
    {
        Vector3 modifyPos = transform.forward * _EmitterOffset.x + transform.right * _EmitterOffset.z + transform.up * _EmitterOffset.y;
        var bulletObj = ResourcePool.Instance.GetIdleBullet(_BulletPrefab);
        bulletObj.gameObject.SetActive(true);
        bulletObj.transform.SetParent(ResourcePool.Instance.transform);
        if (_SenderPos)
        {
            bulletObj.transform.position = SenderMotion.transform.position + modifyPos;
        }
        else
        {
            bulletObj.transform.position = transform.position + modifyPos;
        }
        bulletObj.transform.rotation = transform.rotation;
        bulletObj.transform.localScale = _BulletScaleModify;
        bulletObj.gameObject.layer = FightLayerCommon.GetBulletLayer(_SenderManager);
        bulletObj.Init(_SenderManager, this);
        var bulletHits= bulletObj.GetComponentsInChildren<ImpactDamage>();
        foreach (var impactDamage in bulletHits)
        {
            impactDamage._DamageRate = _Damage;
            impactDamage._DamageType = _DamageType;

            if (impactDamage is ImpactHit)
            {
                ImpactHit impactHit = impactDamage as ImpactHit;
                impactHit._IsBulletHit = true;
            }
        }
        return bulletObj as T;
    }

    protected List<T> InitBulletGO<T>(int bulletCnt) where T :class
    {
        List<T> bullets = new List<T>();
        for (int i = 0; i < bulletCnt; ++i)
        {
            var bullet = InitBulletGO<T>();
            bullets.Add(bullet);
        }

        return bullets;
    }

    #region bullet hit motions

    private Dictionary<MotionManager, int> _HitMotions = new Dictionary<MotionManager, int>();
    public Dictionary<MotionManager, int> HitMotions
    {
        get
        {
            return _HitMotions;
        }
    }

    public void AddHitTimes(MotionManager targetMotion)
    {
        if (!_HitMotions.ContainsKey(targetMotion))
        {
            _HitMotions.Add(targetMotion, 0);
        }

        ++_HitMotions[targetMotion];
    }

    public int GetMotionHitimes(MotionManager targetMotion)
    {
        if (!_HitMotions.ContainsKey(targetMotion))
        {
            return 0;
        }

        return _HitMotions[targetMotion];
    }
    #endregion
}
