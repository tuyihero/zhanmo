
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;


public class UIDragableItemBase : UIPackItemBase, IBeginDragHandler, IEndDragHandler, IDragHandler,IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool _IsCanDrag = true;
    public GameObject _DropEnable;
    public GameObject _DropDisable;

    public UIBase _DragPackBase;
    private IDragablePack _DragablePack;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (hash.ContainsKey("DragPack"))
        {
            _DragPackBase = (UIBase)hash["DragPack"];
            _DragablePack = (IDragablePack)hash["DragPack"];
        }
    }

    public override void ShowItem(ItemBase showItem)
    {
        base.ShowItem(showItem);

        SetGOActive(_DropEnable, false);
        SetGOActive(_DropDisable, false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_IsCanDrag)
            return;

        if (!IsCanDrag())
            return;

        UIDragPanel.ShowAsyn(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UIDragPanel.HideAsyn();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIDragPanel.DragItem(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragItem = UIDragPanel.GetDragingItem();
        if (dragItem == null)
            return;

        if (dragItem == this)
            return;

        if (IsCanDrop())
        {
            OnDrop(dragItem);
        }
        UIDragPanel.HideAsyn();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!UIDragPanel.IsDragingItem())
            return;

        UIDragPanel.SetDragOnItem(this);
        var dragItem = UIDragPanel.GetDragingItem();
        if (dragItem == null)
            return;

        if (dragItem == this)
            return;

        if (IsCanDrop())
        {
            SetGOActive(_DropEnable, true);
            SetGOActive(_DropDisable, false);
        }
        else
        {
            SetGOActive(_DropEnable, false);
            SetGOActive(_DropDisable, true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!UIDragPanel.IsDragingItem())
            return;

        var dragItem = UIDragPanel.GetDragingItem();
        if (dragItem == null)
            return;

        if (dragItem == this)
            return;

        SetGOActive(_DropEnable, false);
        SetGOActive(_DropDisable, false);
    }

    #region func

    protected virtual bool IsCanDrag()
    {
        if (_IsCanDrag)
        {
            return ShowedItem.IsVolid();
        }
        return false;
    }

    protected virtual bool IsCanDrop()
    {
        var dragItem = UIDragPanel.GetDragingItem();
        if (_DragablePack == null)
            return false;

        if (dragItem == null)
            return false;

        return _DragablePack.IsCanDropItem(dragItem, this);
    }

    protected virtual void OnDrop(UIDragableItemBase dragItem)
    {
        if (_DragablePack == null)
            return;

        if (dragItem == null)
            return;

        _DragablePack.OnDragItem(dragItem, this);
    }

    #endregion
}

