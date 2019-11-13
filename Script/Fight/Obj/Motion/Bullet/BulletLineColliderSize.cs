using UnityEngine;
using System.Collections;

public class BulletLineColliderSize : BulletLine
{
    public float _OrgSize;
    public float _SizeSpeed;

    private CapsuleCollider _Collider;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _Collider = GetComponent<CapsuleCollider>();
        _Collider.radius = _OrgSize;
    }

    private IEnumerator FinishDelay()
    {
        yield return new WaitForSeconds(_LifeTime);

        BulletFinish();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        MoveUpdate();

        if (_Collider != null)
        {
            _Collider.radius += _SizeSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        TriggetMotion(targetMotion);
    }

}
