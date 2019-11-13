using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BulletMuTong : BulletSummon
{
    public AnimationClip _HitAnimation;
    public BulletEmitterBase _SubBulletEmitter;
    public float _LiveTime = 60;
    private Animation _Animation;

    public void Update()
    {
        if (_SkillMotion == null)
        {
            BulletFinish();
        }
    }

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        gameObject.layer = FightLayerCommon.EVIL;

        var collider = gameObject.GetComponent<Collider>();
        collider.enabled = true;

        _Animation = gameObject.GetComponentInChildren<Animation>();
        if (_Animation != null && _HitAnimation != null)
        {
            _Animation.AddClip(_HitAnimation, _HitAnimation.name);
        }

        Invoke("OnBulletHit", _LiveTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        if (other.gameObject.GetComponent<Rigidbody>() != null)
            return;

        OnBulletHit();
    }

    public void OnBulletHit()
    {
        Debug.Log("OnBulletHit");
        var collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;

        if (_Animation != null && _HitAnimation != null)
        {
            _Animation.Play(_HitAnimation.name);
        }

        _SubBulletEmitter.ActImpact(_SkillMotion, _SkillMotion);
        Invoke("BulletFinish", 1.5f);
    }
}
