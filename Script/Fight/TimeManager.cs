using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region 

    private static TimeManager _Instance;
    public static TimeManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Awake()
    {
        _Instance = this;
    }

    #endregion

    private float _FightTime = 0;
    public float FightTime
    {
        get
        {
            return _FightTime;
        }
    }

    private bool _IsPause = false;

    public void Init()
    {
        _FightTime = 0;
    }

    public void Pause()
    {
        _IsPause = true;
    }

    public void Resume()
    {
        _IsPause = false;
    }

    public void FixedUpdate()
    {
        if (_IsPause)
            return;

        _FightTime += Time.fixedDeltaTime;
    }
}
