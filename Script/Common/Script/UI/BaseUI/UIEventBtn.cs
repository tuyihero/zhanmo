using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.Events;

public class UIEventBtn : MonoBehaviour, IPointerClickHandler
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent<PointerEventData>
    {
        public ButtonClickedEvent() { }
    }

    [SerializeField]
    private ButtonClickedEvent _ButtonClickedEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_ButtonClickedEvent != null)
        {
            _ButtonClickedEvent.Invoke(eventData);
        }
    }
}
