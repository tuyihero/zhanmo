using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

using System;


public class UISubMenuItem : UIItemBase, IPointerClickHandler
{
    public GameObject _SelectGO;
    public GameObject _SunOpenGO;
    public Text _MenuText;

    protected object _MenuObj;
    public object MenuObj
    {
        get
        {
            return _MenuObj;
        }
    }

    protected int _SubLevel;
    public int SubLevel
    {
        get
        {
            return _SubLevel;
        }
        set
        {
            _SubLevel = value;
        }
    }

    public virtual void InitMenu(object obj)
    {
        _MenuObj = obj;
        string str = obj as string;
        _MenuText.text = str;
        if(_SelectGO != null)
            _SelectGO.SetActive(false);
    }

    #region select
    public virtual bool ShowMenu()
    {
        //if (_SelectGO != null)
        {
            gameObject.SetActive(true);
            return true;
        }
    }

    public virtual void Selected()
    {
        if (_SelectGO != null)
        {
            _SelectGO.SetActive(true);
        }
    }

    public virtual bool IsSelected()
    {
        return _SelectGO.activeInHierarchy;
    }

    public virtual void UnSelected()
    {
        if (_SelectGO != null)
        {
            _SelectGO.SetActive(false);
        }
    }

    #endregion

    #region subGO

    public void OpenSubGO()
    {
        if (_SunOpenGO != null)
        {
            _SunOpenGO.SetActive(true);
        }
    }

    public void CloseSubGO()
    {
        if (_SunOpenGO != null)
        {
            _SunOpenGO.SetActive(false);
        }
    }

    #endregion
}

