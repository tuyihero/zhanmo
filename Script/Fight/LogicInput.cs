using UnityEngine;
using System.Collections;
using System;

public class LogicInput : MonoBehaviour,IFightUpdate
{
    #region instance
    
    private LogicInput _Instance;
    public LogicInput Instance
    {
        get
        {
            return _Instance;
        }
        set
        {
            _Instance = value;
        }
    }

    public void Start()
    {
        Instance = this;
    }

    #endregion

    #region 

    public void LogicUpdate()
    {
        
    }

    #endregion

    #region input mouse
    private bool _IsMouseDown = false;
    private Vector3 _MouseDownPos;

    public void MouseDown(Vector3 mousePoint)
    {
        _IsMouseDown = true;
        _MouseDownPos = mousePoint;
    }

    public void MouseUp(Vector3 mousePoint)
    {
        _IsMouseDown = false;
        _MouseDownPos = mousePoint;
    }

    private void MouseUpdate()
    {

    }
    #endregion

    #region history

    private class InputHistory
    {
        int updateCount;
        int inputType;
        Vector3 inputValue;
    }


    #endregion

}
