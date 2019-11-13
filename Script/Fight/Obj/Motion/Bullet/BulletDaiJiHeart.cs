using UnityEngine;
using System.Collections;

public class BulletDaiJiHeart : BulletBase
{
    //public Vector3 _StartSpeed;
    public float _GravityModifier;
    public float _TrackAccelate;
    public EffectController _SubEffect;
    public int _BulletDestoryAudio;

    private MotionManager _TargetMotion;
    private Vector3 _UpSpeed;
    private Vector3 _TargetPosition;
    private Vector3 _TrackSpeed;
    private Collider _Collider;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        var target = SelectTargetCommon.GetNearMotion(senderMotion, senderMotion.transform.position, null);
        if (target != null)
        {
            _TargetMotion = target.GetComponent<MotionManager>();
        }

        _Collider = gameObject.GetComponent<Collider>();
        _Collider.enabled = false;
    }

    public void SetInitSpeed(Vector3 initSpeed)
    {
        _UpSpeed = initSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_UpSpeed.y > 0)
        {
            transform.position += _UpSpeed * Time.fixedDeltaTime;
            _UpSpeed.y -= _GravityModifier * 10 * Time.fixedDeltaTime;
            if (_UpSpeed.y <= 0)
            {
                _TargetPosition = _TargetMotion.transform.position;
                var targetDirect = _TargetPosition - transform.position;
                _TrackSpeed = targetDirect.normalized * _UpSpeed.magnitude;
            }
        }
        else
        {
            _Collider.enabled = true;
            transform.position += _TrackSpeed * Time.fixedDeltaTime;
            _TrackSpeed += _TrackAccelate * _TrackSpeed.normalized;
            if (transform.position.y < _TargetPosition.y)
            {
                if (_SubEffect != null)
                {
                    //ResourcePool.Instance.PlaySceneEffect(_SubEffect, transform.position, Vector3.zero);
                    Hashtable hash = new Hashtable();
                    hash.Add("WorldPos", transform.position);
                    var effectInstance = SkillMotion.PlayDynamicEffect(_SubEffect, hash);
                }

                BulletFinish();
            }
        }
    }

    protected override void BulletFinish()
    {
        base.BulletFinish();

        if (_BulletDestoryAudio > 0)
        {
            SkillMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_BulletDestoryAudio]);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        BulletHit(targetMotion);

        if (_SubEffect != null)
        {
            Hashtable hash = new Hashtable();
            hash.Add("WorldPos", transform.position);
            var effectInstance = SkillMotion.PlayDynamicEffect(_SubEffect, hash);
            //ResourcePool.Instance.PlaySceneEffect(_SubEffect, transform.position, Vector3.zero);
        }

        BulletFinish();
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollierEnter:" + other.ToString());
    }
}
