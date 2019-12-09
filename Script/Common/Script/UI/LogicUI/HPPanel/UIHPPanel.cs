using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHPPanel : UIInstanceBase<UIHPPanel>
{
    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIHPPanel, UILayer.BaseUI, hash);
    }

    static List<Hashtable> _ShowHpMotions = new List<Hashtable>();
    public static void ShowHPItem(MotionManager motionManager, bool shoHPValue = true)
    {
        Hashtable hash = new Hashtable();
        hash.Add("InitObj", motionManager);
        hash.Add("ShowHP", shoHPValue);
        if (Instance != null)
        {
            
            Instance.ShowItem(hash);
        }
        else
        {
            _ShowHpMotions.Add(hash);
        }
    }

    public static void HideHPItem(UIHPItem hpItem)
    {
        Hashtable hash = new Hashtable();
        hash.Add("HideItem", hpItem);
        Instance.HideItem(hash);
        
    }

    #endregion

    public UIHPItem _UIHPItemPrefab;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (_ShowHpMotions != null)
        {
            foreach (var showMotion in _ShowHpMotions)
            {
                ShowItem(showMotion);
            }
            _ShowHpMotions.Clear();
        }
    }

    private void ShowItem(Hashtable args)
    {

        var itemBase = ResourcePool.Instance.GetIdleUIItem<UIHPItem>(_UIHPItemPrefab.gameObject);
        itemBase.Show(args);
        itemBase.transform.SetParent(transform);
        itemBase.transform.localScale = Vector3.one;

    }

    private void HideItem(Hashtable args)
    {
        UIHPItem hideItem = args["HideItem"] as UIHPItem;
        ResourcePool.Instance.RecvIldeUIItem(hideItem.gameObject);

    }


}

