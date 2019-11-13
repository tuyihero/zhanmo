
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIAchievement : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIAchievement, UILayer.PopUI, hash);
    }

    #endregion

    public override void Init()
    {
        base.Init();
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        //_Achievetype.ShowDefaultFirst();

        ShowAchieveClass();
    }

    #region 

    public UIContainerBase _AchieveContainer;

    public void ShowAchieveClass()
    {
        _AchieveContainer.InitContentItem(AchievementData.Instance._AchieveGroup.Values);
    }

    #endregion

}

