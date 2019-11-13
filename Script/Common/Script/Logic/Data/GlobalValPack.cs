using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GlobalValPack : DataPackBase
{
    #region 单例

    private static GlobalValPack _Instance;
    public static GlobalValPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GlobalValPack();
            }
            return _Instance;
        }
    }

    private GlobalValPack()
    {
        _SaveFileName = "GlobalValPack";
    }

    #endregion

    #region sys setting

    [SaveField(1)]
    private bool _IsShowShadow = true;
    public bool IsShowShadow
    {
        get
        {
            return _IsShowShadow;
        }
        set
        {
            if (_IsShowShadow != value)
            {
                _IsShowShadow = value;
                GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_SYSTEMSETTING_CHANGE, this, null);
                SaveClass(false);
            }
        }
    }

    [SaveField(2)]
    private float _Volume = 1;
    public float Volume
    {
        get
        {
            return _Volume;
        }
        set
        {
            if (_Volume != value)
            {
                _Volume = value;
                GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_SYSTEMSETTING_CHANGE, this, null);
                SaveClass(false);
            }
        }
    }

    [SaveField(3)]
    private bool _IsRotToAnimTarget = false;
    public bool IsRotToAnimTarget
    {
        get
        {
            return _IsRotToAnimTarget;
        }
        set
        {
            if (_IsRotToAnimTarget != value)
            {
                _IsRotToAnimTarget = value;
                GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_SYSTEMSETTING_CHANGE, this, null);
                SaveClass(false);
            }
        }
    }


    #endregion

}

