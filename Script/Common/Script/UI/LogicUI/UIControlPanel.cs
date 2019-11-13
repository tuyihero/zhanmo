using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;

 
 
using System;



public class UIControlPanel : UIBase, IPointerClickHandler
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIControlPanel, UILayer.ControlUI, hash);
    }

    public static void AddClickEvent(OnPointClick pointEvent)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIControlPanel>(UIConfig.UIControlPanel);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.AddPointEvent(pointEvent);
    }

    public static void RemoveClickEvent(OnPointClick pointEvent)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIControlPanel>(UIConfig.UIControlPanel);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RemovePointEvent(pointEvent);
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
    }

    public void OnEnable()
    {

    }

    #endregion

    #region event

    public delegate void OnPointClick(PointerEventData eventData);
    private List<OnPointClick> _PointEvents = new List<OnPointClick>();

    public void AddPointEvent(OnPointClick pointEvent)
    {
        if (!_PointEvents.Contains(pointEvent))
        {
            _PointEvents.Add(pointEvent);
        }
    }

    public void RemovePointEvent(OnPointClick pointEvent)
    {
        if (_PointEvents.Contains(pointEvent))
        {
            _PointEvents.Remove(pointEvent);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < _PointEvents.Count; ++i)
        {
            _PointEvents[i].Invoke(eventData);
        }
    }

    #endregion
}

