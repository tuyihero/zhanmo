using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 



public class DamagePanel : InstanceBase<DamagePanel>
{
    #region 

    public void Awake()
    {
        SetInstance(this);
    }

    void OnDestory()
    {
        SetInstance(null);
    }

    #endregion

    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.DamagePanel, UILayer.PopUI, hash);
    }

    public static void ShowItem(Vector3 showWorldPos, int showValue1, int showValue2, RoleAttrManager.ShowDamageType showType, int baseSize)
    {

        var instance = DamagePanel.Instance;
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowItemInner(showWorldPos, showValue1, showValue2, showType, baseSize);
    }

    public static void HideItem(DamageItem item)
    {
        ResourcePool.Instance.RecvIldeUIItem(item.gameObject);
    }

    #endregion

    public DamageItem _UIItemPrefab;

    private void ShowItemInner(Vector3 showWorldPos, int showValue1, int showValue2, RoleAttrManager.ShowDamageType showType, int baseSize)
    {
        var itemBase = ResourcePool.Instance.GetIdleUIItem<DamageItem>(_UIItemPrefab.gameObject);
        itemBase.transform.SetParent(transform);
        itemBase.Show(showWorldPos, showValue1, showValue2, showType, baseSize);
    }

}

