using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIGlobalBuff : UIBase
{

    #region static funs

    public static void ShowTelantAsyn()
    {
        UIGlobalBuff._ShowType = 1;
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGlobalBuff, UILayer.PopUI, hash);
    }

    public static void ShowAttrAsyn()
    {
        UIGlobalBuff._ShowType = 2;
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGlobalBuff, UILayer.PopUI, hash);
    }

    public static void RefreshBuffs()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGlobalBuff>(UIConfig.UIGlobalBuff);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public static int _ShowType = 1;

    public UIContainerBase _BuffContainer;
    public Text _Desc;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (_ShowType == 1)
        {
            GlobalBuffData.Instance.RefreshTelant();
            _BuffContainer.InitContentItem(GlobalBuffData.Instance._BuffTelantItems);
        }
        else
        {
            GlobalBuffData.Instance.RefreshAttr();
            _BuffContainer.InitContentItem(GlobalBuffData.Instance._BuffAttrItems);
        }
    }

    public void RefreshItems()
    {
        if (_ShowType == 1)
        {
            GlobalBuffData.Instance.RefreshTelant();
            _BuffContainer.InitContentItem(GlobalBuffData.Instance._BuffTelantItems);
        }
        else
        {
            GlobalBuffData.Instance.RefreshAttr();
            _BuffContainer.InitContentItem(GlobalBuffData.Instance._BuffAttrItems);
        }
    }

    #endregion
}

