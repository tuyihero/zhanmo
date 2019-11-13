using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectFollowPlayer : MonoBehaviour
{

    public float _Accelate;
    public float _MaxSpeed;

    private float _Speed;
    private EffectController _BindEffect;
    private float _EffectLastTime;
    private float _StartTime;

    public void OnEnable()
    {
        _BindEffect = GetComponent<EffectController>();
        _EffectLastTime = _BindEffect._EffectLastTime;
        _Speed = 0;
        _StartTime = Time.time;
    }

    public void Update()
    {
        if (FightManager.Instance.MainChatMotion == null)
        {
            RevEffect();
            return;
        }
        _Speed = _Speed + _Accelate * Time.deltaTime;
        _Speed = Mathf.Min(_Speed, _MaxSpeed);
        Vector3 pickPos = FightManager.Instance.MainChatMotion.transform.position + new Vector3(0, 1, 0);
        var direct = (pickPos - transform.position);
        if (direct.magnitude < 0.5f)
        {
            RevEffect();
        }
        else
        {
            transform.position += direct.normalized * _Speed;
            transform.LookAt(pickPos);
            //if (Vector3.Distance(transform.position, FightManager.Instance.MainChatMotion.transform.position) <= 0.5f)
            //{
            //    RevEffect();
            //}
        }

        if (_EffectLastTime > 0 && Time.time - _StartTime > _EffectLastTime)
        {
            RevEffect();
        }
    }

    public void RevEffect()
    {
        gameObject.SetActive(false);
        ResourcePool.Instance.RecvIldeEffect(_BindEffect);
    }
}
