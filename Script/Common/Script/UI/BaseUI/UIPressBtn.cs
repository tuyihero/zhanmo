using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class UIPressBtn : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
    #region 

    public float _PressStart = 0;
    public float _PressInterval = 0;
    
    [Serializable]
    public class PressAction : UnityEvent<bool>
    {
        public PressAction() { }
    }

    [SerializeField]
    public PressAction _PressAction;

    private bool _IsPress = false;
    public bool IsPress
    {
        get
        {
            return _IsPress;
        }
    }

    #endregion

    #region  IPointerDownHandler

    public void OnPointerDown(PointerEventData eventData)
    {
        PressInvoke();

        if (_PressStart > 0)
        {
            StartCoroutine(OnPressStart());
        }
        else if (_PressInterval > 0)
        {
            StartCoroutine(OnPressInvoke());
        }
        _IsPress = true;
    }

    private IEnumerator OnPressStart()
    {
        yield return new WaitForSeconds(_PressStart);
        if (!_IsPress)
            yield break;

        if (_PressInterval > 0)
        {
            StartCoroutine(OnPressInvoke());
        }
        else
        {
            PressInvoke();
        }
    }

    private IEnumerator OnPressInvoke()
    {
        while (_IsPress)
        {
            yield return new WaitForSeconds(_PressInterval);
            if (_PressAction != null)
            {
                _PressAction.Invoke(true);
            }
        }
    }

    private void PressInvoke()
    {
        if (_PressAction != null)
        {
            _PressAction.Invoke(true);
        }
    }

    #endregion

    #region IPointerUpHandler

    public void OnPointerUp(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        if (_PressAction != null)
        {
            _PressAction.Invoke(false);
        }
        _IsPress = false;
    }

    #endregion
}

