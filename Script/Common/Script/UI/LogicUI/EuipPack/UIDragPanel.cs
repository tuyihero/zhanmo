using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class UIDragPanel : UIBase
{

    #region static funs

    public static void ShowAsyn(UIDragableItemBase dragItem)
    {
        Hashtable hash = new Hashtable();
        hash.Add("DragItem", dragItem);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIDragPanel, UILayer.MessageUI, hash);
    }

    public static void DragItem(PointerEventData eventData)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIDragPanel>(UIConfig.UIDragPanel);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.OnDrag(eventData);
    }

    public static void SetDragOnItem(UIDragableItemBase dragOnItem)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIDragPanel>(UIConfig.UIDragPanel);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.SetDragOnItemInner(dragOnItem);
    }

    public static bool IsDragingItem()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIDragPanel>(UIConfig.UIDragPanel);
        if (instance == null)
            return false;

        if (!instance.isActiveAndEnabled)
            return false;

        return instance.IsDragingItemInner();
    }

    public static UIDragableItemBase GetDragingItem()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIDragPanel>(UIConfig.UIDragPanel);
        if (instance == null)
            return null;

        if (!instance.isActiveAndEnabled)
            return null;

        return instance._DragItem;
    }

    public static void HideAsyn()
    {
        UIManager.Instance.HideUI("LogicUI/BagPack/UIDragPanel");
    }

    #endregion

    #region 

    public UIPackItemBase _UIDragItem;
    private RectTransform _DragRectTrans;

    #endregion

    #region 

    private UIDragableItemBase _DragItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _DragItem = (UIDragableItemBase)hash["DragItem"];
        if (_DragItem == null)
            return;

        _DragRectTrans = _UIDragItem.GetComponent<RectTransform>();
        ShowDragItem();
    }

    public override void Hide()
    {
        if (_DragOnItem != null)
        {
            _DragOnItem.OnPointerExit(null);
            _DragOnItem = null;
        }
        _DragItem = null;
        base.Hide();
    }

    private void ShowDragItem()
    {
        _UIDragItem.ShowItem(_DragItem.ShowedItem);
    }

    #endregion

    private UIDragableItemBase _DragOnItem;

    public void OnDrag(PointerEventData eventData)
    {
        if (_DragItem == null)
            return;
        _DragRectTrans.anchoredPosition = eventData.position;
    }

    public void SetDragOnItemInner(UIDragableItemBase dragItem)
    {
        _DragOnItem = dragItem;
    }

    public bool IsDragingItemInner()
    {
        return _DragItem != null;
    }

}

