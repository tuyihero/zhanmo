using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAnimController : MonoBehaviour
{
    public Animator _Body;
    public Animator _Effect;
    public Animator _Weapon;
    public Animator _WeaponEffect;

    public void Play(string animName)
    {
        if (_Body != null)
        {
            _Body.Play(animName);
        }
        if (_Effect != null)
        {
            _Effect.Play(animName);
        }
        if (_Weapon != null)
        {
            _Weapon.Play(animName);
        }
        if (_WeaponEffect != null)
        {
            _WeaponEffect.Play(animName);
        }
    }

    #region test

    public void Update()
    {
        if (Input.GetKey("1"))
        {
            Play("skill1");
        }
        else if (Input.GetKey("2"))
        {
            Play("skill2");
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Play("run");
        }
        
    }

    #endregion
}
