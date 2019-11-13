using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public int _TriggerTimes = 1;

    [Serializable]
    public class EventCallBack : UnityEvent<object>
    {
        public EventCallBack() { }
    }

    [SerializeField]
    public EventCallBack _EventCallBack;

    private int _TrigTimes = 0;
    void OnTriggerEnter(Collider other)
    {
        if (_EventCallBack != null)
        {
            _EventCallBack.Invoke(null);
        }

        ++_TrigTimes;
        if (_TrigTimes == _TriggerTimes)
        {
            gameObject.SetActive(false);
        }
    }

}
