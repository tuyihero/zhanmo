using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 



public class UIDamagePanel : UIBase
{
    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("IndependCanvas", true);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIDamagePanel, UILayer.BaseUI, hash);
    }

    public static void ShowItem(Vector3 showWorldPos, int showValue1, int showValue2, RoleAttrManager.ShowDamageType showType, int baseSize)
    {
        if (!GameCore.Instance)
            return;

        var instance = GameCore.Instance.UIManager.GetUIInstance<UIDamagePanel>(UIConfig.UIDamagePanel);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowItemInner(showWorldPos, showValue1, showValue2, showType, baseSize);
    }

    public static void HideItem(UIDamageItem item)
    {
        ResourcePool.Instance.RecvIldeUIItem(item.gameObject);
    }

    #endregion

    public UIDamageItem _UIHPItemPrefab;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

    }

    public override void Hide()
    {
        base.Hide();

    }

    private void ShowItemInner(Vector3 showWorldPos, int showValue1, int showValue2, RoleAttrManager.ShowDamageType showType, int baseSize)
    {
        if (showType == RoleAttrManager.ShowDamageType.Criticle ||
            showType == RoleAttrManager.ShowDamageType.Heal ||
            showType == RoleAttrManager.ShowDamageType.Hurt)
        {
            var itemBase = ResourcePool.Instance.GetIdleUIItem<UIDamageItem>(_UIHPItemPrefab.gameObject);
            itemBase.transform.SetParent(transform);
            itemBase.Show(showWorldPos, showValue1, showValue2, showType, baseSize);
        }
    }

    private void HideItem(object sender, Hashtable args)
    {
        UIHPItem hideItem = args["HideItem"] as UIHPItem;
        ResourcePool.Instance.RecvIldeUIItem(hideItem.gameObject);
    }


}

