using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



/// <summary>
/// 事件管理类
/// </summary>
public class EventController : MonoBehaviour
{
    /// <summary>
    /// 事件发送参数
    /// </summary>
    private class EventParam
    {
        /// <summary>
        /// 事件
        /// </summary>
        public EVENT_TYPE EventType;
        /// <summary>
        /// 事件发送者
        /// </summary>
        public object Sender;
        /// <summary>
        /// 事件参数
        /// </summary>
        public Hashtable EventArgs;

        public EventParam(EVENT_TYPE eventType, object sender, Hashtable eventArgs)
        {
            EventType = eventType;
            Sender = sender;
            EventArgs = eventArgs;
        }
    }

    private class HandlerInfo
    {
        public EventDelegate _EventDelegate;
        public int _Priority;
    }

    void Update()
    {
        if (_EventList.Count > 0)
        {
            DispatchEvent();
        }
    }

    #region 一般事件

    public delegate void EventDelegate(object go, Hashtable eventArgs);
    /// <summary>
    /// 事件处理者列表
    /// </summary>        
    private Dictionary<EVENT_TYPE, List<HandlerInfo>> _HandleList = new Dictionary<EVENT_TYPE, List<HandlerInfo>>();

    /// <summary>
    /// 事件列表
    /// </summary>
    private List<EventParam> _EventList = new List<EventParam>();

    private bool _IsSendedEvent;

    public void RegisteEvent(EVENT_TYPE eventType, EventDelegate handler, int delegatePrior = 0)
    {
        HandlerInfo handleInfo = new HandlerInfo();
        handleInfo._EventDelegate = handler;
        handleInfo._Priority = delegatePrior;

        if (_HandleList.ContainsKey(eventType))
        {

            _HandleList[eventType].Add(handleInfo);
            _HandleList[eventType].Sort((handler1, handler2) =>
            {
                if (handler1._Priority > handler2._Priority)
                    return -1;
                else if (handler1._Priority < handler2._Priority)
                    return 1;
                return 0;
            });
        }
        else
        {
            List<HandlerInfo> newHandle = new List<HandlerInfo>();
            newHandle.Add(handleInfo);
            _HandleList.Add(eventType, newHandle);
        }
    }

    public void UnRegisteEvent(EVENT_TYPE eventType, EventDelegate handler)
    {
        if (_HandleList.ContainsKey(eventType))
        {
            HandlerInfo handleInfo = null;
            foreach (var handle in _HandleList[eventType])
            {
                if (handler == handle._EventDelegate)
                {
                    handleInfo = handle;
                }
            }
            _HandleList[eventType].Remove(handleInfo);
            if (_HandleList[eventType].Count == 0)
            {
                _HandleList.Remove(eventType);
            }
        }
    }

    public void PushEvent(EVENT_TYPE EventType, object sender, Hashtable eventArgs)
    {
        Hashtable hash = eventArgs;
        if (hash == null)
            hash = new Hashtable();

        _EventList.Add(new EventParam(EventType, sender, hash));
        //if (!_IsSendedEvent)
        //{
        //    Invoke("DispatchEvent", 0);
        //    _IsSendedEvent = true;
        //}
    }

    public void DispatchEvent()
    {
        for (int i = 0; i < _EventList.Count; ++i)
        {
            if (_HandleList.ContainsKey(_EventList[i].EventType))
            {
                _EventList[i].EventArgs.Add("EVENT_TYPE", _EventList[i].EventType);
                List<HandlerInfo> invalidHandle = new List<HandlerInfo>();
                for (int j = 0; j < _HandleList[_EventList[i].EventType].Count; ++j)
                {
                    EventDelegate handler = _HandleList[_EventList[i].EventType][j]._EventDelegate;
                    if (handler == null)
                    {
                        invalidHandle.Add(_HandleList[_EventList[i].EventType][j]);
                        continue;
                    }
                    try
                    {
                        handler.Invoke(_EventList[i].Sender, _EventList[i].EventArgs);
                        if (_EventList[i].EventArgs.ContainsKey("StopEvent"))
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        invalidHandle.Add(_HandleList[_EventList[i].EventType][j]);
                        Debug.LogError("EventController DispatchEvent Exception EventType:" + _EventList[i].EventType +
                            " handleName:" + handler.ToString() + " e:" + ex);
                    }
                }

                for (int j = 0; j < invalidHandle.Count; ++j)
                {
                    _HandleList[_EventList[i].EventType].Remove(invalidHandle[j]);
                }
            }
        }
        _EventList.Clear();
        _IsSendedEvent = false;
    }
    #endregion
}

