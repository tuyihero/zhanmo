
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;

public enum InputDirect
{
    Up,
    Down,
    Left,
    Right
}


public class UIDirectControl : UIBase
{

    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIDirectControl, UILayer.BaseUI, hash);
    }

    #endregion

    public UIDirectItem _BtnUp;
    public UIDirectItem _BtnDown;
    public UIDirectItem _BtnLeft;
    public UIDirectItem _BtnRight;

    public void Update()
    {
        DirectUpdate();
    }

    public void DirectUpdate()
    {
        var inputAxis = Vector2.zero;
        if (_BtnRight.IsPress)
        {
            inputAxis = new Vector2(1, 0);
        }
        else if (_BtnLeft.IsPress)
        {
            inputAxis = new Vector2(-1, 0);
        }
        else if (_BtnUp.IsPress)
        {
            inputAxis = new Vector2(0, 1);
        }
        else if (_BtnDown.IsPress)
        {
            inputAxis = new Vector2(0, -1);
        }

#if !UNITY_EDITOR
        InputManager.Instance.Axis = inputAxis;
#endif
    }

    
}

