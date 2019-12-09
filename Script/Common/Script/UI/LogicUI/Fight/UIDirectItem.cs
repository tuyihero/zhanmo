using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;

public class UIDirectItem : UIItemBase, IPointerEnterHandler, IPointerExitHandler
{
    public InputDirect _InputDirect;
    private bool _IsPress = false;
    public bool IsPress
    {
        get
        {
            return _IsPress;
        }
    }

    public void OnItemPress(bool isPress)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _IsPress = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _IsPress = false;
    }
}

