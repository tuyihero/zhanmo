using UnityEngine;
using System.Collections;

public class ImpactBuffFireBurn : ImpactBuff
{

    public float _Damage = 0.2f;
    public float _BurnRange = 1.5f;
    public float _Interval = 1;

    private CapsuleCollider _Collider;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _Collider = gameObject.AddComponent<CapsuleCollider>();
        _Collider.direction = 1;
        _Collider.radius = _BurnRange;
        _Collider.height = 2;
        _Collider.center = new Vector3(0, 1, 0);
        _Collider.enabled = false;
        _Collider.isTrigger = true;
        gameObject.layer = FightLayerCommon.GetBulletLayer(senderManager);

        StartCoroutine(FireBurn());
    }

    public override bool IsCanAddBuff(ImpactBuff newBuff)
    {
        if (!(newBuff is ImpactBuffFireBurn))
            return true;

        ImpactBuffFireBurn burnImpact = newBuff as ImpactBuffFireBurn;
        _Damage = burnImpact._Damage;
        _BurnRange = burnImpact._BurnRange;
        _Interval = burnImpact._Interval;

        RefreshLastTime();
        return false;
    }

    private IEnumerator FireBurn()
    {
        yield return new WaitForSeconds(_Interval);
        Debug.Log("FireBurn");
        _Collider.enabled = true;
        yield return new WaitForFixedUpdate();
        _Collider.enabled = false;
        
        StartCoroutine(FireBurn());
    }

    void OnTriggerEnter(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion.IsMotionDie)
            return;

        if (_BuffSender == null)
            return;

        _BuffSender.RoleAttrManager.SendDamageEvent(targetMotion, _Damage, ElementType.Fire, this);
        targetMotion.PlayHitEffect(_BuffSender, 3);
        targetMotion.PlayAudio(ResourcePool.Instance._CommonAudio[110]);
        //Debug.Log("OnTriggerStay:" + targetMotion.name);
    }

}
