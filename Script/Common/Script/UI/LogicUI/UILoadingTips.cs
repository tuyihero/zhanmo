using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UILoadingTips : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILoadingTips, UILayer.TopUI, hash);
    }

    public static void HideAsyn()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UILoadingTips>(UIConfig.UILoadingTips);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.Hide();
    }

    #endregion

}

