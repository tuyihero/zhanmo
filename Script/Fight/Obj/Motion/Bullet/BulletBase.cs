using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletBase : MonoBehaviour
{
    public bool _IsBulletHitLie;
    public int _BornAudio = -1;
    public int _HitAudio = -1;
    public int _NoHitAudio = -1;
    protected ImpactBase[] _ImpactList;
    protected MotionManager _SkillMotion;
    public MotionManager SkillMotion
    {
        get
        {
            return _SkillMotion;
        }
    }

    protected BulletEmitterBase _EmitterBase;


    public virtual void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        _SkillMotion = senderMotion;
        _ImpactList = gameObject.GetComponents<ImpactBase>();
        _EmitterBase = emitterBase;
    }

    protected virtual void BulletHit(MotionManager hitMotion)
    {
        foreach (var impact in _ImpactList)
        {
            impact.ActImpact(_SkillMotion, hitMotion);
        }
    }

    protected virtual void BulletFinish()
    {
        Debug.Log("BulletFinish");
        ResourcePool.Instance.RecvIldeBullet(this);
    }

    #region autio

    private AudioSource _AudioSource;
    public AudioSource AudioSource
    {
        get
        {
            if (_AudioSource == null)
            {
                _AudioSource = gameObject.GetComponent<AudioSource>();
                if (_AudioSource == null)
                {
                    _AudioSource = gameObject.AddComponent<AudioSource>();
                }
            }
            return _AudioSource;
        }
    }

    #endregion

    #region hit autio

    public bool _IsPlayedHitAudio = false;

    public void PlayHitAudio()
    {
        if (_IsPlayedHitAudio)
            return;

        if (_HitAudio < 0)
            return;

        AudioSource.PlayOneShot(ResourcePool.Instance._CommonAudio[_HitAudio]);
        _IsPlayedHitAudio = true;
    }

    public void PlayNoHitAudio()
    {
        if (_IsPlayedHitAudio)
            return;

        if (_NoHitAudio < 0)
            return;

        AudioSource.PlayOneShot(ResourcePool.Instance._CommonAudio[_NoHitAudio]);
        _IsPlayedHitAudio = true;
    }

    public void ClearHitFlag()
    {
        _IsPlayedHitAudio = false;
    }

    #endregion

    #region 2d

    private Transform _SpriteGO;

    protected void RefreshSpriteRot()
    {
        if (_SpriteGO == null)
        {
            _SpriteGO = transform.Find("Sprite");
        }

        if (_SpriteGO != null)
        {
            if (transform.rotation.eulerAngles.y == 0)
            {
                _SpriteGO.localRotation = Quaternion.Euler(45, 0, 0);
            }
            else
            {
                _SpriteGO.localRotation = Quaternion.Euler(-45, 0, 0);
            }
        }
    }

    #endregion
}
