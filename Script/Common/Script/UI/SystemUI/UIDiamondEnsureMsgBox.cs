using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIDiamondEnsureMsgBox : UIPopBase
{
    #region 

    public static List<Action> _EnsureActions = new List<Action>();

    public static void Show(string message, Action okAction, Action cancelAction)
    {
        if (_EnsureActions.Contains(okAction))
        {
            okAction.Invoke();
            return;
        }

        Hashtable hash = new Hashtable();
        hash.Add("Message", message);
        hash.Add("OkAction", (Action)okAction);
        hash.Add("CancelAction", cancelAction);
        //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOW_MESSAGEBOX, null, hash);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIDiamondEnsureMsgBox, UILayer.MessageUI, hash);
    }

    #endregion

    #region 

    public Text _MsgText;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _OkAction = null;
        if (hash.ContainsKey("OkAction"))
        {
            _OkAction = (Action)hash["OkAction"];
        }

        _CancelAction = null;
        if (hash.ContainsKey("CancelAction"))
        {
            _CancelAction = (Action)hash["CancelAction"];
        }

        if (hash.ContainsKey("Message"))
        {
            _MsgText.text = (string)hash["Message"];
        }

        transform.SetAsLastSibling();
    }

    #endregion

    #region 

    private Action _OkAction;
    private Action _CancelAction;

    public GameObject m_btnYesNo;
    public GameObject m_btnOK;
    public void BtnOkEvent()
    {
        if (_OkAction != null)
            _OkAction();

        Hide();
    }

    public void BtnCancelEvent()
    {
        if (_CancelAction != null)
            _CancelAction();

        Hide();
    }


    #endregion
}

