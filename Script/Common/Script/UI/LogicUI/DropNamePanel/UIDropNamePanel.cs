using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDropNamePanel : UIInstanceBase<UIDropNamePanel>
{
    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIDropNamePanel, UILayer.BaseUI, hash);
    }

    public static void ShowDropItem(DropItem dropItem)
    {
        if (Instance == null)
            return;

        Hashtable hash = new Hashtable();
        hash.Add("InitObj", dropItem);
        Instance.ShowItem(hash);
    }

    public static void HideDropItem(UIDropNameItem hpItem)
    {
        if (Instance == null)
            return;

        Hashtable hash = new Hashtable();
        hash.Add("HideItem", hpItem);
        Instance.HideItem(hash);
        
    }

    #endregion

    public UIDropNameItem _UIDropItemPrefab;

    public List<UIDropNameItem> _DropItems = new List<UIDropNameItem>();

    private void ShowItem(Hashtable args)
    {
        var itemBase = ResourcePool.Instance.GetIdleUIItem<UIDropNameItem>(_UIDropItemPrefab.gameObject);
        itemBase.Show(args);
        itemBase.transform.SetParent(transform);
        itemBase.transform.localScale = Vector3.one;

        _DropItems.Add(itemBase);
    }

    private void HideItem(Hashtable args)
    {
        UIDropNameItem hideItem = args["HideItem"] as UIDropNameItem;
        _DropItems.Remove(hideItem);
        ResourcePool.Instance.RecvIldeUIItem(hideItem.gameObject);
    }


}

