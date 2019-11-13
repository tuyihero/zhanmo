using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 

 

public enum AimType
{
    Free,
    Lock,
}

public class AimTargetItem : MonoBehaviour
{
    public float _AnimAlpha;
    public Color _AimFreeColor;
    public Color _AimLockColor;
    public Renderer _ImageRenderer;
    public Animator _Animator;

    private Color _AnimColor;
    private Color _LastUpdateColor;

    void Update()
    {
        //_AnimColor.a = _AnimAlpha;
        //if (_LastUpdateColor == _AnimColor)
        //{
        //    return;
        //}

        
        //_LastUpdateColor = _AnimColor;
        //_ImageRenderer.material.SetColor("_Color", _AnimColor);
    }

    public void SetAimType(AimType aimType)
    {
        //if (aimType == AimType.Free)
        //{
        //    _AnimColor = _AimFreeColor;
        //    _Animator.Play("AimFree");
        //}
        //else
        //{
        //    _AnimColor = _AimLockColor;
        //    _Animator.Play("AimLock");
        //}
    }
}

