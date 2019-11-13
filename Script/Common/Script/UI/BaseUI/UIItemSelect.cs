using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

using System;


public class UIItemSelect : UIItemBase, IPointerClickHandler
{
    public GameObject _SelectGO;

    #region select

    public virtual bool IsCanSelect()
    {
        return true;
    }

    public virtual void Selected()
    {
        if (_SelectGO != null)
        {
            _SelectGO.SetActive(true);
        }
    }

    public virtual void UnSelected()
    {
        if (_SelectGO != null)
        {
            _SelectGO.SetActive(false);
        }
    }

    #endregion
}

