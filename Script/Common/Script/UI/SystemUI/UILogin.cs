using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UILogin : UIBase
{ 
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILogin, UILayer.BaseUI, hash);
    }

    #endregion

    #region params

    public GameObject InitOkTex;
    public Button _BtnStart;

    #endregion

    #region show funs

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        GuiTextDebug.debug("UILogin show");
        InitOkTex.SetActive(false);
        _BtnStart.interactable = false;

        LogicManager.Instance.StartLoadLogic();

        _BtnStart.interactable = true;
        InitOkTex.SetActive(true);
    }

    #endregion

    #region inact

    public void OnBtnStart()
    {
        GuiTextDebug.debug("UILogin OnPointerClick:");

        LogicManager.Instance.StartLogic();
        Destory();

    }

    #endregion
}

