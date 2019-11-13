using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;



public class UIHoldBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    public class HoldEvent : UnityEvent
    {
        public HoldEvent()
        {

        }
    }

    [SerializeField]
    private HoldEvent _BtnHold;

    [Serializable]
    public class ReleaseEvent : UnityEvent
    {
        public ReleaseEvent()
        {

        }
    }

    [SerializeField]
    private ReleaseEvent _BtnRelease;

    public float _HoldTime = 0.5f;
    private float _HoldDuring = 0;
    private bool _Holding = false;

    public void Update()
    {
        if (_HoldDuring > 0)
        {
            _HoldDuring -= Time.deltaTime;
            if (_HoldDuring <= 0)
            {
                _BtnHold.Invoke();
                _Holding = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _HoldDuring = _HoldTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_Holding)
        {
            _Holding = false;
            _BtnRelease.Invoke();
        }
    }
}

