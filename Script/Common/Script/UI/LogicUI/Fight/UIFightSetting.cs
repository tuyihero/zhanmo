using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIFightSetting : UIPopBase
{
    #region static
    
    
    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightSetting, UILayer.MessageUI, hash);
    }

    #endregion

    #region 

    public Text _Text;
    public Text _Title;
    public Text _BtnOkTx;
    public Text _BtnCancelTx;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _Text.text = Tables.StrDictionary.GetFormatStr(100000);
        _Title.text = Tables.StrDictionary.GetFormatStr(1000008);
        _BtnOkTx.text = Tables.StrDictionary.GetFormatStr(1000000);
        _BtnCancelTx.text = Tables.StrDictionary.GetFormatStr(1000001);
    }

    public void OnBtnExitOk()
    {
        Debug.Log("exit");
        LogicManager.Instance.ExitFight();
        Hide();
    }

    public void OnBtnCancle()
    {
        Hide();
    }

    public void OnBtnHelp()
    {
        UIFightTips.ShowAsyn();
    }

    #endregion
}

